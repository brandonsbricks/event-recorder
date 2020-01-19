using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventRecorder.UnityEvents
{
    public abstract class EventSubscriber : EventRecorder, ISubscribeToEvents
    {
        public abstract void ResetSubscriptions();
        public abstract void UnsubscribeAll();
    }
    
    public abstract class EventRecorder : IRecordEvents
    {
        public abstract EventModelCollection ExtractNewEvents();
        public abstract string Name { get; }
    }
}