using System.Collections;
using UnityEngine;

namespace Game
{
    public class LookAtMe : MonoBehaviour
    {
        private Transform cam;

        void Start()
        {
            cam = Camera.main.transform;
        }

        void LateUpdate()
        {
            if (cam == null)
                return;

            transform.LookAt(transform.position + cam.forward);        
        }
    }
}