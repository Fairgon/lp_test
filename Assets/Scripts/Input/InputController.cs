using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Extensions.EventSystem;
using System;
using UnityEngine.EventSystems;

namespace InputSystem
{
    public class InputController : MonoBehaviour, MainController.IMainMapActions
    {
        private const float INPUT_SAFE_OFFSET = 0.1f;
        private const float INPUT_SAFE_DRAG_OFFSET = 1f;

        public static bool LeftInput { get; private set; }
        public static bool RightInput { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        [Header("General")]
        [SerializeField]
        private LayerMask _layerMask = new LayerMask();
        [SerializeField]
        private LayerMask _dragLayerMask = new LayerMask();

        [Header("Scene Objects")]
        [SerializeField]
        private Camera _gameCamera = null;

        [Header("Global Events")]
        [SerializeField]
        protected GlobalEvent _onStart = null;
        [SerializeField]
        private GlobalEvent _onFinish = null;
        [SerializeField]
        private GlobalEvent _onMove = null;
        [SerializeField]
        private GlobalEvent _onHitObject = null;

        [Space, SerializeField]
        private GlobalEvent _onDragStart = null;
        [SerializeField]
        private GlobalEvent _onDrag = null;
        [SerializeField]
        private GlobalEvent _onDragFinish = null;

        public Action<InputData> OnMove;
        public Action<InputData> OnStart;

        public Action OnDragStart;
        public Action OnDragFinish;
        public Action<GameObject> OnHitObject;

        /// <summary>
        /// General input data.
        /// </summary>
        public InputData GeneralData { get; private set; }

        /// <summary>
        /// Left click input data;
        /// </summary>
        public InputData MainActionData { get; private set; }

        /// <summary>
        /// Input data for camera moving.
        /// </summary>
        public InputData DragData { get; private set; }

        private Ray ray;
        private RaycastHit hit;

        public GameObject HittedObject => hittedObject;
        private GameObject hittedObject;
        private GameObject newHittedObject;
        private Vector3 dragPos;

        private MainController mainController = null;

        private void Awake()
        {
            mainController = new MainController();

            mainController.MainMap.SetCallbacks(this);

            mainController.MainMap.Enable();
        }

        private void Update()
        {
            if (IsMouseOverUi)
            {
                GeneralData = null;

                hittedObject = null;

                OnHitObject?.Invoke(hittedObject);
                _onHitObject?.Invoke(this, hittedObject);

                return;
            }
            
            ray = _gameCamera.ScreenPointToRay(MousePosition);

            newHittedObject = Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask) ? hit.transform.gameObject : null;

            if(hittedObject != newHittedObject)
            {
                hittedObject = newHittedObject;

                OnHitObject?.Invoke(hittedObject);
                _onHitObject?.Invoke(this, hittedObject);
            }

            if (GeneralData == null)
                GeneralData = new InputData(hit.point);

            GeneralData.Position = hit.point;

            Drag();
            MianAction();
        }

        public static bool IsMouseOverUi
        {
            get
            {
                EventSystem eventSystem = EventSystem.current;

                return (eventSystem != null && eventSystem.IsPointerOverGameObject());
            }
        }

        private void Drag()
        {
            if (RightInput)
            {
                ray = _gameCamera.ScreenPointToRay(MousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, _dragLayerMask);

                dragPos = hit.point;
                dragPos.y = 0f;

                if (DragData == null)
                {
                    DragData = new InputData(dragPos);

                    OnDragStart?.Invoke();
                    _onDragStart.Invoke(this, null);
                }

                DragData.Position = dragPos;

                if (Vector3.Distance(DragData.StartPosition, DragData.Position) < INPUT_SAFE_DRAG_OFFSET)
                    return;

                _onDrag.Invoke(this, DragData);

                return;
            }

            if (DragData == null)
                return;

            DragData = null;

            OnDragFinish?.Invoke();
            _onDragFinish.Invoke(this, null);
        }

        private void MianAction()
        {
            if (LeftInput)
            {
                if (MainActionData == null)
                {
                    MainActionData = new InputData(hit.point, hittedObject);

                    OnStart?.Invoke(MainActionData);
                    _onStart.Invoke(this, MainActionData);
                }

                MainActionData.Position = hit.point;
                MainActionData.Object = hittedObject;

                if (Vector3.Distance(MainActionData.StartPosition, MainActionData.Position) < INPUT_SAFE_OFFSET)
                    return;

                OnMove?.Invoke(MainActionData);
                _onMove.Invoke(this, MainActionData);

                return;
            }

            if (MainActionData == null)
                return;

            _onFinish.Invoke(this, MainActionData);

            MainActionData = null;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            RightInput = context.phase != InputActionPhase.Canceled;
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            LeftInput = context.phase != InputActionPhase.Canceled;
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            //MiddleInput = context.phase != InputActionPhase.Canceled;
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }
    }
}