using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class CurrencyPanel : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private CurrencyData _currency = null;
        [SerializeField]
        private TextMeshProUGUI label = null;

        private void Awake()
        {
            label.text = _currency.Value.ToString();
        }

        private void OnEnable()
        {
            _currency.OnChange += HandleChange;
        }

        private void OnDisable()
        {
            _currency.OnChange -= HandleChange;
        }

        private void HandleChange(int value)
        {
            label.text = value.ToString();

            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            float time = 0.1f;

            while(time > 0f)
            {
                time -= Time.deltaTime;

                label.rectTransform.localScale += Vector3.one * 0.05f;

                yield return new WaitForEndOfFrame();
            }

            label.rectTransform.localScale = Vector3.one;
        }
    }
}