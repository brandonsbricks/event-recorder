using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class EventTriggerSubscriber : SelectableSubscriber<EventTrigger, ComponentTouchEvent>
    {
        public override string Name => nameof(EventTriggerSubscriber);
        
        private readonly List<UnityAction<BaseEventData>> _onClicks = new List<UnityAction<BaseEventData>>();
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
        
        
        protected override void OnSubscribe(EventTrigger eventTrigger)
        {
            AddEventTriggerListener(EventTriggerType.PointerClick, eventTrigger);
            AddEventTriggerListener(EventTriggerType.PointerDown, eventTrigger);
            AddEventTriggerListener(EventTriggerType.PointerUp, eventTrigger);
        }

        protected override void OnUnsubscribe(EventTrigger eventTrigger)
        {
            _onClicks.ForEach(onClick =>
            {
                RemoveEventTriggerListener(EventTriggerType.PointerClick, onClick, eventTrigger);
                RemoveEventTriggerListener(EventTriggerType.PointerDown, onClick, eventTrigger);
                RemoveEventTriggerListener(EventTriggerType.PointerUp, onClick, eventTrigger);
            });
        }

        
        private void AddEventTriggerListener(EventTriggerType type, EventTrigger eventTrigger)
        {
            if (ReferenceEquals(eventTrigger, null))
            {
                return;
            }
            UnityAction<BaseEventData> onClick = data =>
            {
                var newEvent = new ComponentTouchEvent(TriggerTypeToString(type)) {IsFromEventSubscription = true};
                PopulateCommonEventData(newEvent, eventTrigger.transform);
                _events.Add(newEvent);
            };
            _onClicks.Add(onClick);

            var clickTrigger = eventTrigger.triggers.Find(trigger => trigger.eventID == type);
            if (clickTrigger == null)
            {
                clickTrigger = new EventTrigger.Entry
                {
                    eventID = type,
                };
                eventTrigger.triggers.Add(clickTrigger);
            }
            clickTrigger.callback.AddListener(onClick);
        }
        
        private void RemoveEventTriggerListener(EventTriggerType type, UnityAction<BaseEventData> onClick, EventTrigger eventTrigger)
        {
            if (ReferenceEquals(eventTrigger, null))
            {
                return;
            }

            var clickTrigger = eventTrigger.triggers.Find(trigger => trigger.eventID == type);
            clickTrigger?.callback.RemoveListener(onClick);
        }

        public static string TriggerTypeToString(EventTriggerType type)
        {
            switch (type)
            {
                case EventTriggerType.PointerDown: return ComponentTouchEvent.EventTriggerDownEvent;
                case EventTriggerType.PointerUp: return ComponentTouchEvent.EventTriggerUpEvent;
                case EventTriggerType.PointerClick: return ComponentTouchEvent.EventTriggerClickEvent;
            }

            return ComponentTouchEvent.EventTriggerUnknownEvent;
        }
    }
}