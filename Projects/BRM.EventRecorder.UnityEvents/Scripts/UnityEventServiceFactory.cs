using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Subscribers;

namespace BRM.EventRecorder.UnityEvents
{
    //todo: acceptable ocp violation. modify when recorder is added 
    public class UnityEventServiceFactory : IEventFactory
    {
        public virtual EventService Create()
        {
            var service = new EventService(
                new List<EventSubscriber>
                {
                    //generic Unity UI subscribers
                    new SceneChangedSubscriber(),
                    new SliderSubscriber(),
                    new DropdownSubscriber(),
                    new TextInputSubscriber(),
                    new ToggleSubscriber()
                },
                new List<EventRecorder>
                {
                    //generic touch events
                    new SimpleTouchRecorder(),
                    new PointerHandlerRecorder(),
                    new TransformRecorder(),
                    new CustomEventRecorder(),
                });
            return service;
        }
    }
}