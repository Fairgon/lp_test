using System.Collections;
using UnityEngine;
using System;

namespace Game
{
    public class Field : GameBehaviour, ITaskMaker
    {
        public Renderer Ivy => _ivy;
        public Transform Point => _point;

        [Header("General")]
        [SerializeField]
        private Renderer _ivy = null;
        [SerializeField]
        private Transform _point = null;

        public Action OnCleaned;

        public ITask GetTask()
        {
            ClearFieldTask task = new ClearFieldTask();

            PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();

            task.Init(this, player);

            return task;
        }

        public void StartCleaning()
        {
            StartCoroutine(Magic());
        }

        private IEnumerator Magic()
        {
            var materials = Ivy.materials;

            float height = materials[0].GetFloat("_Height");

            while (materials[0].GetFloat("_Height") > 0)
            {
                height -= Time.deltaTime * 5f;

                materials[0].SetFloat("_Height", height);

                Ivy.materials = materials;

                yield return new WaitForEndOfFrame();
            }

            OnCleaned.Invoke();
        }
    }
}