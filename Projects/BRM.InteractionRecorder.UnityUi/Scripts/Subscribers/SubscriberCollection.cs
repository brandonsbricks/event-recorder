using System.Collections.Generic;
using System.Linq;
using BRM.InteractionRecorder.UnityUi.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class SubscriberCollection : UiEventSubscriber, IAddCollectors, IAddSubscribers
    {
        public override string Name => nameof(SubscriberCollection);
        private readonly List<UiEventSubscriber> _subscribers = new List<UiEventSubscriber>();
        private readonly List<UiEventCollector> _collectors = new List<UiEventCollector>();

        public SubscriberCollection(List<UiEventSubscriber> subscribers, List<UiEventCollector> collectors)
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

        public void AddUniqueSubscriber(UiEventSubscriber subscriber)
        {
            var subscriberName = subscriber.Name;
            var existingSubscriber = _subscribers.Find(sub => sub.Name == subscriberName);
            if (existingSubscriber == null)
            {
                _subscribers.Add(subscriber);
            }
            AddUniqueCollector(subscriber);
        }
        public void AddUniqueCollector(UiEventCollector collector)
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

            collection.TouchEvents = eventCollectionList.SelectMany(col => col.TouchEvents).ToList();
            collection.ToggleEvents = eventCollectionList.SelectMany(col => col.ToggleEvents).ToList();
            collection.DropdownEvents = eventCollectionList.SelectMany(col => col.DropdownEvents).ToList();
            collection.TextInputEvents = eventCollectionList.SelectMany(col => col.TextInputEvents).ToList();

            return collection;
        }

        public T GetSubscriber<T>() where T : UiEventSubscriber
        {
            return _subscribers.Find(sub => sub.Name == nameof(T)) as T;
        }
        public T GetCollector<T>() where T : UiEventCollector
        {
            return _collectors.Find(sub => sub.Name == nameof(T)) as T;
        }
    }
}