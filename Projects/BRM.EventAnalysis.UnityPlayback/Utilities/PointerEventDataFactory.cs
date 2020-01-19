using BRM.EventRecorder.UnityUi.Models;
using UnityEngine.EventSystems;

namespace BRM.EventAnalysis.UnityPlayback
{
    public static class PointerEventDataFactory
    {
        public static PointerEventData Create(ComponentEvent model)
        {
            return new PointerEventData(EventSystem.current){position = model.TouchPoint};
        }
    }
}