using System.Collections;
using UnityEngine;
using Extensions.EventSystem;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private FieldForm _fieldForm = null;
        [SerializeField]
        private PlantingForm _plantingForm = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onFieldCleaned = null;
        [SerializeField]
        private GlobalEvent _onCellClicked = null;

        private void OnEnable()
        {
            _onCellClicked.Subscribe(HandleCellClick);
            _onFieldCleaned.Subscribe(HandleFieldCleaned);
        }

        private void OnDisable()
        {
            _onCellClicked.Unsubscribe(HandleCellClick);
            _onFieldCleaned.Unsubscribe(HandleFieldCleaned);
        }

        private void HandleCellClick(object sender, object data)
        {
            Cell cell = (Cell)data;

            if (cell.IsEmpty)
            {
                if (cell.HasPlantingTask)
                    return;

                _plantingForm.Init(cell);

                _plantingForm.Open();
            }
        }

        private void HandleFieldCleaned(object sender, object _)
        {
            _fieldForm.Init((Field)sender);

            _fieldForm.Open();
        }
    }
}