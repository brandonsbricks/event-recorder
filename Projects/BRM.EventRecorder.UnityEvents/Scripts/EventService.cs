using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using BRM.EventRecorder.UnityEvents.Recorders;

namespace BRM.EventRecorder.UnityEvents
{    
    
    public class RecordingService : ISubscriberCollection
    {
        public readonly EventAndAppPayload Payload = new EventAndAppPayload();
        public int EventCount => Payload.EventCount;
        
        private readonly EventRecorderCollection _allEventRecorders = null;
        
        public RecordingService(List<EventSubscriber> subscribers, List<EventRecorder> recorders)
        {
            _allEventRecorders = new EventRecorderCollection(subscribers, recorders);
        }

        public void AddUniqueSubscriber(EventSubscriber subscriber)
        {
            _allEventRecorders.AddUniqueSubscriber(subscriber);
        }
        public void AddUniqueRecorder(EventRecorder recorder)
        {
            _allEventRecorders.AddUniqueRecorder(recorder);
        }
        public List<IUpdate> GetUpdaters()
        {
            return _allEventRecorders.GetUpdaters();
        }
        public List<IGui> GetGuiers()
        {
            return _allEventRecorders.GetGuiers();
        }

        public void ResetSubscriptions()
        {
            _allEventRecorders.ResetSubscriptions();
        }

        public void ClearRecording()
        {
            Payload.ClearRecording();
        }

        public void UpdatePayload()
        {
            var newEvents = _allEventRecorders.ExtractNewEvents();
            Payload.UpdateProperties();
            Payload.AppendNewEventsUniquely(newEvents);
        }

        public void SetAppValues(string gitSha, string server)
        {
            Payload.SetAppValues(gitSha, server);
        }

        public void ToggleRecording(bool record)
        {
            if (!record)
            {
                _allEventRecorders.UnsubscribeAll();
            }
            else
            {
                _allEventRecorders.ResetSubscriptions();
            }
        }
    }
}