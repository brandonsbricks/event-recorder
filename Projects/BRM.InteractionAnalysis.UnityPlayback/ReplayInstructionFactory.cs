using System;
using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;

namespace BRM.InteractionAnalysis.UnityPlayback
{
    public class ReplayInstructionFactory
    {
        public virtual Dictionary<string, Action<EventModelBase>> GetInstructions()
        {
            var sceneChangedReplayer = new SceneChangedReplayer();
            var simpleTouchReplayer = new SimpleTouchReplayer();
            var buttonReplayer = new ButtonReplayer();
            var toggleReplayer = new ToggleReplayer();
            var dropdownReplayer = new DropdownReplayer();
            var textInputReplayer = new TextInputReplayer();
            var eventTriggerReplayer = new EventTriggerReplayer();
            
            var replayers = new Dictionary<string, Action<EventModelBase>>
            {
                {nameof(SceneChangedEvent), modelBase => sceneChangedReplayer.Replay(modelBase as SceneChangedEvent)},

                {SimpleTouchEvent.TouchUp, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {SimpleTouchEvent.TouchDown, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {nameof(SimpleTouchEvent), modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},

                {nameof(ButtonEvent), modelBase => buttonReplayer.Replay(modelBase as ButtonEvent)},

                {EventTriggerEvent.EventTriggerUpEvent, modelBase => eventTriggerReplayer.Replay(modelBase as EventTriggerEvent)},
                {EventTriggerEvent.EventTriggerClickEvent, modelBase => eventTriggerReplayer.Replay(modelBase as EventTriggerEvent)},
                {EventTriggerEvent.EventTriggerDownEvent, modelBase => eventTriggerReplayer.Replay(modelBase as EventTriggerEvent)},
                {EventTriggerEvent.EventTriggerUnknownEvent, modelBase => eventTriggerReplayer.Replay(modelBase as EventTriggerEvent)},

                {nameof(ToggleEvent), modelBase => toggleReplayer.Replay(modelBase as ToggleEvent)},
                
                {DropdownEvent.UnityDropdownEvent, modelBase => dropdownReplayer.Replay(modelBase as DropdownEvent)},
                {TextInputEvent.UnityTextInputEvent, modelBase => textInputReplayer.Replay(modelBase as TextInputEvent)},
            };
            return replayers;
        }
    }
}