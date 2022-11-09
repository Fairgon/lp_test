using System.Collections.Generic;
using UnityEngine;
using Extensions.EventSystem;

namespace Game
{
    public class PlantingController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private List<PlantData> plantsData = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onNeedPlant = null;

        private void OnEnable()
        {
            _onNeedPlant.Subscribe(HandleNeedPlant);
        }

        private void OnDisable()
        {
            _onNeedPlant.Unsubscribe(HandleNeedPlant);
        }

        private void HandleNeedPlant(object sender, object _)
        {
            Cell cell = (Cell)sender;
            PlantTypes type = (PlantTypes)_;

            PlantData data = plantsData.Find(x => x.Type == type);

            Plant plant = Instantiate(data.GetPlant());

            plant.Init(data.Type, data.GrowthTime, data.HarvestingType);

            cell.Plant(plant);
        }
    }
}