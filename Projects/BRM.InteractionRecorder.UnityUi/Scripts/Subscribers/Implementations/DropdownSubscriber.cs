using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class DropdownSubscriber : TouchSubscriber<Dropdown, DropdownEvent>
    {
        public override string Name => nameof(DropdownSubscriber);
        
        private readonly List<DropdownEvent> _dropdownChangedEvents = new List<DropdownEvent>();        
        private readonly List<UnityAction<int>> _onDropdownValueChanged = new List<UnityAction<int>>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onDropdownValueChanged.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.DropdownEvents.AddRange(_dropdownChangedEvents);
            _dropdownChangedEvents.Clear();
            return collection;
        }
        
        
        protected override void OnSubscribe(Dropdown dropdown)
        {
            UnityAction<int> onDropdownChanged = (newValue) =>
            {
                var newEvent = new DropdownEvent
                {
                    PropertyName = "value",
                    NewIntValue = newValue,
                    NewStringValue = dropdown.options[newValue].text,
                };
                PopulateCommonEventData(newEvent, dropdown.transform);
                _dropdownChangedEvents.Add(newEvent);
            };
            _onDropdownValueChanged.Add(onDropdownChanged);
            dropdown.onValueChanged.AddListener(onDropdownChanged);
        }

        protected override void OnUnsubscribe(Dropdown dropdown)
        {
            _onDropdownValueChanged.ForEach(dropdown.onValueChanged.RemoveListener);
        }
    }
}