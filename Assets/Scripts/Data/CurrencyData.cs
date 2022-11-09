using System.Collections;
using UnityEngine;
using System;
using Extensions.EventSystem;

namespace Game
{
    [Serializable, CreateAssetMenu(menuName = "Game/New Currency Data")]
    public class CurrencyData : ScriptableObject
    {
        public int Value { get; private set; }

        [Header("General")]
        [SerializeField]
        private int _initValue = 0;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onChange = null;

        public Action<int> OnChange;

        public void Add(int value)
        {
            Value += value;

            OnChange?.Invoke(Value);
            _onChange.Invoke(this, Value);
        }
    }
}