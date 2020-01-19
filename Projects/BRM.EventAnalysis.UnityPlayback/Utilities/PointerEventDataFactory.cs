using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.EventSystems;

namespace BRM.EventAnalysis.UnityPlayback
{
    public static class PointerEventDataFactory
    {
        public static PointerEventData Create(PointerEvent model)
        {
            return new PointerEventData(EventSystem.current){position = model.TouchPoint};
        }
    }
}