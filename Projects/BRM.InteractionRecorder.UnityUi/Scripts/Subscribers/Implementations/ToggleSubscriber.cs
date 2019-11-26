using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class ToggleSubscriber : TouchSubscriber<Toggle, ToggleEvent>
    {
        public override string Name => nameof(ToggleSubscriber);
        private readonly List<ToggleEvent> _events = new List<ToggleEvent>();
        private readonly List<UnityAction<bool>> _onToggleChanges = new List<UnityAction<bool>>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onToggleChanges.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.ToggleEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }
        
        
        protected override void OnSubscribe(Toggle toggle)
        {
            UnityAction<bool> onToggleChange = (newValue) =>
            {
                var newEvent = new ToggleEvent
                {
                    PropertyName = "isOn",
                    NewValue = newValue,
                };
                PopulateCommonEventData(newEvent, toggle.transform);
                _events.Add(newEvent);
            };
            _onToggleChanges.Add(onToggleChange);
            toggle.onValueChanged.AddListener(onToggleChange);
        }

        protected override void OnUnsubscribe(Toggle toggle)
        {
            _onToggleChanges.ForEach(toggle.onValueChanged.RemoveListener);
        }
    }
}