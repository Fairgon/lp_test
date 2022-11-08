using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct CameraPoint
    {
        [SerializeField]
        public Vector3 Position;
        [SerializeField]
        public Vector3 Rotation;
    }
}