using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class KeypressRecorder : EventRecorder, IGui
    {
        public override string Name => nameof(KeypressRecorder);
        private readonly List<StringEvent> _events = new List<StringEvent>();
        
        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.StringEvents.AddRange(_events);
            _events.Clear();

            return collection;
        }

        public void OnGui()//doesn't work for any old keypress. Only for IMGUI keypresses...
        {
            if (Event.current.type==EventType.KeyDown)
            {
                var keycode = Event.current.keyCode;
                var newEvent = new StringEvent(StringEvent.KeypressEvent, keycode.ToString());
                _events.Add(newEvent);
            }
        }
    }
}