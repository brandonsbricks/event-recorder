using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Subscribers
{
    public class SimpleTouchRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(SimpleTouchRecorder);
        private readonly List<PositionEvent> _events = new List<PositionEvent>();
        
        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                CreateEvent(PositionEvent.TouchDown);
            }
            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                CreateEvent(PositionEvent.TouchUp);
            }
        }
        
        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.SimpleTouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }
        
        private void CreateEvent(string eventType)
        {
            var newEvent = new PositionEvent(eventType)
            {
                Position = Input.mousePosition,
            };
            _events.Add(newEvent);
        }
    }
}