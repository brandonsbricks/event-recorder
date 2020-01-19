using System.Collections.Generic;
using BRM.EventRecorder.UnityUi.Models;
using BRM.EventRecorder.UnityUi.Subscribers;
using TMPro;
using UnityEngine.Events;

namespace BRM.EventRecorder.TmpUi.Subscribers
{
    public class TmpDropdownSubscriber : TouchSubscriber<TMP_Dropdown, DropdownEvent>
    {
        public override string Name => nameof(TmpDropdownSubscriber);
        
        private readonly List<UnityAction<int>> _onTmpDropdownValueChanged = new List<UnityAction<int>>();
        private readonly List<DropdownEvent> _dropdownChangedEvents = new List<DropdownEvent>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onTmpDropdownValueChanged.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.DropdownEvents.AddRange(_dropdownChangedEvents);
            _dropdownChangedEvents.Clear();
            return collection;
        }
        
        
        protected override void OnSubscribe(TMP_Dropdown tmpDropdown)
        {
            UnityAction<int> onDropdownChanged = (newValue) =>
            {
                var newEvent = new DropdownEvent(TmpEventNames.TmpDropdownEvent)
                {
                    PropertyName = "value",
                    NewIntValue = newValue,
                    NewStringValue = tmpDropdown.options[newValue].text,
                };
                PopulateCommonEventData(newEvent, tmpDropdown.transform);
                _dropdownChangedEvents.Add(newEvent);
            };
            _onTmpDropdownValueChanged.Add(onDropdownChanged);
            tmpDropdown.onValueChanged.AddListener(onDropdownChanged);
        }

        protected override void OnUnsubscribe(TMP_Dropdown dropdown)
        {
            _onTmpDropdownValueChanged.ForEach(dropdown.onValueChanged.RemoveListener);
        }
    }
}