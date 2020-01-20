using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class TextInputEvent : ComponentEventModel
    {
        public const string UnityTextInputEvent = nameof(UnityTextInputEvent);
        
        public string NewValue;
        
        public TextInputEvent(string eventTypeName) : base(eventTypeName)
        {
            IsFromEventSubscription = true;
        }
    }
}