using UnityEngine;
using System;

namespace Extensions.EventSystem
{
    [Serializable, CreateAssetMenu(menuName = "Extensions/New Global Event")]
    public class GlobalEvent : ScriptableObject
    {
        [Header("Annotation")]
        [SerializeField, Tooltip("Name (or type) of regular event sender.")]
        private string _sender = "GameObject";
        /// <summary>
        /// Name (or type) of regular event sender.
        /// </summary>
        public string Sender => _sender;
        [SerializeField, Tooltip("Event data type.")]
        private string _data = "null";
        /// <summary>
        /// Event data type.
        /// </summary>
        public string Data => _data;

        private event EventHandler<object> handler;

        /// <summary>
        /// Add new subscriber.
        /// </summary>
        public void Subscribe(EventHandler<object> subscriber)
        {
            handler += subscriber;
        }

        /// <summary>
        /// Remove subscriber.
        /// </summary>
        public void Unsubscribe(EventHandler<object> subscriber)
        {
            handler -= subscriber;
        }

        /// <summary>
        /// Invoke the global event without any specific data.
        /// </summary>
        /// <param name="sender">Reference to sender.</param>
        public void Invoke(object sender)
        {
            handler?.Invoke(sender, null);
        }

        /// <summary>
        /// Invoke the global event.
        /// </summary>
        /// <param name="sender">Reference to sender.</param>
        /// <param name="data">Specific event data. Can be null.</param>
        public void Invoke(object sender, object data)
        {
            handler?.Invoke(sender, data);
        }
    }
}