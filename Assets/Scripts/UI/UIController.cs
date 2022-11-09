using System.Collections;
using UnityEngine;
using Extensions.EventSystem;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private FieldForm _fieldForm = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onFieldCleaned = null;

        private void OnEnable()
        {
            _onFieldCleaned.Subscribe(HandleFieldCleaned);
        }

        private void OnDisable()
        {
            _onFieldCleaned.Unsubscribe(HandleFieldCleaned);
        }

        private void HandleFieldCleaned(object sender, object _)
        {
            _fieldForm.Init((Field)sender);

            _fieldForm.Open();
        }
    }
}