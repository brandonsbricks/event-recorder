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
        
        [SerializeField] private Vector2Int TouchPointPixels;
        
        public Vector3 TouchPoint
        {
            get => new Vector3(TouchPointPixels.x, TouchPointPixels.y);
            set => TouchPointPixels = new Vector2Int((int) value.x, (int) value.y);
        }
        
        public PointerEvent(string eventTypeName) : base(eventTypeName)
        {
        }
    }
}