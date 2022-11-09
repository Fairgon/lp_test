using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public class PlantingTask : ITask
    {
        public event Action<ITask> OnStart;
        public event Action<ITask> OnFinish;

        public bool CanBreak { get; set; } = false;

        private Cell cell = null;
        private PlantTypes type = PlantTypes.Grass;
        private PlayerBehaviour player = null;


        private AnimationComponent animation = null;
        private NavMeshMovement movement = null;

        public void Init(Cell cell, PlayerBehaviour player, PlantTypes type)
        {
            this.cell = cell;
            this.player = player;
            this.type = type;

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

            cell.StartPlanting(type);

            OnFinish?.Invoke(this);
        }

        public void Break()
        {
            cell.ResetTasks();

            movement.OnFinish -= HandleOnPosition;
        }
    }
}