using System.Collections.Generic;
using BRM.EventRecorder.UnityUi.Subscribers;

namespace BRM.EventRecorder.UnityUi
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
                    new SliderSubscriber(),
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