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
        /// List of the objects under current 
        /// </summary>
        public List<GameObject> Objects = null;

        public InputData(Vector3 startPoint, List<GameObject> hittedObjects)
        {
            StartPosition = startPoint;
            Position = StartPosition;

            Objects = hittedObjects;
        }

        public InputData(Vector3 startPoint)
        {
            StartPosition = startPoint;
            Position = StartPosition;
        }
    }
}