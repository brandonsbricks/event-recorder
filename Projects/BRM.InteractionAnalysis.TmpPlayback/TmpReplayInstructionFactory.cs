using System;
using System.Collections.Generic;
using BRM.InteractionAnalysis.UnityPlayback;
using BRM.InteractionRecorder.UnityUi.Models;

namespace BRM.InteractionAnalysis.TmpPlayback
{
    public class TmpReplayInstructionFactory : ReplayInstructionFactory
    {
        public override Dictionary<string, Action<EventModelBase>> GetInstructions()
        {
            var replayers = base.GetInstructions();
            var tmpDropdownReplayer = new TmpDropdownReplayer();
            var tmpTextInputReplayer = new TmpTextInputReplayer();
            replayers.Add(DropdownEvent.TmpDropdownEvent, model => tmpDropdownReplayer.Replay(model as DropdownEvent));
            replayers.Add(TextInputEvent.TmpTextInputEvent, model => tmpTextInputReplayer.Replay(model as TextInputEvent));
            return replayers;
        }
    }
}