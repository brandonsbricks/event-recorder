using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using BRM.InteractionRecorder.UnityUi.Subscribers;

namespace BRM.InteractionRecorder.UnityUi
{    
    public class EventService : IAddCollectors, IAddSubscribers
    {
        public readonly EventAndAppPayload Payload = new EventAndAppPayload();
        public int EventCount => Payload.EventCount;
        
        private readonly SubscriberCollection _allSubscribers = null;
        
        public EventService(List<UiEventSubscriber> subscribers, List<UiEventCollector> collectors)
        {
            _allSubscribers = new SubscriberCollection(subscribers, collectors);
        }

        public void AddUniqueSubscriber(UiEventSubscriber subscriber)
        {
            _allSubscribers.AddUniqueSubscriber(subscriber);
        }
        public void AddUniqueCollector(UiEventCollector collector)
        {
            _allSubscribers.AddUniqueCollector(collector);
        }
        public T GetSubscriber<T>() where T : UiEventSubscriber
        {
            return _allSubscribers.GetSubscriber<T>();
        }
        public T GetCollector<T>() where T : UiEventCollector
        {
            return _allSubscribers.GetCollector<T>();
        }

        public void ResetSubscriptions()
        {
            _allSubscribers.ResetSubscriptions();
        }

        public void UpdatePayload()
        {
            var newEvents = _allSubscribers.ExtractNewEvents();
            Payload.UpdateProperties();
            Payload.AppendNewEventsUniquely(newEvents);
        }

        public void SetServer(string server)
        {
            Payload.SetServer(server);
        }

        public void ToggleRecording(bool record)
        {
            if (!record)
            {
                _allSubscribers.UnsubscribeAll();
            }
            else
            {
                _allSubscribers.ResetSubscriptions();
            }
        }
    }
}