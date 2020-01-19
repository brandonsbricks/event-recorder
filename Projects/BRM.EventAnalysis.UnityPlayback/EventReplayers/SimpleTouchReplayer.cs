using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class SimpleTouchReplayer : Replayer<SimpleTouchEvent>
    {
        public override void Replay(SimpleTouchEvent model)
        {
            //todo: display touch ui
        }
    }
}