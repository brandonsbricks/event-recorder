using System;
using System.Collections.Generic;
using BRM.EventAnalysis.UnityPlayback;
using BRM.EventRecorder.TmpEvents;
using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.TmpPlayback
{
    public class TmpReplayInstructionFactory : ReplayInstructionFactory
    {
        public override Dictionary<string, Action<EventModelBase>> GetInstructions()
        {
            var replayers = base.GetInstructions();
            var tmpDropdownReplayer = new TmpDropdownReplayer();
            var tmpTextInputReplayer = new TmpTextInputReplayer();
            replayers.Add(TmpEventNames.TmpDropdownEvent, model => tmpDropdownReplayer.Replay(model as DropdownEvent));
            replayers.Add(TmpEventNames.TmpTextInputEvent, model => tmpTextInputReplayer.Replay(model as TextInputEvent));
            return replayers;
        }
    }
}