using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Extensions.EventSystem;
using UnityEngine.AI;
using Game.UI;

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
        private GameObject _cleanFX = null;
        [SerializeField]
        private Transform _cellParent = null;
        [SerializeField]
        private FieldHUD _hud = null;

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

        public void ResetTask()
        {
            cleaned = false;
        }

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
            _hud.Init(this);
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

                OnBuild?.Invoke(time / _buildTime);

                yield return new WaitForEndOfFrame();
            }

            _dustFX.SetActive(false);

            OnBuildFinish?.Invoke();

            GetComponent<Collider>().enabled = false;
            GetComponent<NavMeshObstacle>().enabled = false;

            _hud.gameObject.SetActive(false);
        }

        public void StartCleaning()
        {
            StartCoroutine(Magic());
        }

        private IEnumerator Magic()
        {
            _cleanFX.SetActive(true);

            var materials = Ivy.materials;

            float height = materials[0].GetFloat("_Height");

            while (materials[0].GetFloat("_Height") > 0)
            {
                height -= Time.deltaTime * 2.5f;

                materials[0].SetFloat("_Height", height);

                Ivy.materials = materials;

                yield return new WaitForEndOfFrame();
            }

            _cleanFX.GetComponent<ParticleSystem>().Stop();

            OnCleaned?.Invoke();
            _onCleaned.Invoke(this, null);

            GetComponent<SelectionComponent>().SetInteractable(false);
            _owner = Owner.Player;

            yield return new WaitForSeconds(1f);

            _cleanFX.SetActive(false);
        }
    }
}