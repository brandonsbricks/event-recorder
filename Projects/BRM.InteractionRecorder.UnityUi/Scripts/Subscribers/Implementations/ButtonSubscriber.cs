using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class ButtonSubscriber : TouchSubscriber<Button, ComponentTouchEvent>
    {
        public override string Name => nameof(ButtonSubscriber);
        
        private readonly List<UnityAction> _onClicks = new List<UnityAction>();
        private readonly List<ComponentTouchEvent> _events = new List<ComponentTouchEvent>();

        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onClicks.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.ComponentTouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }
        
        
        protected override void OnSubscribe(Button button)
        {
            UnityAction onClick = () =>
            {
                var newEvent = new ComponentTouchEvent(ComponentTouchEvent.ButtonEvent);
                PopulateCommonEventData(newEvent, button.transform);
                _events.Add(newEvent);
            };
            _onClicks.Add(onClick);
            button.onClick.AddListener(onClick);
        }

        protected override void OnUnsubscribe(Button button)
        {
            _onClicks.ForEach(button.onClick.RemoveListener);
        }
    }
}