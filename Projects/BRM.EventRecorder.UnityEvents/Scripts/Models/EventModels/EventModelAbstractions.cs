using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public abstract class EventModelBase : IEquatable<EventModelBase>
    {
        public string EventType;
        public long TimestampMillis;
        public bool IsFromEventSubscription;

        protected EventModelBase(string eventTypeName)
        {
            EventType = eventTypeName;
            TimestampMillis = UnixTime;
        }

        public bool Equals(EventModelBase other)
        {
            if (other == null) return false;

            return
                TimestampMillis == other.TimestampMillis &&
                EventType == other.EventType;
        }

        protected static long UnixTime => (long) DateTime.UtcNow.Subtract(UnixStartTime).TotalMilliseconds;
        private static DateTime UnixStartTime => new DateTime(1970, 1, 1); 
        public string GetDisplayTime()
        {
            var dateTime = UnixStartTime.AddMilliseconds(TimestampMillis).ToUniversalTime();
            return $"{dateTime.ToShortDateString()}, {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{dateTime.Millisecond}";
        }
    }

    [Serializable]
    public abstract class ComponentEventModel : EventModelBase, IEquatable<ComponentEventModel>
    {
        public string ComponentType;
        public string GameObjectName;

        protected ComponentEventModel(string eventTypeName) : base(eventTypeName)
        {
        }

        public bool Equals(ComponentEventModel other)
        {
            if (other == null) return false;

            return Equals(other as EventModelBase) &&
                   ComponentType == other.ComponentType &&
                   GameObjectName == other.GameObjectName;
        }
    }
}