using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.EventSystems;

namespace BRM.InteractionAnalysis.UnityPlayback
{
    public static class PointerEventDataFactory
    {
        public static PointerEventData Create(ComponentTouchEvent model)
        {
            return new PointerEventData(EventSystem.current){position = model.TouchPoint};
        }

        public static BaseEventData Create()
        {
            return new BaseEventData(EventSystem.current);
        }
    }
}