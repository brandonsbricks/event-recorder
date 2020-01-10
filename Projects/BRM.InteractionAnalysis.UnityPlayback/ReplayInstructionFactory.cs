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
            var pointerDownReplayer = new PointerDownReplayer();
            var pointerUpReplayer = new PointerUpReplayer();
            var pointerClickReplayer = new PointerClickReplayer();
            var pointerEnterReplayer = new PointerEnterReplayer();
            var pointerExitReplayer = new PointerExitReplayer();
            
            var replayers = new Dictionary<string, Action<EventModelBase>>
            {
                {nameof(SceneChangedEvent), modelBase => sceneChangedReplayer.Replay(modelBase as SceneChangedEvent)},

                {SimpleTouchEvent.TouchUp, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {SimpleTouchEvent.TouchDown, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {nameof(SimpleTouchEvent), modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},

                {ComponentTouchEvent.ButtonEvent, modelBase => buttonReplayer.Replay(modelBase as ComponentTouchEvent)},
                
                {ComponentTouchEvent.EventTriggerUpEvent, modelBase => eventTriggerReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.EventTriggerClickEvent, modelBase => eventTriggerReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.EventTriggerDownEvent, modelBase => eventTriggerReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.EventTriggerUnknownEvent, modelBase => eventTriggerReplayer.Replay(modelBase as ComponentTouchEvent)},
                
                {ComponentTouchEvent.IPointerClickEvent, modelBase => pointerClickReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.IPointerDownEvent, modelBase => pointerDownReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.IPointerUpEvent, modelBase => pointerUpReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.IPointerEnterEvent, modelBase => pointerEnterReplayer.Replay(modelBase as ComponentTouchEvent)},
                {ComponentTouchEvent.IPointerExitEvent, modelBase => pointerExitReplayer.Replay(modelBase as ComponentTouchEvent)},

                {nameof(ToggleEvent), modelBase => toggleReplayer.Replay(modelBase as ToggleEvent)},
                
                {DropdownEvent.UnityDropdownEvent, modelBase => dropdownReplayer.Replay(modelBase as DropdownEvent)},
                {TextInputEvent.UnityTextInputEvent, modelBase => textInputReplayer.Replay(modelBase as TextInputEvent)},
            };
            return replayers;
        }
    }
}