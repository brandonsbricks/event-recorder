using BRM.EventRecorder.UnityUi;

namespace BRM.EventRecorder.TmpUi
{
    public class TmpEventGatherer : UnityEventGatherer
    {
        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new TmpEventServiceFactory().Create());
    }
}