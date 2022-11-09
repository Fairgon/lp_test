using UnityEngine;
using System.Collections.Generic;

namespace InputSystem
{
    public class InputData
    {
        /// <summary>
        /// Input start position. Readonly.
        /// </summary>
        public readonly Vector3 StartPosition;
        /// <summary>
        /// Current input position.
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// Hitted object.
        /// </summary>
        public GameObject Object;

        public InputData(Vector3 startPoint, GameObject hittedObject)
        {
            StartPosition = startPoint;
            Position = StartPosition;

            Object = hittedObject;
        }

        public InputData(Vector3 startPoint)
        {
            StartPosition = startPoint;
            Position = StartPosition;
        }
    }
}