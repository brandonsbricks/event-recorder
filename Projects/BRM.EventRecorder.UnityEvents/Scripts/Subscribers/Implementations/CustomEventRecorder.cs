using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventRecorder.UnityEvents.Subscribers
{
    public class CustomEventRecorder : EventRecorder
    {
        public override string Name => nameof(CustomEventRecorder);
        private List<StringEvent> _events = new List<StringEvent>();

        public void AddEvent(string eventData)
        {
            var newEvent = new StringEvent(eventData);
            _events.Add(newEvent);
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.CustomEvents.AddRange(_events);
            _events.Clear();
            
            return collection;
        }
    }
}