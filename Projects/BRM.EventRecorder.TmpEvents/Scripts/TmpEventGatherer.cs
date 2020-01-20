using BRM.EventRecorder.UnityEvents;

namespace BRM.EventRecorder.TmpEvents
{
    public class TmpEventGatherer : UnityEventGatherer
    {
        protected override RecordingService _recordingService => _recordingServiceLocal ?? (_recordingServiceLocal = new TmpRecordingServiceFactory().Create());
    }
}