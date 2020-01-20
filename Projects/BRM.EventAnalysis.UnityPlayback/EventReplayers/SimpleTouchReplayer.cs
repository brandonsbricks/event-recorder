using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class SimpleTouchReplayer : Replayer<PositionEvent>
    {
        public override void Replay(PositionEvent model)
        {
            //todo: display touch ui
        }
    }
}