using BRM.EventRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityUi.Subscribers
{
    public abstract class TouchSubscriber<TSelectable, TModel> : SelectableSubscriber<TSelectable, TModel> where TSelectable : Object where TModel : ComponentEvent
    {
        protected override void PopulateCommonEventData(TModel eventData, Transform componentTransform)
        {
            base.PopulateCommonEventData(eventData, componentTransform);
            eventData.TouchPoint = Input.mousePosition;
        }
    }
}