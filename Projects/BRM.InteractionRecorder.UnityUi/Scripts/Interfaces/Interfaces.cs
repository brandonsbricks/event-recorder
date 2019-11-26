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
    
    public interface IAddSubscribers
    {
        void AddUniqueSubscriber(UiEventSubscriber subscriber);
        T GetSubscriber<T>() where T : UiEventSubscriber;        
    }

    public interface IAddCollectors
    {
        T GetCollector<T>() where T : UiEventCollector;
        void AddUniqueCollector(UiEventCollector collector);
    }
}