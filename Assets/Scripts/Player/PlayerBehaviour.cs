using System.Collections;
using UnityEngine;
using Extensions.EventSystem;
using InputSystem;

namespace Game
{
    public class PlayerBehaviour : CharacterBehaviour
    {
        [Header("General")]
        [SerializeField]
        private NavMeshMovement _movement = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onInputStart = null;
        [SerializeField]
        private GlobalEvent _onInputMove = null;

        private void OnEnable()
        {
            _onInputStart.Subscribe(HandleInputStart);
            _onInputMove.Subscribe(HandleInputMove);
        }

        private void OnDisable()
        {
            _onInputStart.Subscribe(HandleInputStart);
            _onInputMove.Subscribe(HandleInputMove);
        }

        private void HandleInputStart(object sender, object data)
        {
            InputData inputData = (InputData)data;

            _movement.SetDestination(inputData.Position);
    }

        private void HandleInputMove(object sender, object data)
        {
            InputData inputData = (InputData)data;

            _movement.SetDestination(inputData.Position);
        }
    }
}