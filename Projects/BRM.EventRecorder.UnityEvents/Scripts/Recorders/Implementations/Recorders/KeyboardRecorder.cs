using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class KeyboardRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(KeyboardRecorder);
        
        private readonly List<StringEvent> _keypressEvents = new List<StringEvent>();
        private readonly List<KeyCode> _keyCodes;

        private enum KeyPlace
        {
            Up,
            Down
        }

        public KeyboardRecorder()
        {
            _keyCodes = System.Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().Skip(1).Where(code => !code.ToString().ToLowerInvariant().Contains("mouse")).ToList();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.StringEvents.AddRange(_keypressEvents);
            _keypressEvents.Clear();

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
            var eventType = place == KeyPlace.Down ? StringEvent.KeydownEvent : StringEvent.KeyupEvent;
            var newEvent = new StringEvent(eventType, eventData);
            _keypressEvents.Add(newEvent);
        }
    }
}