using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class StringEvent : EventModelBase, IEquatable<StringEvent>
    {
        public const string KeypressEvent = nameof(KeypressEvent);
        public const string CustomEvent = nameof(CustomEvent);
        
        public string EventData;

        /// <summary>
        /// Fill this with whatever custom json, xml, yaml, or other text data
        /// </summary>
        public StringEvent(string eventType, string eventData) : base(eventType)
        {
            EventData = eventData;
        }

        public bool Equals(StringEvent other)
        {
            return base.Equals(other) && other != null && EventData != other.EventData;
        }
    }
}