using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class MouseInputReplayer : Replayer<MouseEvent>
    {
        public override void Replay(MouseEvent model)
        {
            //todo: display touch ui
        }
    }
}