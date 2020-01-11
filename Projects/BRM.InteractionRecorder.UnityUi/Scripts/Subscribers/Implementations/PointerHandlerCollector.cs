using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class PointerHandlerCollector : UiEventCollector, IUpdate
    {
        public override string Name => nameof(PointerHandlerCollector);
        private readonly List<ComponentTouchEvent> _events = new List<ComponentTouchEvent>();
        
        private GameObject _lastHoveredGo;
        private GameObject _lastDownedGo;

        public void OnUpdate()
        {
            GameObject currentHoveredGo = null;
            if (EventSystem.current.currentInputModule is PointerEventInputModule inputModule)
            {
                currentHoveredGo = inputModule.GetHoveredGameObject();//todo: return null when expecting a go
            }

            TryCreatePressEvents(currentHoveredGo);
            
            //only check for hover events when the hovered GameObject changes
            if (currentHoveredGo != _lastHoveredGo)
            {
                TryCreateHoverEvents(currentHoveredGo);
            }
            _lastHoveredGo = currentHoveredGo;
        }
        
        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.ComponentTouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }

        private void TryCreatePressEvents(GameObject currentHoveredGo)
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                _lastDownedGo = currentHoveredGo;
                TryCreateEvent<IPointerDownHandler>(ComponentTouchEvent.IPointerDownEvent, currentHoveredGo);
            }

            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                TryCreateEvent<IPointerUpHandler>(ComponentTouchEvent.IPointerUpEvent, currentHoveredGo);
                if (currentHoveredGo == _lastDownedGo)
                {
                    TryCreateEvent<IPointerClickHandler>(ComponentTouchEvent.IPointerClickEvent, currentHoveredGo);//bug: not properly registered. missing click events for the dropdown. perhaps the lastdownedgo is not properly assigned? perhaps the currentgo at that point is still one go behind?
                }
            }
        }

        private void TryCreateHoverEvents(GameObject currentHoveredGo)
        {
            TryCreateEvent<IPointerExitHandler>(ComponentTouchEvent.IPointerExitEvent, _lastHoveredGo);
            TryCreateEvent<IPointerEnterHandler>(ComponentTouchEvent.IPointerEnterEvent, currentHoveredGo);
        }

        private void TryCreateEvent<T>(string eventType, GameObject targetGo) where T : class
        {
            if (targetGo == null)
            {
                return;
            }

            var comp = targetGo.GetComponent(typeof(T));
            if (comp == null)
            {
                return;
            }

            var newEvent = new ComponentTouchEvent(eventType)
            {
                GameObjectName = UnityNamingUtils.GetHierarchyName(targetGo.transform),
                ComponentType = typeof(T).FullName,
                TouchPoint = Input.mousePosition,
            };
            _events.Add(newEvent);
        }
    }
}