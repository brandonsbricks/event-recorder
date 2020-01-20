using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public abstract class TouchSubscriber<TSelectable, TModel> : SelectableSubscriber<TSelectable, TModel> where TSelectable : Object where TModel : PointerEvent
    {
        protected override void PopulateCommonEventData(ref TModel eventData, Transform componentTransform)
        {
            base.PopulateCommonEventData(ref eventData, componentTransform);
            eventData.Position = Input.mousePosition;
        }
    }
}