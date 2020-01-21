using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class KeyboardInputReplayer : Replayer<StringEvent>
    {
        public override void Replay(StringEvent model)
        {
            //todo: cross platform win32 .dll/library stuff for simulating keypresses
        }
    }
}