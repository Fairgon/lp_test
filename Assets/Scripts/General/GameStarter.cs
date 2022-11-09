using System.Collections;
using UnityEngine;

namespace Game
{
    public class GameStarter : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Cameraman _cameraman = null;
        [SerializeField]
        private CameraPoint _cameraPoint;
        [SerializeField]
        private PlayerBehaviour _player = null;

        private void Start()
        {
            _cameraman.Show(_player.transform, _cameraPoint);
        }
    }
}