using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using BRM.InteractionRecorder.UnityUi.Subscribers;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BRM.InteractionAnalysis.UnityPlayback
{
    public abstract class Replayer<TEventModel> where TEventModel : EventModelBase
    {
        public abstract void Replay(TEventModel model);
    }

    public abstract class ComponentReplayer<TEventModel, TComponent> : Replayer<TEventModel> where TComponent : class where TEventModel : ComponentEventModel
    {
        private IDebug _debugger = new UnityDebugger();

        public override void Replay(TEventModel model)
        {
            if (model == null)
            {
                _debugger.LogWarning("Model is null. Is this eventType improperly cast? Skipping replay");
                return;
            }

            var gameObject = GameObjectFinder.Find(model.GameObjectName);
            if (gameObject == null)
            {
                _debugger.LogWarning($"GameObject not found with name:{model.GameObjectName}. Is the gameobject disabled? This may be expected and insignificant with dropdown toggles. Skipping replay");
                return;
            }

            var component = gameObject.GetComponent(typeof(TComponent)) as TComponent;
            if (component == null || ReferenceEquals(null, component))
            {
                _debugger.LogWarning($"Component of type:{model.ComponentType} not found on gameobject:{model.GameObjectName}. Skipping replay");
                return;
            }
            
            OnGetComponent(model, component);
        }

        protected abstract void OnGetComponent(TEventModel model, TComponent component);
    }

    public class SceneChangedReplayer : Replayer<SceneChangedEvent>
    {
        public override void Replay(SceneChangedEvent model)
        {
            //todo: display scene change ui
        }
    }

    public class SimpleTouchReplayer : Replayer<SimpleTouchEvent>
    {
        public override void Replay(SimpleTouchEvent model)
        {
            //todo: display touch ui
        }
    }
    public class PointerClickReplayer : ComponentReplayer<ComponentTouchEvent, IPointerClickHandler>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, IPointerClickHandler component)
        {
            component.OnPointerClick(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerDownReplayer : ComponentReplayer<ComponentTouchEvent, IPointerDownHandler>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, IPointerDownHandler component)
        {
            component.OnPointerDown(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerUpReplayer : ComponentReplayer<ComponentTouchEvent, IPointerUpHandler>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, IPointerUpHandler component)
        {
            component.OnPointerUp(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerEnterReplayer : ComponentReplayer<ComponentTouchEvent, IPointerEnterHandler>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, IPointerEnterHandler component)
        {
            component.OnPointerEnter(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerExitReplayer : ComponentReplayer<ComponentTouchEvent, IPointerExitHandler>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, IPointerExitHandler component)
        {
            component.OnPointerExit(PointerEventDataFactory.Create(model));
        }
    }

    public class ButtonReplayer : ComponentReplayer<ComponentTouchEvent, Button>
    {
        protected override void OnGetComponent(ComponentTouchEvent model, Button component)
        {
            component.onClick.Invoke();
        }
    }

    public class ToggleReplayer : ComponentReplayer<ToggleEvent, Toggle>
    {
        protected override void OnGetComponent(ToggleEvent model, Toggle component)
        {
            component.isOn = model.NewValue;
        }
    }

    public class DropdownReplayer : ComponentReplayer<DropdownEvent, Dropdown>
    {
        protected override void OnGetComponent(DropdownEvent model, Dropdown component)
        {
            component.value = model.NewIntValue;
        }
    }

    public class TextInputReplayer : ComponentReplayer<TextInputEvent, InputField>
    {
        protected override void OnGetComponent(TextInputEvent model, InputField component)
        {
            component.text = model.NewValue;
            component.onEndEdit.Invoke(model.NewValue);
        }
    }

    public class EventTriggerReplayer : ComponentReplayer<ComponentTouchEvent, EventTrigger>
    {
        private IDebug _debugger = new UnityDebugger();

        protected override void OnGetComponent(ComponentTouchEvent model, EventTrigger component)
        {
            foreach (var trigger in component.triggers)
            {
                if (EventTriggerSubscriber.TriggerTypeToString(trigger.eventID) == model.EventType)
                {
                    trigger.callback.Invoke(new BaseEventData(EventSystem.current));
                    return;
                }
            }

            _debugger.LogError($"No EventTriggerType found for eventtype:{model.EventType}");
        }
    }
}