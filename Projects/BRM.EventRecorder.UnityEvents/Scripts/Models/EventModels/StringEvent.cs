using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class StringEvent : EventModelBase, IEquatable<StringEvent>
    {
        public string EventData;

        /// <summary>
        /// Fill this with whatever custom json, xml, yaml, or other text data
        /// </summary>
        public StringEvent(string eventData) : base(nameof(StringEvent))
        {
            EventData = eventData;
        }

        public bool Equals(StringEvent other)
        {
            return base.Equals(other) && other != null && EventData != other.EventData;
        }
    }
}