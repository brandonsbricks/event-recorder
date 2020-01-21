using System;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents
{
    internal sealed class UpdateBroadcaster : MonoBehaviour
    {
        private static UpdateBroadcaster _instance;

        public static event Action OnUpdate
        {
            add
            {
                if (_instance == null)
                {
                    var go = new GameObject(typeof(UpdateBroadcaster).FullName);
                    _instance = go.AddComponent<UpdateBroadcaster>();
                }

                _onUpdate += value;
            }
            remove
            {
                _onUpdate -= value;
            }
        }
        private static event Action _onUpdate;

        
        private void Update()
        {
            _onUpdate?.Invoke();
        }
    }
}