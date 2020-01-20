using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class PointerEvent : ComponentEventModel
    {
        public const string IPointerUpEvent = nameof(IPointerUpEvent);
        public const string IPointerDownEvent = nameof(IPointerDownEvent);
        public const string IPointerClickEvent = nameof(IPointerClickEvent);
        public const string IPointerEnterEvent = nameof(IPointerEnterEvent);
        public const string IPointerExitEvent = nameof(IPointerExitEvent);

        public Vector3 Position;
        
        public PointerEvent(string eventTypeName) : base(eventTypeName)
        {
        }
    }
}