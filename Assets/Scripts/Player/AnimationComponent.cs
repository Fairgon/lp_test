using UnityEngine;
using Extensions.EventSystem;
using System;

namespace Game
{
    public class AnimationComponent : MonoBehaviour
    {
        private readonly string FORWARD = "Forward";
        private readonly string ATTACK = "Attack";
        private readonly string LEVITATING = "Levitating";

        [Header("General")]
        [SerializeField]
        private Animator _animator = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onMove = null;
        [SerializeField]
        private GlobalEvent _onMoveFinish = null;

        public Action<bool> OnLevitating;

        private void OnEnable()
        {
            _onMove.Subscribe(HandleMove);
            _onMoveFinish.Subscribe(HandleMoveFinish);
        }

        public void Attack()
        {
            _animator.SetTrigger(ATTACK);
        }

        public void SetLevitating(bool value)
        {
            _animator.SetBool(LEVITATING, value);

            OnLevitating?.Invoke(value);
        }

        private void HandleMove(object sender, object _)
        {
            _animator.SetFloat(FORWARD, 1f);
        }

        private void HandleMoveFinish(object sender, object _)
        {
            _animator.SetFloat(FORWARD, 0f);
        }
    }
}