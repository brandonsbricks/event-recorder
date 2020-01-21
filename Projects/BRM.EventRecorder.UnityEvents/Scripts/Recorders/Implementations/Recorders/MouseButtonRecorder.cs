using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class MouseButtonRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(MouseButtonRecorder);
        
        private readonly List<MouseEvent> _mouseEvents = new List<MouseEvent>();
        private readonly List<KeyCode> _keyCodes;

        private enum KeyPlace
        {
            Up,
            Down
        }

        public MouseButtonRecorder()
        {
            _keyCodes = System.Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().Where(code => code.ToString().ToLowerInvariant().Contains("mouse")).ToList();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.MouseEvents.AddRange(_mouseEvents);
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

        private void CreateEvent(KeyPlace place, string eventData)//todo: does this record touch inputs?
        {
            var eventType = place == KeyPlace.Down ? MouseEvent.MouseDownEvent : MouseEvent.MouseUpEvent;
            var newEvent = new MouseEvent(eventType, eventData, Input.mousePosition);
            _mouseEvents.Add(newEvent);
        }
    }
}