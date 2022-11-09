using System.Collections;
using UnityEngine;
using Extensions.EventSystem;
using Game.UI;
using System;

namespace Game
{
    public class Cell : GameBehaviour, ITaskMaker
    {
        [Header("General")]
        [SerializeField]
        private Transform _point = null;
        [SerializeField]
        private PlantHUD _plantHUD = null;

        [Header("FX")]
        [SerializeField]
        private GameObject _plantingFX = null;
        [SerializeField]
        private GameObject _cuttingFX = null;
        [SerializeField]
        private GameObject _carrotFX = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onNeedPlant = null;

        [Header("Currencies")]
        [SerializeField]
        private CurrencyData _carrot = null;
        [SerializeField]
        private CurrencyData _exp = null;

        public Action OnGrown;
        public Action<float> OnGrowing;

        public Action OnHarvestingFinish;
        public Action OnCuttingFinish;

        public bool PlantReady { get; private set; }
        public bool IsEmpty => plant == null;

        private Plant plant = null;

        public bool HasPlantingTask { get; private set; }

        private bool isBusy = false;

        private void Awake()
        {
            _plantHUD.Init(this);
        }

        public void PrepareToPlanting()
        {
            HasPlantingTask = true;
        }

        public ITask GetTask()
        {
            if (IsEmpty)
                return null;

            if (isBusy)
                return null;

            if (plant.Type == PlantTypes.Carrot)
            {
                isBusy = true;

                HarvestingTask task = new HarvestingTask();

                task.Init(this, FindObjectOfType<PlayerBehaviour>());

                return task;
            }
            else if (plant.Type == PlantTypes.Grass)
            {
                isBusy = true;

                CuttingTask task = new CuttingTask();

                task.Init(this, FindObjectOfType<PlayerBehaviour>());

                return task;
            }

            return null;
        }

        public void ResetTasks()
        {
            HasPlantingTask = false;
            isBusy = false;
        }

        public void StartPlanting(PlantTypes type)
        {
            _onNeedPlant.Invoke(this, type);
        }

        public void Plant(Plant plant)
        {
            _plantingFX.SetActive(true);

            this.plant = plant;

            this.plant.transform.position = _point.position;

            StartCoroutine(GrowthCoroutine());

            _owner = Owner.Player;
        }

        public IEnumerator GrowthCoroutine()
        {
            float time = 0f;

            while (time <= plant.GrowthTime)
            {
                time += Time.deltaTime;

                plant.Process(time / plant.GrowthTime);

                OnGrowing?.Invoke(time / plant.GrowthTime);

                yield return new WaitForEndOfFrame();
            }

            _exp.Add((int)plant.GrowthTime);

            _plantingFX.SetActive(false);

            OnGrown?.Invoke();
            PlantReady = true;

            if (plant.Type == PlantTypes.Carrot)
            {
                _owner = Owner.Neutral;
            }
            else if (plant.Type == PlantTypes.Grass)
            {
                _owner = Owner.AI;
            }
        }

        public void StartHarvesting()
        {
            StartCoroutine(HarvestingCoroutine());
        }

        private IEnumerator HarvestingCoroutine()
        {
            GameObject carrot = Instantiate(plant.transform.GetChild(0), _point.position, _point.rotation, null).gameObject;

            carrot.AddComponent<CapsuleCollider>();
            Rigidbody rgb = carrot.AddComponent<Rigidbody>();

            rgb.AddForce(Vector3.up * 2f, ForceMode.Impulse);

            Destroy(plant.gameObject);
            plant = null;

            ResetTasks();

            _carrot.Add(1);

            OnHarvestingFinish?.Invoke();

            _owner = Owner.Neutral;

            yield return new WaitForSeconds(1f);

            _carrotFX.transform.position = carrot.transform.position;
            _carrotFX.SetActive(true);

            Destroy(carrot);

            yield return new WaitForSeconds(1f);

            _carrotFX.SetActive(false);
        }

        public void StartCutting()
        {
            StartCoroutine(CuttingCoroutine());
        }

        private IEnumerator CuttingCoroutine()
        {
            _cuttingFX.SetActive(true);
            
            Renderer renderer = plant.GetComponent<Renderer>();

            var materials = renderer.materials;

            float height = 1f;

            while (materials[0].GetFloat("_Height") > 0)
            {
                height -= Time.deltaTime * 1f;

                materials[0].SetFloat("_Height", height);

                renderer.materials = materials;

                yield return new WaitForEndOfFrame();
            }

            Destroy(plant.gameObject);
            plant = null;

            ResetTasks();

            OnCuttingFinish?.Invoke();

            _owner = Owner.Neutral;

            yield return new WaitForSeconds(0.5f);

            _cuttingFX.SetActive(false);
        }
    }
}