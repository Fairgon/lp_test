using System.Collections;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : NavMeshMovement
    {
        [Header("TargetHUD")]
        [SerializeField]
        private GameObject _targetSpritePrefab = null;

        private Transform targetSprite = null;

        public override void SetDestination(Vector3 destination, float stopDistance = 0.5F)
        {
            if(targetSprite == null)
                targetSprite = Instantiate(_targetSpritePrefab).transform;

            targetSprite.position = destination + Vector3.up * 0.1f;

            base.SetDestination(destination, stopDistance);
        }
    }
}