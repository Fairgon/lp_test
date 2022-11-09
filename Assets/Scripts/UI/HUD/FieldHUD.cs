using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FieldHUD : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Slider _slider;

        private Field field;

        public void Init(Field field)
        {
            this.field = field;

            field.OnBuild += HandleBuild;

            gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            field.OnBuild -= HandleBuild;
        }

        private void HandleBuild(float value)
        {
            _slider.value = value;
        }
    }
}