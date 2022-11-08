using System.Collections;
using UnityEngine;
using Extensions.EventSystem;

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

        private void OnEnable()
        {
            _onMove.Subscribe(HandleMove);
            _onMoveFinish.Subscribe(HandleMoveFinish);
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