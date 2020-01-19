using BRM.EventRecorder.UnityUi.Models;

namespace BRM.EventRecorder.UnityUi
{
    public interface ISubscribeToUiEvents
    {
        void ResetSubscriptions();
        void UnsubscribeAll();
    }

    public interface ICollectEvents
    {
        EventModelCollection ExtractNewEvents();
    }
    
    
    public abstract class UiEventCollector : ICollectEvents
    {
        public abstract EventModelCollection ExtractNewEvents();
        public abstract string Name { get; }
    }

    public abstract class UiEventSubscriber : UiEventCollector, ISubscribeToUiEvents
    {
        public abstract void ResetSubscriptions();
        public abstract void UnsubscribeAll();
    }
}