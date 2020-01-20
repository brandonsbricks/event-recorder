using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class SliderEvent : PointerEvent
    {
        public const string Name = nameof(SliderEvent);
        
        public float NewValue;
        
        public SliderEvent() : base(Name)
        {
            IsFromEventSubscription = true;
        }
    }
}