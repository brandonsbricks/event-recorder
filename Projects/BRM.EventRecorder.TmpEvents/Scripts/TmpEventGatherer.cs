using BRM.EventRecorder.UnityEvents;

namespace BRM.EventRecorder.TmpEvents
{
    public class TmpEventGatherer : UnityEventGatherer
    {
        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new TmpEventServiceFactory().Create());
    }
}