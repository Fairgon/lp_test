using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public class ClearFieldTask : ITask
    {
        public event Action<ITask> OnStart;
        public event Action<ITask> OnFinish;

        public bool CanBreak { get; set; } = false;

        private Field field = null;
        private PlayerBehaviour player = null;
        private AnimationComponent animation = null;
        private NavMeshMovement movement = null;
        public void Init(Field field, PlayerBehaviour player)
        {
            this.field = field;
            this.player = player;
        }

        public void Start()
        {
            CanBreak = true;

            movement = player.GetComponent<NavMeshMovement>();

            movement.SetDestination(field.Point.position);
            movement.OnFinish += HandleOnPosition;

            OnStart?.Invoke(this);
        }

        private void HandleOnPosition()
        {
            CanBreak = false;

            movement.OnFinish -= HandleOnPosition;

            animation = player.GetComponent<AnimationComponent>();

            animation.SetLevitating(true);

            field.StartCleaning();

            field.OnCleaned += HandleCleaned;
        }

        private void HandleCleaned()
        {
            field.OnCleaned -= HandleCleaned;

            animation.SetLevitating(false);

            OnFinish?.Invoke(this);
        }

        public void Break()
        {
            field.ResetTask();

            movement.OnFinish -= HandleOnPosition;
        }
    }
}