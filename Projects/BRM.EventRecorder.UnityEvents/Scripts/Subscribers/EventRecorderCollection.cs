using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventRecorder.UnityEvents.Subscribers
{
    public class EventRecorderCollection : EventSubscriber, ISubscriberCollection
    {
        public override string Name => nameof(EventRecorderCollection);
        
        private readonly List<EventSubscriber> _subscribers = new List<EventSubscriber>();
        private readonly List<EventRecorder> _recorders = new List<EventRecorder>();

        public EventRecorderCollection(List<EventSubscriber> subscribers, List<EventRecorder> recorders)
        {
            if (subscribers != null)
            {
                _subscribers = subscribers;
            }

            if (recorders != null)
            {
                _recorders = recorders;
            }

            for (int i = 0; i < _subscribers.Count; i++)
            {
                AddUniqueRecorder(_subscribers[i]);
            }

            _subscribers.RemoveAll(sub => sub == null);
            _recorders.RemoveAll(col => col == null); 
        }

        public void AddUniqueSubscriber(EventSubscriber subscriber)
        {
            var subscriberName = subscriber.Name;
            var existingSubscriber = _subscribers.Find(sub => sub.Name == subscriberName);
            if (existingSubscriber == null)
            {
                _subscribers.Add(subscriber);
            }
            AddUniqueRecorder(subscriber);
        }
        public void AddUniqueRecorder(EventRecorder recorder)
        {
            var recorderName = recorder.Name;
            var existingRecorder = _recorders.Find(col => col.Name == recorderName);
            if (existingRecorder == null)
            {
                _recorders.Add(recorder);
            }
        }

        public override void ResetSubscriptions()
        {
            _subscribers.ForEach(sub => sub.ResetSubscriptions());
        }

        public override void UnsubscribeAll()
        {
            _subscribers.ForEach(sub => sub.UnsubscribeAll());
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();

            var eventCollectionList = new List<EventModelCollection>();
            _recorders.ForEach(col =>
            {
                var events = col.ExtractNewEvents();
                eventCollectionList.Add(events);
            });

            //todo: acceptable ocp violation. modify when event data type is added
            collection.SceneChangedEvents = eventCollectionList.SelectMany(col => col.SceneChangedEvents).ToList();
            collection.SimpleTouchEvents = eventCollectionList.SelectMany(col => col.SimpleTouchEvents).ToList();
            collection.TransformEvents = eventCollectionList.SelectMany(col => col.TransformEvents).ToList();
            collection.PointerEvents = eventCollectionList.SelectMany(col => col.PointerEvents).ToList();
            collection.ToggleEvents = eventCollectionList.SelectMany(col => col.ToggleEvents).ToList();
            collection.SliderEvents = eventCollectionList.SelectMany(col => col.SliderEvents).ToList();
            collection.DropdownEvents = eventCollectionList.SelectMany(col => col.DropdownEvents).ToList();
            collection.TextInputEvents = eventCollectionList.SelectMany(col => col.TextInputEvents).ToList();
            collection.CustomEvents = eventCollectionList.SelectMany(col => col.CustomEvents).ToList();

            return collection;
        }
        
        public List<IUpdate> GetUpdaters()
        {
            return _recorders.Where(sub => sub is IUpdate).Cast<IUpdate>().ToList();
        }
    }
}