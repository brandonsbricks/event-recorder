namespace BRM.InteractionRecorder.UnityUi.Interfaces
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
        void AddUniqueSubscriber(UiEventSubscriber subscriber);
        void AddUniqueCollector(UiEventCollector collector);
    }
}