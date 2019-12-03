using BRM.InteractionRecorder.UnityUi.Models;
using BRM.InteractionRecorder.UnityUi.Subscribers;

using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

namespace BRM.InteractionRecorder.TmpUi.Subscribers
{
    public class TmpTextInputSubscriber : SelectableSubscriber<TMP_InputField, TextInputEvent>
    {
        public override string Name => nameof(TmpTextInputSubscriber);
        
        private readonly List<TextInputEvent> _events = new List<TextInputEvent>();
        private readonly List<UnityAction<string>> _onTmpInputFieldEndEdits = new List<UnityAction<string>>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onTmpInputFieldEndEdits.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.TextInputEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }
        
        
        protected override void OnSubscribe(TMP_InputField tmpInput)
        {
            UnityAction<string> onInputFieldEndEdit = newValue =>
            {
                var newEvent = new TextInputEvent
                {
                    PropertyName = "text",
                    NewValue = newValue,
                    EventType = TextInputEvent.TmpTextInputEvent
                };
                PopulateCommonEventData(newEvent, tmpInput.transform);
                _events.Add(newEvent);
            };
            _onTmpInputFieldEndEdits.Add(onInputFieldEndEdit);
            tmpInput.onEndEdit.AddListener(onInputFieldEndEdit);
        }

        protected override void OnUnsubscribe(TMP_InputField tmpInput)
        {
            _onTmpInputFieldEndEdits.ForEach(tmpInput.onEndEdit.RemoveListener);
        }
    }
}