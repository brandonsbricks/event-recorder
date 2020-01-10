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
        private GameObject _currentGo => EventSystem.current.currentSelectedGameObject;
        private GameObject _lastSelectedGo;
        private GameObject _lastDownedGo;

        public void OnUpdate()
        {
            TryCreatePressEvents();
            
            //don't check for hover events if the selected gameobject remains the same
            if (_lastSelectedGo == _currentGo)
            {
                return;
            }
            
            TryCreateHoverEvents();
            _lastSelectedGo = _currentGo;
        }

        private void TryCreatePressEvents()
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                _lastDownedGo = _currentGo;
                TryCreateEvent<IPointerDownHandler>(ComponentTouchEvent.IPointerDownEvent, _currentGo);
            }

            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                TryCreateEvent<IPointerUpHandler>(ComponentTouchEvent.IPointerUpEvent, _currentGo);
                if (_currentGo == _lastDownedGo)
                {
                    TryCreateEvent<IPointerClickHandler>(ComponentTouchEvent.IPointerClickEvent, _currentGo);//bug: not properly registered. missing click events for the dropdown. perhaps the lastdownedgo is not properly assigned? perhaps the currentgo at that point is still one go behind?
                }
            }
        }

        private void TryCreateHoverEvents()
        {
            TryCreateEvent<IPointerExitHandler>(ComponentTouchEvent.IPointerExitEvent, _lastSelectedGo);
            TryCreateEvent<IPointerEnterHandler>(ComponentTouchEvent.IPointerEnterEvent, _currentGo);
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.ComponentTouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
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
                IsFromEventSubscription = false,
            };
            _events.Add(newEvent);
        }
    }
}