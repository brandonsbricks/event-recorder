using System.Collections.Generic;
using BRM.EventRecorder.UnityUi.Interfaces;
using BRM.EventRecorder.UnityUi.Models;
using BRM.EventRecorder.UnityUi.Subscribers;

namespace BRM.EventRecorder.UnityUi
{    
    public class EventService : ISubscriberCollection
    {
        public readonly EventAndAppPayload Payload = new EventAndAppPayload();
        public int EventCount => Payload.EventCount;
        
        protected readonly SubscriberCollection _allSubscribers = null;
        
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
        public List<IUpdate> GetUpdaters()
        {
            return _allSubscribers.GetUpdaters();
        }

        public void ResetSubscriptions()
        {
            _allSubscribers.ResetSubscriptions();
        }

        public void ClearRecording()
        {
            Payload.ClearRecording();
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