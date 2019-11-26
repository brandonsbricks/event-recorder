using System;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityUi.Models
{
    [Serializable]
    public struct Vector3S
    {
        public float x, y, z;
        
        public Vector3S(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3S(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}