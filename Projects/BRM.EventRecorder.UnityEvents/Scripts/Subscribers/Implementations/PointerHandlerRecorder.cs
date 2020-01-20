using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BRM.EventRecorder.UnityEvents.Subscribers
{
    public class PointerHandlerRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(PointerHandlerRecorder);
        private readonly List<PointerEvent> _events = new List<PointerEvent>();

        private GameObject _lastHoveredGoForExit;
        private GameObject _lastDownedGo;

        public void OnUpdate()//todo: measure performance. Much GetComponent in here
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
            collection.PointerEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }

        private void TryCreatePressEvents(List<GameObject> currentHoveredGos)
        {
            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                TryCreateEvent<IPointerDownHandler>(PointerEvent.IPointerDownEvent, currentHoveredGos, out _lastDownedGo);
            }

            if (Input.GetMouseButtonUp(MouseButton.Left))
            {
                TryCreateEvent<IPointerUpHandler>(PointerEvent.IPointerUpEvent, currentHoveredGos, out var dummy1);
                var currentDownableGo = TryGetGameObjectWithComponent<IPointerDownHandler>(currentHoveredGos);
                if (currentDownableGo == _lastDownedGo)
                {
                    TryCreateEvent<IPointerClickHandler>(PointerEvent.IPointerClickEvent, currentHoveredGos, out var dummy2);
                }
            }
        }

        private void TryCreateHoverEvents(List<GameObject> currentHoveredGos)
        {
            TryCreateEvent<IPointerExitHandler>(PointerEvent.IPointerExitEvent, _lastHoveredGoForExit);
            TryCreateEvent<IPointerEnterHandler>(PointerEvent.IPointerEnterEvent, currentHoveredGos, out var dummy);
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

            var newEvent = new PointerEvent(eventType)
            {
                GameObjectName = UnityNamingUtils.GetHierarchyName(targetGo.transform),
                ComponentType = typeof(T).FullName,
                Position = Input.mousePosition,
            };
            _events.Add(newEvent);
        }
    }
}