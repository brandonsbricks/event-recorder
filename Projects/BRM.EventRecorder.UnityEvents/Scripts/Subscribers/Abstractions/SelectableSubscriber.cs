using System.Collections.Generic;
using System.Linq;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BRM.EventRecorder.UnityEvents.Subscribers
{
    public abstract class SelectableSubscriber<TSelectable, TModel> : EventSubscriber where TSelectable : Object where TModel : ComponentEventModel
    {
        protected abstract void OnSubscribe(TSelectable selectable);
        protected abstract void OnUnsubscribe(TSelectable selectable);

        public override void ResetSubscriptions()
        {
            UnsubscribeAll();
            var selectables = GetAllSelectables();
            for (int i = 0; i < selectables.Count; i++)
            {
                var sel = selectables[i];
                OnSubscribe(sel);
            }
        }

        public override void UnsubscribeAll()
        {
            var selectables = GetAllSelectables();
            for (int i = 0; i < selectables.Count; i++)
            {
                var sel = selectables[i];
                OnUnsubscribe(sel);
            }
        }
        
        
        protected virtual void PopulateCommonEventData(ref TModel eventData, Transform componentTransform)
        {
            eventData.GameObjectName = UnityNamingUtils.GetHierarchyName(componentTransform);
            eventData.ComponentType = typeof(TSelectable).FullName;//todo: measure performance
        }

        private static List<TSelectable> GetAllSelectables()
        {
            return Object.FindObjectsOfType<TSelectable>().ToList();
        }
    }
}