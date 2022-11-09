using System.Collections;
using UnityEngine;

namespace Game
{
    public class Rotator : MonoBehaviour
    {
        private void FixedUpdate()
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * 5f);
        }
    }
}