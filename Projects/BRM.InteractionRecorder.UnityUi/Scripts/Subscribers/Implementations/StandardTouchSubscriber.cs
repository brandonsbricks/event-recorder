using System.Collections.Generic;
using System.Linq;
using BRM.InteractionRecorder.UnityUi.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class StandardTouchSubscriber : UiEventCollector, IUpdate
    {
        public override string Name => nameof(StandardTouchSubscriber);
        private readonly List<TouchEvent> _events = new List<TouchEvent>();
        
        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                CreateEvent(TouchEvent.TouchDown);
            }
            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                CreateEvent(TouchEvent.TouchUp);
            }
        }
        
        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.TouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }

        
        private void CreateEvent(string eventType)
        {
            var eventSystem = EventSystem.current;
            var selected = eventSystem.currentSelectedGameObject;
            
            bool goIsNull;
            List<Component> componentsOnHere = null;
            //Null reference exceptions being thrown even when null check shows selectedGo is NOT null. This catches
            //this strange edge case where GetComponents<> throws exceptions on this missing go reference
            try
            {
                componentsOnHere = selected.GetComponents<Component>().ToList();
                goIsNull = false;
            }
            catch
            {
                goIsNull = true;
            }

            
            var componentTypeNames = goIsNull ? "-" : UnityNamingUtils.GetComponentTypeNames(componentsOnHere);
            var goHierarchyName = goIsNull ? "-" : UnityNamingUtils.GetHierarchyName(selected.transform);
                
            var newEvent = new TouchEvent
            {
                SceneName = SceneManager.GetActiveScene().name,
                GameObjectName = goHierarchyName,
                ComponentType = componentTypeNames,
                TouchPointProp = new Vector3S(Input.mousePosition),
                EventType = eventType,
                IsFromEventSubscription = false,
            };
            _events.Add(newEvent);
        }
    }
}