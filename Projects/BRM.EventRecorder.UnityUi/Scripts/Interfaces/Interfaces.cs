namespace BRM.EventRecorder.UnityUi.Interfaces
{
    public interface IUpdate
    {
        void OnUpdate();
    }

    public interface IHoldEventData
    {
        string GetJson();
    }
    
    public interface ISubscriberCollection
    {
        void AddUniqueSubscriber(EventSubscriber subscriber);
        void AddUniqueCollector(EventCollector collector);
    }
}