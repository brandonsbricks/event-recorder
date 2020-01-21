using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class InputRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(InputRecorder);
        
        private readonly List<StringEvent> _keypressEvents = new List<StringEvent>();
        private readonly List<MouseEvent> _mouseEvents = new List<MouseEvent>();
        private readonly List<KeyCode> _keyCodes;

        private enum KeyPlace
        {
            Up,
            Down
        }

        public InputRecorder()
        {
            _keyCodes = System.Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().Skip(1).ToList();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.StringEvents.AddRange(_keypressEvents);
            collection.MouseEvents.AddRange(_mouseEvents);
            
            _keypressEvents.Clear();
            _mouseEvents.Clear();

            return collection;
        }

        public void OnUpdate()
        {
            TryRecordEvents();
        }

        private void TryRecordEvents()
        {
            for (int i = 0; i < _keyCodes.Count; i++)
            {
                var keyCode = _keyCodes[i];
                if (Input.GetKeyDown(keyCode))
                {
                    CreateEvent(KeyPlace.Down, keyCode.ToString());
                }
                else if (Input.GetKeyUp(keyCode))
                {
                    CreateEvent(KeyPlace.Up, keyCode.ToString());
                }
            }
        }

        private void CreateEvent(KeyPlace place, string eventData)//todo: does this record touch inputs?u
        {
            if (eventData.Contains("Mouse"))
            {
                var eventType = place == KeyPlace.Down ? MouseEvent.MouseDownEvent : MouseEvent.MouseUpEvent;
                var newEvent = new MouseEvent(eventType, eventData, Input.mousePosition);
                _mouseEvents.Add(newEvent);
            }
            else
            {
                var eventType = place == KeyPlace.Down ? StringEvent.KeydownEvent : StringEvent.KeyupEvent;
                var newEvent = new StringEvent(eventType, eventData);
                _keypressEvents.Add(newEvent);
            }
        }
    }
}