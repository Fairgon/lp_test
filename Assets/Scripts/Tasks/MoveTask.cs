using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public class MoveTask : ITask
    {
        public event Action<ITask> OnStart;
        public event Action<ITask> OnFinish;

        private Vector3 pos;
        private PlayerBehaviour player = null;

        public void Init(PlayerBehaviour player, Vector3 pos)
        {
            this.pos = pos;
            this.player = player;
        }

        public void Start()
        {
            NavMeshMovement movement = player.GetComponent<NavMeshMovement>();

            movement.SetDestination(pos);
        }

        public void Break()
        {

        }
    }
}