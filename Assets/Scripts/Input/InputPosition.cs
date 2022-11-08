using UnityEngine;
using System;

namespace InputSystem
{
    [Serializable]
    public struct InputPosition
    {
        /// <summary>
        /// Input position in world coordinates.
        /// </summary>
        public Vector3 World;
        /// <summary>
        /// Input position in screen coordinates.
        /// </summary>
        public Vector2 Screen;
    }
}