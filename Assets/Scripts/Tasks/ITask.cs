using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public interface ITask
    {
        public event Action<ITask> OnStart;
        public event Action<ITask> OnFinish;

        public bool CanBreak { get; set; } 

        public void Start();
        public void Break();
    }
}