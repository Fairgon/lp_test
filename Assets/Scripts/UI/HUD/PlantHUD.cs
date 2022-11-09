using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlantHUD : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Image _image;

        private Cell cell;

        public void Init(Cell cell)
        {
            this.cell = cell;

            cell.OnGrowing += HandleGroving;
            cell.OnGrown += HandleGrown;
        }

        private void HandleGrown()
        {
            gameObject.SetActive(false);
        }

        private void HandleGroving(float value)
        {
            if (!gameObject.activeInHierarchy)
                gameObject.SetActive(true);

            _image.fillAmount = value;
        }
    }
}