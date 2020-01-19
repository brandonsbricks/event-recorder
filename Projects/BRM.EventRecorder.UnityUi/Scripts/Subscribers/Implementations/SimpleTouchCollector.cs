using System.Collections.Generic;
using BRM.EventRecorder.UnityUi.Interfaces;
using BRM.EventRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityUi.Subscribers
{
    public class SimpleTouchCollector : UiEventCollector, IUpdate
    {
        public override string Name => nameof(SimpleTouchCollector);
        private readonly List<SimpleTouchEvent> _events = new List<SimpleTouchEvent>();
        
        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                CreateEvent(SimpleTouchEvent.TouchDown);
            }
            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                CreateEvent(SimpleTouchEvent.TouchUp);
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
            var newEvent = new SimpleTouchEvent
            {
                TouchPointProp = Input.mousePosition,
                EventType = eventType,
            };
            _events.Add(newEvent);
        }
    }
}