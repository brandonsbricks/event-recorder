using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class ToggleEvent : PointerEvent
    {
        public const string Name = nameof(ToggleEvent);
        
        public bool NewValue;
        
        public ToggleEvent() : base(Name)
        {
            IsFromEventSubscription = true;
        }
    }
}