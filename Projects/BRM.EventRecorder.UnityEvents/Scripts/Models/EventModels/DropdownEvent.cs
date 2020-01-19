using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class DropdownEvent : PointerEvent
    {
        public const string UnityDropdownEvent = nameof(UnityDropdownEvent);

        public string PropertyName;
        public int NewIntValue;
        public string NewStringValue;
        
        public DropdownEvent(string eventTypeName) : base(eventTypeName)
        {
            IsFromEventSubscription = true;
        }
    }
}