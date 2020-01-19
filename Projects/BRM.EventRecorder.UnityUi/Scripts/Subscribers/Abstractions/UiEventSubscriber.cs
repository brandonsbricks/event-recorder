using BRM.EventRecorder.UnityUi.Models;

namespace BRM.EventRecorder.UnityUi
{
    public interface ISubscribeToEvents
    {
        void ResetSubscriptions();
        void UnsubscribeAll();
    }

    public interface ICollectEvents
    {
        EventModelCollection ExtractNewEvents();
    }
    
    
    public abstract class EventCollector : ICollectEvents
    {
        public abstract EventModelCollection ExtractNewEvents();
        public abstract string Name { get; }
    }

    public abstract class EventSubscriber : EventCollector, ISubscribeToEvents
    {
        public abstract void ResetSubscriptions();
        public abstract void UnsubscribeAll();
    }
}