using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Subscribers;

namespace BRM.InteractionRecorder.UnityUi
{
    public class UnityEventServiceFactory : IEventFactory
    {
        public virtual EventService Create()
        {
            var service = new EventService(
                new List<UiEventSubscriber>
                {
                    //generic Unity UI subscribers
                    new SceneChangedSubscriber(),
                    new EventTriggerSubscriber(),
                    new ButtonSubscriber(),
                    new DropdownSubscriber(),
                    new TextInputSubscriber(),
                    new ToggleSubscriber()
                },
                new List<UiEventCollector>
                {
                    //generic touch events
                    new SimpleTouchCollector(),
                    new PointerHandlerCollector()
                    
                });
            return service;
        }
    }

    public interface IEventFactory
    {
        EventService Create();
    }
}