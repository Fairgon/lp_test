using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions.EventSystem;

namespace Game.UI
{
    public class PlantingForm : UIForm
    {
        [Header("General")]
        [SerializeField]
        private List<Toggle> toggles = null;
        [SerializeField]
        private Button _plant = null;

        [Header("Gloabal Events")]
        [SerializeField]
        private GlobalEvent _onCreateTask;

        private Cell cell = null;

        public void Init(Cell cell)
        {
            this.cell = cell;
        }

        private void Awake()
        {
            _plant.onClick.AddListener(() => Plant());
        }

        private void Plant()
        {
            PlantTypes type = PlantTypes.Grass;

            if (toggles[0].isOn) 
                type = PlantTypes.Carrot;
            else if (toggles[1].isOn)
                type = PlantTypes.Grass;
            else if (toggles[2].isOn)
                type = PlantTypes.Tree;

            PlantingTask task = new PlantingTask();

            task.Init(cell, FindObjectOfType<PlayerBehaviour>(), type); 

            _onCreateTask?.Invoke(this, task);

            Close();
        }
    }
}