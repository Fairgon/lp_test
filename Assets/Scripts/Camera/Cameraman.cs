using UnityEngine;
using System.Collections;
using System;
using Extensions.EventSystem;
using InputSystem;

namespace Game
{
    public class Cameraman : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private CameraPoint _defaultPoint;
        [SerializeField]
        private Transform _axis = null;
        [SerializeField]
        private float _rotationSpeed = 5f;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onPosition = null;

        [Space, SerializeField]
        private GlobalEvent _onDragStart = null;
        [SerializeField]
        private GlobalEvent _onDrag = null;

        public Transform Target { get; private set; }
        public CameraPoint Offset { get; private set; }

        public event EventHandler OnPosition;
        public bool IsOnPosition { get; private set; } = false;

        private Coroutine coroutine = null;
        private Vector3 startDragPos;

        private void Awake()
        {
            Offset = _defaultPoint;
        }

        private void OnEnable()
        {
            _onDragStart.Subscribe(HandleDragStart);
            _onDrag.Subscribe(HandleDrag);
        }

        private void OnDisable()
        {
            _onDragStart.Unsubscribe(HandleDragStart);
            _onDrag.Unsubscribe(HandleDrag);
        }

        private void HandleDragStart(object sender, object _)
        {
            startDragPos = transform.position;
        }

        private void HandleDrag(object seder, object _)
        {
            InputData data = (InputData)_;

            transform.position = Vector3.Lerp(transform.position, startDragPos + (data.StartPosition - data.Position), Time.fixedDeltaTime * 15f);
        }

        public void Show(Transform target, CameraPoint point)
        {
            this.Target = target;
            Offset = point;

            if(coroutine == null)
                coroutine = StartCoroutine(ShowCoroutine());
        }

        private IEnumerator ShowCoroutine()
        {
            float dist = (transform.position - Target.position).magnitude + (_axis.localPosition - Offset.Position).magnitude;

            while (dist > 0.2f)
            {
                transform.position = Vector3.Lerp(transform.position, Target.position, Time.fixedDeltaTime * 2f);

                _axis.localPosition = Vector3.Lerp(_axis.localPosition, Offset.Position, Time.fixedDeltaTime * 2f);

                transform.LookAt(Target.position);

                yield return new WaitForEndOfFrame();

                dist = (transform.position - Target.position).magnitude + (_axis.localPosition - Offset.Position).magnitude;
            }

            _onPosition.Invoke(this, Target.position);
            coroutine = null;
        }

        private void FixedUpdate()
        {
            if (Target != null)
                return;

        }

        /// <summary>
        /// Return camera to default position.
        /// </summary>
        public void Reset(bool instant = true)
        {
            if (instant)
            {
                transform.position = _defaultPoint.Position;
                transform.rotation = Quaternion.Euler(_defaultPoint.Rotation);

                return;
            }
        }

        /// <summary>
        /// Reset camera follow target.
        /// </summary>
        public void ResetTarget()
        {
            Target = null;
        }
        
#if UNITY_EDITOR
        [ContextMenu("To Default Point")]
        private void ResetCameraPoint()
        {
            transform.position = _defaultPoint.Position;
            transform.rotation = Quaternion.Euler(_defaultPoint.Rotation);
        }
#endif
    }
}