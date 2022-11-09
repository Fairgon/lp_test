using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Extensions.EventSystem;
using UnityEngine.AI;

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
        [SerializeField]
        private List<Transform> _cellPoints = null;
        [SerializeField]
        private GameObject _dustFX = null;
        [SerializeField]
        private Transform _cellParent = null;

        [Header("Cells Prefabs")]
        [SerializeField]
        private GameObject _cell = null;
        [SerializeField]
        private GameObject _emptySell = null;

        [Header("Build Settings")]
        [SerializeField]
        private float _buildTime = 3f;

        [Header("Gloval Events")]
        [SerializeField]
        private GlobalEvent _onCleaned = null;

        public Action OnCleaned;
        public Action<float> OnBuild;
        public Action OnBuildFinish;

        private List<bool> buildData = null;

        private bool cleaned = false;

        public ITask GetTask()
        {
            if (cleaned)
                return null;

            ClearFieldTask task = new ClearFieldTask();

            PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();

            task.Init(this, player);

            cleaned = true;

            return task;
        }

        public void Build(List<bool> buildData)
        {
            this.buildData = buildData;

            StartCoroutine(BuildCoroutine());

            _dustFX.SetActive(true);
        }

        private IEnumerator BuildCoroutine()
        {
            float time = 0f;
            int i = 0;

            while (time <= _buildTime)
            {
                if (time > (_buildTime / 9f) * i)
                {
                    Instantiate(buildData[i] ? _cell : _emptySell, _cellPoints[i].position, _cellPoints[i].rotation, _cellParent);

                    i++;
                }

                time += Time.deltaTime;

                OnBuild?.Invoke(time);

                yield return new WaitForEndOfFrame();
            }

            _dustFX.SetActive(false);

            OnBuildFinish?.Invoke();

            GetComponent<Collider>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;
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

            OnCleaned?.Invoke();
            _onCleaned.Invoke(this, null);
        }
    }
}