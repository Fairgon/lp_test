using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Extensions.EventSystem;

namespace InputSystem
{
    public class InputController : MonoBehaviour, MainController.IMainMapActions
    {
        private const float INPUT_SAFE_OFFSET = 0.1f;
        private const float INPUT_SAFE_DRAG_OFFSET = 1f;

        public static bool LeftInput { get; private set; }
        public static bool MiddleInput { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        [Header("General")]
        [SerializeField]
        private LayerMask _layerMask = new LayerMask();

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

        /// <summary>
        /// Current input data. Can be null;
        /// </summary>
        public InputData Current { get; private set; }

        public InputData Drag { get; private set; }

        private Ray ray;
        private RaycastHit hit;

        private MainController mainController = null;

        private void Awake()
        {
            mainController = new MainController();

            mainController.MainMap.SetCallbacks(this);

            mainController.MainMap.Enable();
        }

        private void Update()
        {
            if (MiddleInput)
            {
                ray = _gameCamera.ScreenPointToRay(MousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);

                if(Drag == null)
                {
                    Drag = new InputData(hit.point);

                    _onDragStart.Invoke(this, null);
                }
                
                Drag.Position = hit.point;

                if (Vector3.Distance(Drag.StartPosition, Drag.Position) < INPUT_SAFE_DRAG_OFFSET)
                    return;

                _onDrag.Invoke(this, Drag);

                return;
            }

            if (Drag != null)
            {
                Drag = null;

                _onDragFinish.Invoke(this, null);
            }

            if (LeftInput)
            {
                ray = _gameCamera.ScreenPointToRay(MousePosition);

                Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);

                List<GameObject> hittedObjects = Physics.RaycastAll(ray, Mathf.Infinity, _layerMask).Select(h => h.transform.gameObject).ToList();

                if (Current == null)
                {
                    Current = new InputData(hit.point, hittedObjects);

                    _onStart.Invoke(this, Current);
                }

                Current.Position = hit.point;
                Current.Objects = hittedObjects;

                if (Vector3.Distance(Current.StartPosition, Current.Position) < INPUT_SAFE_OFFSET)
                    return;

                _onMove.Invoke(this, Current);

                return;
            }

            if (Current == null)
                return;

            _onFinish.Invoke(this, Current);

            Current = null;
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            LeftInput = context.phase != InputActionPhase.Canceled;
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) ;

            MiddleInput = context.phase != InputActionPhase.Canceled;
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }
    }
}