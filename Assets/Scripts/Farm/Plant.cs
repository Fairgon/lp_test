using System.Collections;
using UnityEngine;

namespace Game
{
    public enum PlantTypes
    {
        Carrot,
        Grass,
        Tree
    }

    public enum HarvestingTypes
    {
        None,
        Harvesting,
        Cut
    }

    public class Plant : MonoBehaviour
    {
        public PlantTypes Type;
        public float GrowthTime;
        public HarvestingTypes HarvestingType;

        public void Init(PlantTypes type, float growthTime, HarvestingTypes harvestingType)
        {
            Type = type;
            GrowthTime = growthTime;
            HarvestingType = harvestingType;
        }

        public void Process(float value)
        {
            value = value + 0.2f;

            transform.localScale = Vector3.one * value;
        }
    }
}