using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Extensions.EventSystem;
using System;

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

        [Space, SerializeField]
        private GlobalEvent _onDragStart = null;
        [SerializeField]
        private GlobalEvent _onDrag = null;
        [SerializeField]
        private GlobalEvent _onDragFinish = null;

        public Action<InputData> OnMove;
        public Action<InputData> OnStart;

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
        List<GameObject> hittedObjects;

        private MainController mainController = null;

        private void Awake()
        {
            mainController = new MainController();

            mainController.MainMap.SetCallbacks(this);

            mainController.MainMap.Enable();
        }

        private void Update()
        {
            ray = _gameCamera.ScreenPointToRay(MousePosition);

            Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);

            hittedObjects = Physics.RaycastAll(ray, Mathf.Infinity, _layerMask).Select(h => h.transform.gameObject).ToList();

            if (GeneralData == null)
                GeneralData = new InputData(hit.point, hittedObjects);

            GeneralData.Position = hit.point;
            GeneralData.Objects = hittedObjects;

            Drag();
            MianAction();
        }

        private void Drag()
        {
            if (RightInput)
            {
                ray = _gameCamera.ScreenPointToRay(MousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, _dragLayerMask);

                if (DragData == null)
                {
                    DragData = new InputData(hit.point);

                    _onDragStart.Invoke(this, null);
                }

                DragData.Position = hit.point;

                if (Vector3.Distance(DragData.StartPosition, DragData.Position) < INPUT_SAFE_DRAG_OFFSET)
                    return;

                _onDrag.Invoke(this, DragData);

                return;
            }

            if (DragData == null)
                return;

            DragData = null;

            _onDragFinish.Invoke(this, null);
            
        }

        private void MianAction()
        {
            if (LeftInput)
            {
                ray = _gameCamera.ScreenPointToRay(MousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);

                List<GameObject> hittedObjects = Physics.RaycastAll(ray, Mathf.Infinity, _layerMask).Select(h => h.transform.gameObject).ToList();

                if (MainActionData == null)
                {
                    MainActionData = new InputData(hit.point, hittedObjects);

                    OnStart?.Invoke(MainActionData);
                    _onStart.Invoke(this, MainActionData);
                }

                MainActionData.Position = hit.point;
                MainActionData.Objects = hittedObjects;

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