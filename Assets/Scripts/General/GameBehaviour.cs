using System.Collections;
using UnityEngine;

namespace Game
{
    public enum Owner
    {
        Player,
        AI,
        Neutral
    }

    public class GameBehaviour : MonoBehaviour
    {
        public Owner Owner => _owner;
        [SerializeField]
        protected Owner _owner = Owner.Neutral;
    }
}