using System.Collections;
using UnityEngine;

namespace Game
{
    public class SelectionComponent : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private GameObject _sprite = null;

        public void Set(bool value)
        {
            _sprite.SetActive(value);
        }
    }
}