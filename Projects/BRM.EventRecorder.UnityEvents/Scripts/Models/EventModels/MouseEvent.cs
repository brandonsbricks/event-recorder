using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class MouseEvent : StringEvent
    {
        public const string MouseDownEvent = nameof(MouseDownEvent);
        public const string MouseUpEvent = nameof(MouseUpEvent);
        
        public Vector3 Position;
        
        public MouseEvent(string eventTypeName, string eventData, Vector3 position) : base(eventTypeName, eventData)
        {
            Position = position;
        }
    }
}