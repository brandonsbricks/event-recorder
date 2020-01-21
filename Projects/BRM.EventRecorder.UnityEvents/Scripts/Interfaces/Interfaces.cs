using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventRecorder.UnityEvents.Interfaces
{
    public interface IUpdate
    {
        void OnUpdate();
    }

    public interface ISubscriberCollection
    {
        void AddUniqueSubscriber(EventSubscriber subscriber);
        void AddUniqueRecorder(EventRecorder recorder);
    }
    
    public interface ISubscribeToEvents
    {
        void ResetSubscriptions();
        void UnsubscribeAll();
    }

    public interface IRecordEvents
    {
        EventModelCollection ExtractNewEvents();
    }
    
    public interface IEventFactory
    {
        RecordingService Create();
    }
}