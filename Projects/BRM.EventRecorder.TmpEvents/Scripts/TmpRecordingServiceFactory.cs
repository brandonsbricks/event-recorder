using BRM.EventRecorder.TmpEvents.Subscribers;
using BRM.EventRecorder.UnityEvents;

namespace BRM.EventRecorder.TmpEvents
{
    public class TmpRecordingServiceFactory : UnityRecordingServiceFactory
    {
        public override RecordingService Create()
        {
            var service = base.Create();
            service.AddUniqueSubscriber(new TmpDropdownSubscriber());
            service.AddUniqueSubscriber(new TmpTextInputSubscriber());
            return service;
        }
    }
}