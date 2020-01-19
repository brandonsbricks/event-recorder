using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class SimpleTouchEvent : EventModelBase
    {
        public const string TouchDown = "TouchDownEvent";
        public const string TouchUp = "TouchUpEvent";
        
        [SerializeField] private Vector2Int TouchPointPixels;
        
        public Vector3 TouchPointProp
        {
            get => new Vector3(TouchPointPixels.x, TouchPointPixels.y);
            set => TouchPointPixels = new Vector2Int((int) value.x, (int) value.y);
        }
        
        public SimpleTouchEvent() : base(nameof(SimpleTouchEvent))
        {
        }
    }
}