using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public class UIForm : MonoBehaviour
    {
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}