using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class ToggleEvent : PointerEvent
    {
        public string PropertyName;
        public bool NewValue;
        
        public ToggleEvent() : base(nameof(ToggleEvent))
        {
            IsFromEventSubscription = true;
        }
    }
}