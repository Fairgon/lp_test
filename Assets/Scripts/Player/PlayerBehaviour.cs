using System.Collections;
using UnityEngine;
using Extensions.EventSystem;
using InputSystem;

namespace Game
{
    public class PlayerBehaviour : GameBehaviour
    {
        [Header("General")]
        [SerializeField]
        private NavMeshMovement _movement = null;
        
        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}