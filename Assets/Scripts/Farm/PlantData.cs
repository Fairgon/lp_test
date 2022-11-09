using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable, CreateAssetMenu(menuName = "Game/New Plant Data")]
    public class PlantData : ScriptableObject
    {
        public PlantTypes Type => _type;
        [SerializeField]
        private PlantTypes _type = PlantTypes.Grass;

        public float GrowthTime => _growthTime;
        [SerializeField]
        private float _growthTime = 3f;

        public HarvestingTypes HarvestingType => _harvestingType;
        [SerializeField]
        private HarvestingTypes _harvestingType = HarvestingTypes.None;

        [SerializeField]
        private List<Plant> plants = null;

        public Plant GetPlant()
        {
            return plants[Random.Range(0, plants.Count - 1)];
        }
    }
}