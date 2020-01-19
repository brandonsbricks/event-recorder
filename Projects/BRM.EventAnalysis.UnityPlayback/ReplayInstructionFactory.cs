using System;
using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Models;

namespace BRM.EventAnalysis.UnityPlayback
{
    //todo: acceptable ocp violation. modify when recorder is added
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

            var transformReplayer = new TransformReplayer();
            
            var replayers = new Dictionary<string, Action<EventModelBase>>
            {
                {nameof(SceneChangedEvent), modelBase => sceneChangedReplayer.Replay(modelBase as SceneChangedEvent)},

                {SimpleTouchEvent.TouchUp, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {SimpleTouchEvent.TouchDown, modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                {nameof(SimpleTouchEvent), modelBase => simpleTouchReplayer.Replay(modelBase as SimpleTouchEvent)},
                
                {PointerEvent.IPointerDownEvent, modelBase => pointerDownReplayer.Replay(modelBase as PointerEvent)},
                {PointerEvent.IPointerUpEvent, modelBase => pointerUpReplayer.Replay(modelBase as PointerEvent)},
                {PointerEvent.IPointerClickEvent, modelBase => pointerClickReplayer.Replay(modelBase as PointerEvent)},
                {PointerEvent.IPointerEnterEvent, modelBase => pointerEnterReplayer.Replay(modelBase as PointerEvent)},
                {PointerEvent.IPointerExitEvent, modelBase => pointerExitReplayer.Replay(modelBase as PointerEvent)},

                {nameof(ToggleEvent), modelBase => toggleReplayer.Replay(modelBase as ToggleEvent)},
                {nameof(SliderEvent), modelBase => sliderReplayer.Replay(modelBase as SliderEvent)},
                {DropdownEvent.UnityDropdownEvent, modelBase => dropdownReplayer.Replay(modelBase as DropdownEvent)},
                {TextInputEvent.UnityTextInputEvent, modelBase => textInputReplayer.Replay(modelBase as TextInputEvent)},
                
                {nameof(TransformEvent), modelBase => transformReplayer.Replay(modelBase as TransformEvent)},
            };
            return replayers;
        }
    }
}