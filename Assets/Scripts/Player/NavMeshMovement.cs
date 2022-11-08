using System.Collections;
using UnityEngine;
using Extensions.EventSystem;
using UnityEngine.AI;
using System;

namespace Game
{
    public class NavMeshMovement : MonoBehaviour
    {
        private const float STOP_DISTANCE = 0.5f;

        [Header("General")]
        [SerializeField]
        private NavMeshAgent _agent = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onMoveStart = null;
        [SerializeField]
        private GlobalEvent _onMoveFinish = null;
        [SerializeField]
        private GlobalEvent _onMove = null;

        public Action OnFinish;

        public Vector3 Destination
        {
            get
            {
                return Target == null ? destination : Target.position;
            }
        }

        public Vector3 RealDestination
        {
            get
            {
                if (_agent.path.corners.Length == 0)
                    return Destination;

                return _agent.pathEndPosition;
            }
        }

        public Transform Target => target;

        private Transform target;
        private Vector3 destination;

        private Coroutine moveCoroutine;

        private void OnDisable()
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = null;
        }

        public void SetDestination(Vector3 destination, float stopDistance = STOP_DISTANCE)
        {
            target = null;

            this.destination = destination;
            _agent.stoppingDistance = stopDistance;

            _agent.SetDestination(destination);

            _onMoveStart.Invoke(this, Destination);

            if (moveCoroutine == null)
                moveCoroutine = StartCoroutine(Moving());
        }

        public void SertTarget(Transform target, float stopDistance = STOP_DISTANCE)
        {
            this.target = target;
            _agent.stoppingDistance = stopDistance;

            _onMoveStart.Invoke(this, target.position);

            if (moveCoroutine == null)
                moveCoroutine = StartCoroutine(Moving());
        }

        private IEnumerator Moving()
        {   
            yield return new WaitWhile(() => _agent.pathPending);
            
            float distance = (transform.position - Destination).magnitude;

            while (distance > _agent.stoppingDistance)
            {
                if (Target != null)
                    _agent.SetDestination(Target.position);

                _onMove.Invoke(this, Destination);

                yield return new WaitForEndOfFrame();

                distance = (transform.position - RealDestination).magnitude;
            }

            moveCoroutine = null;

            OnFinish?.Invoke();
            _onMoveFinish.Invoke(this, null);
        }
    }
}