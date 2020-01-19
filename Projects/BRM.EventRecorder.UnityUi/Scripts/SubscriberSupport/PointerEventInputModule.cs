using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BRM.EventRecorder.UnityUi
{
    public class PointerEventInputModule : StandaloneInputModule
    {
        public List<GameObject> GetHoveredGameObjects()
        {
            var data = GetLastPointerEventData(kMouseLeftId);
            var numHovered = data?.hovered?.Count;
            return numHovered > 0 ? data.hovered : null;
        }
    }
}