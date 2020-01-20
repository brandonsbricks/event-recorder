using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class PositionEvent : EventModelBase
    {
        public const string TouchDown = "TouchDownEvent";
        public const string TouchUp = "TouchUpEvent";

        public Vector3 Position;
        
        public PositionEvent(string eventTypeName) : base(eventTypeName)
        {
        }
    }
}