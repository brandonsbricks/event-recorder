using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Recorders;

namespace BRM.EventRecorder.UnityEvents
{
    //todo: acceptable ocp violation. modify when recorder is added 
    public class UnityRecordingServiceFactory : IEventFactory
    {
        public virtual RecordingService Create()
        {
            var service = new RecordingService(
                new List<EventSubscriber>
                {
                    new SceneChangedSubscriber(),
                    new SliderSubscriber(),
                    new DropdownSubscriber(),
                    new TextInputSubscriber(),
                    new ToggleSubscriber()
                },
                new List<EventRecorder>
                {
                    new SimpleTouchRecorder(),
                    new PointerHandlerRecorder(),
                    new TransformRecorder(),
                    new KeypressRecorder(),
                    new CustomEventRecorder(),
                });
            return service;
        }
    }
}