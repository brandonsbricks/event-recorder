using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class SceneChangedReplayer : Replayer<SceneChangedEvent>
    {
        public override void Replay(SceneChangedEvent model)
        {
            //todo: display scene change ui
        }
    }
}