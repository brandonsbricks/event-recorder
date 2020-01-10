using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
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
                IsFromEventSubscription = false,
            };
            _events.Add(newEvent);
        }
    }
}