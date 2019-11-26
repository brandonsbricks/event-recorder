using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class TextInputSubscriber : SelectableSubscriber<InputField, TextInputEvent>
    {
        public override string Name => nameof(TextInputSubscriber);
        
        private readonly List<TextInputEvent> _events = new List<TextInputEvent>();
        private readonly List<UnityAction<string>> _onInputFieldEndEdits = new List<UnityAction<string>>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onInputFieldEndEdits.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.TextInputEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }
        
        protected override void OnSubscribe(InputField inputField)
        {
            UnityAction<string> onInputFieldEndEdit = (newValue) =>
            {
                var newEvent = new TextInputEvent
                {
                    PropertyName = "text",
                    NewValue = newValue
                };
                PopulateCommonEventData(newEvent, inputField.transform);
                _events.Add(newEvent);
            };
            _onInputFieldEndEdits.Add(onInputFieldEndEdit);
            inputField.onEndEdit.AddListener(onInputFieldEndEdit);
        }

        protected override void OnUnsubscribe(InputField inputField)
        {
            _onInputFieldEndEdits.ForEach(inputField.onEndEdit.RemoveListener);
        }
    }
}