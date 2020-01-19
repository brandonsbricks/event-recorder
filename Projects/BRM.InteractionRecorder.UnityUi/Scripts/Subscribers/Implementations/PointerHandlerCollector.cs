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
        private readonly List<ComponentEvent> _events = new List<ComponentEvent>();

        private GameObject _lastHoveredGoForExit;
        private GameObject _lastDownedGo;

        public void OnUpdate()
        {
            List<GameObject> currentHoveredGos = null;
            if (EventSystem.current.currentInputModule is PointerEventInputModule inputModule)
            {
                currentHoveredGos = inputModule.GetHoveredGameObjects();
            }

            TryCreatePressEvents(currentHoveredGos);

            //only create hover events when the hovered GameObject changes
            if ((_lastHoveredGoForExit != null && currentHoveredGos == null) || //exiting to nothing
                (_lastHoveredGoForExit == null && currentHoveredGos != null) || //entering from nothing
                (currentHoveredGos != null && !currentHoveredGos.Contains(_lastHoveredGoForExit)) //moving to/from something else 
            )
            {
                TryCreateHoverEvents(currentHoveredGos);
            }

            _lastHoveredGoForExit = TryGetGameObjectWithComponent<IPointerExitHandler>(currentHoveredGos);
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.ComponentTouchEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }

        private void TryCreatePressEvents(List<GameObject> currentHoveredGos)
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                TryCreateEvent<IPointerDownHandler>(ComponentEvent.IPointerDownEvent, currentHoveredGos, out _lastDownedGo);
            }

            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                TryCreateEvent<IPointerUpHandler>(ComponentEvent.IPointerUpEvent, currentHoveredGos, out var dummy1);
                var currentDownableGo = TryGetGameObjectWithComponent<IPointerDownHandler>(currentHoveredGos);
                if (currentDownableGo == _lastDownedGo)
                {
                    TryCreateEvent<IPointerClickHandler>(ComponentEvent.IPointerClickEvent, currentHoveredGos, out var dummy2);
                }
            }
        }

        private void TryCreateHoverEvents(List<GameObject> currentHoveredGos)
        {
            TryCreateEvent<IPointerExitHandler>(ComponentEvent.IPointerExitEvent, _lastHoveredGoForExit);
            TryCreateEvent<IPointerEnterHandler>(ComponentEvent.IPointerEnterEvent, currentHoveredGos, out var dummy);
        }

        private void TryCreateEvent<T>(string eventType, List<GameObject> targetGos, out GameObject targetGo) where T : class
        {
            targetGo = null;
            if (targetGos == null)
            {
                return;
            }

            targetGo = TryGetGameObjectWithComponent<T>(targetGos);
            TryCreateEvent<T>(eventType, targetGo);
        }

        private GameObject TryGetGameObjectWithComponent<T>(List<GameObject> currentHoveredGos) where T : class
        {
            if (currentHoveredGos == null)
            {
                return null;
            }

            //process backwards to prioritize deepest in hierarchy / topmost ui element
            for (int i = currentHoveredGos.Count - 1; i >= 0; i--)
            {
                var targetGo = currentHoveredGos[i];
                if (targetGo == null)
                {
                    continue;
                }

                if (targetGo.GetComponent(typeof(T)))
                {
                    return targetGo;
                }
            }

            return null;
        }

        private void TryCreateEvent<T>(string eventType, GameObject targetGo) where T : class
        {
            if (targetGo == null)
            {
                return;
            }

            var newEvent = new ComponentEvent(eventType)
            {
                GameObjectName = UnityNamingUtils.GetHierarchyName(targetGo.transform),
                ComponentType = typeof(T).FullName,
                TouchPoint = Input.mousePosition,
            };
            _events.Add(newEvent);
        }
    }
}