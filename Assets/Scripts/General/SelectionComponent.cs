using System.Collections;
using UnityEngine;

namespace Game
{
    public class SelectionComponent : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private GameObject _sprite = null;

        private bool interactable = true;

        public void Set(bool value)
        {
            if (interactable == false)
                return;

            _sprite.SetActive(value);
        }

        public void SetInteractable(bool value)
        {
            interactable = value;

            if (value == false)
                _sprite.SetActive(false);
        }
    }
}