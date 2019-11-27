using BRM.InteractionRecorder.TmpUi;
using BRM.InteractionRecorder.UnityUi;

namespace BRM.InteractionRecorder.Ui.TMP
{
    public class TmpInteractionGatherer : UnityInteractionGatherer
    {
        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new TmpEventServiceFactory().Create());
    }
}