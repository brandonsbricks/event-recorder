using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class TransformEvent : ComponentEventModel
    {
        public const string Name = nameof(TransformEvent);
        
        public Vector3 Position;
        public Quaternion Rotation;

        public TransformEvent() : base(nameof(TransformEvent))
        {
        }
    }
}