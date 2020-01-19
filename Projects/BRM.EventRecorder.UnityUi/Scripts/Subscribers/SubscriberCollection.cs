using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityUi.Interfaces;
using BRM.EventRecorder.UnityUi.Models;

namespace BRM.EventRecorder.UnityUi.Subscribers
{
    public class SubscriberCollection : EventSubscriber, ISubscriberCollection
    {
        public override string Name => nameof(SubscriberCollection);
        private readonly List<EventSubscriber> _subscribers = new List<EventSubscriber>();
        private readonly List<EventCollector> _collectors = new List<EventCollector>();

        public SubscriberCollection(List<EventSubscriber> subscribers, List<EventCollector> collectors)
        {
            if (subscribers != null)
            {
                _subscribers = subscribers;
            }

            if (collectors != null)
            {
                _collectors = collectors;
            }

            for (int i = 0; i < _subscribers.Count; i++)
            {
                AddUniqueCollector(_subscribers[i]);
            }

            _subscribers.RemoveAll(sub => sub == null);
            _collectors.RemoveAll(col => col == null); 
        }

        public void AddUniqueSubscriber(EventSubscriber subscriber)
        {
            var subscriberName = subscriber.Name;
            var existingSubscriber = _subscribers.Find(sub => sub.Name == subscriberName);
            if (existingSubscriber == null)
            {
                _subscribers.Add(subscriber);
            }
            AddUniqueCollector(subscriber);
        }
        public void AddUniqueCollector(EventCollector collector)
        {
            var collectorName = collector.Name;
            var existingCollector = _collectors.Find(col => col.Name == collectorName);
            if (existingCollector == null)
            {
                _collectors.Add(collector);
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
            _collectors.ForEach(col =>
            {
                var events = col.ExtractNewEvents();
                eventCollectionList.Add(events);
            });

            collection.SceneChangedEvents = eventCollectionList.SelectMany(col => col.SceneChangedEvents).ToList();
            collection.SimpleTouchEvents = eventCollectionList.SelectMany(col => col.SimpleTouchEvents).ToList();
            collection.IPointerEvents = eventCollectionList.SelectMany(col => col.IPointerEvents).ToList();
            collection.ToggleEvents = eventCollectionList.SelectMany(col => col.ToggleEvents).ToList();
            collection.DropdownEvents = eventCollectionList.SelectMany(col => col.DropdownEvents).ToList();
            collection.TextInputEvents = eventCollectionList.SelectMany(col => col.TextInputEvents).ToList();

            return collection;
        }
        
        public List<IUpdate> GetUpdaters()
        {
            return _collectors.Where(sub => sub is IUpdate).Cast<IUpdate>().ToList();
        }
    }
}