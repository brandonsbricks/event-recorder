using UnityEngine;
using UnityEngine.EventSystems;

namespace BRM.InteractionRecorder.UnityUi
{
    public class PointerEventInputModule : StandaloneInputModule
    {
        public GameObject GetHoveredGameObject()
        {
            var data = GetLastPointerEventData(kMouseLeftId);
            return data.hovered.Count > 0 ? data.hovered[0] : null;
        }
    }
}