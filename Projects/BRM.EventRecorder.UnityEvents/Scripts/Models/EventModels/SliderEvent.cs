using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class SliderEvent : PointerEvent
    {
        public string PropertyName;
        public float NewValue;
        
        public SliderEvent() : base(nameof(SliderEvent))
        {
            IsFromEventSubscription = true;
        }
    }
}