using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public abstract class TouchSubscriber<TSelectable, TModel> : SelectableSubscriber<TSelectable, TModel> where TSelectable : Object where TModel : ComponentTouchEvent
    {
        protected override void PopulateCommonEventData(TModel eventData, Transform componentTransform)
        {
            base.PopulateCommonEventData(eventData, componentTransform);
            eventData.TouchPoint = Input.mousePosition;
        }
    }
}