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
            
            var pointerDownReplayer = new PointerDownReplayer();
            var pointerUpReplayer = new PointerUpReplayer();
            var pointerClickReplayer = new PointerClickReplayer();
            var pointerEnterReplayer = new PointerEnterReplayer();
            var pointerExitReplayer = new PointerExitReplayer();
            
            var toggleReplayer = new ToggleReplayer();
            var sliderReplayer = new SliderReplayer();
            var dropdownReplayer = new DropdownReplayer();
            var textInputReplayer = new TextInputReplayer();
            
            var replayers = new Dictionary<string, Action<EventModelBase>>
            {
                {nameof(SceneChangedEvent), modelBase => sceneChangedReplayer.Replay(modelBase as SceneChangedEvent)},

                {SimpleTouchEvent.TouchUp, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {SimpleTouchEvent.TouchDown, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {nameof(SimpleTouchEvent), modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                
                {ComponentEvent.IPointerDownEvent, modelBase => pointerDownReplayer.Replay(modelBase as ComponentEvent)},
                {ComponentEvent.IPointerUpEvent, modelBase => pointerUpReplayer.Replay(modelBase as ComponentEvent)},
                {ComponentEvent.IPointerClickEvent, modelBase => pointerClickReplayer.Replay(modelBase as ComponentEvent)},
                {ComponentEvent.IPointerEnterEvent, modelBase => pointerEnterReplayer.Replay(modelBase as ComponentEvent)},
                {ComponentEvent.IPointerExitEvent, modelBase => pointerExitReplayer.Replay(modelBase as ComponentEvent)},

                {nameof(ToggleEvent), modelBase => toggleReplayer.Replay(modelBase as ToggleEvent)},
                {nameof(SliderEvent), modelBase => sliderReplayer.Replay(modelBase as SliderEvent)},
                {DropdownEvent.UnityDropdownEvent, modelBase => dropdownReplayer.Replay(modelBase as DropdownEvent)},
                {TextInputEvent.UnityTextInputEvent, modelBase => textInputReplayer.Replay(modelBase as TextInputEvent)},
            };
            return replayers;
        }
    }
}