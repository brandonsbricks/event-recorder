using BRM.EventRecorder.TmpEvents.Subscribers;
using BRM.EventRecorder.UnityEvents;

namespace BRM.EventRecorder.TmpEvents
{
    public class TmpEventServiceFactory : UnityEventServiceFactory
    {
        public override EventService Create()
        {
            var service = base.Create();
            service.AddUniqueSubscriber(new TmpDropdownSubscriber());
            service.AddUniqueSubscriber(new TmpTextInputSubscriber());
            return service;
        }
    }
}