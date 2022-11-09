using UnityEngine;
using System;

namespace Game
{
    public class HarvestingTask : ITask
    {
        public event Action<ITask> OnStart;
        public event Action<ITask> OnFinish;

        public bool CanBreak { get; set; } = false;

        private Cell cell = null;
        private PlayerBehaviour player = null;


        private AnimationComponent animation = null;
        private NavMeshMovement movement = null;

        public void Init(Cell cell, PlayerBehaviour player)
        {
            this.cell = cell;
            this.player = player;

            cell.PrepareToPlanting();
        }

        public void Start()
        {
            movement = player.GetComponent<NavMeshMovement>();

            movement.SetDestination(cell.transform.position, 2f);
            movement.OnFinish += HandleOnPosition;

            OnStart?.Invoke(this);
        }

        private void HandleOnPosition()
        {
            movement.OnFinish -= HandleOnPosition;

            animation = player.GetComponent<AnimationComponent>();

            animation.Attack();

            cell.OnHarvestingFinish += HandleHarvestingFinish;

            cell.StartHarvesting();
        }

        private void HandleHarvestingFinish()
        {
            cell.OnHarvestingFinish -= HandleHarvestingFinish;

            OnFinish?.Invoke(this);
        }

        public void Break()
        {
            cell.ResetTasks();

            movement.OnFinish -= HandleOnPosition;
        }
    }
}