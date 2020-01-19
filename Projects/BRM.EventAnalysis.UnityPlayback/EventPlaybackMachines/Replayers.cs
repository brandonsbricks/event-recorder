using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using BRM.EventRecorder.UnityUi.Models;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BRM.EventAnalysis.UnityPlayback
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
                _debugger.LogWarning($"GameObject :{model.GameObjectName} not found for event {model.EventType}. Skipping replay");
                return;
            }

            var component = gameObject.GetComponent(typeof(TComponent)) as TComponent;
            if (component == null || ReferenceEquals(null, component))
            {
                _debugger.LogWarning($"Component :{model.ComponentType} not found on gameobject :{model.GameObjectName} for event {model.EventType}. Skipping replay");
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
    public class PointerClickReplayer : ComponentReplayer<ComponentEvent, IPointerClickHandler>
    {
        protected override void OnGetComponent(ComponentEvent model, IPointerClickHandler component)
        {
            component.OnPointerClick(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerDownReplayer : ComponentReplayer<ComponentEvent, IPointerDownHandler>
    {
        protected override void OnGetComponent(ComponentEvent model, IPointerDownHandler component)
        {
            component.OnPointerDown(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerUpReplayer : ComponentReplayer<ComponentEvent, IPointerUpHandler>
    {
        protected override void OnGetComponent(ComponentEvent model, IPointerUpHandler component)
        {
            component.OnPointerUp(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerEnterReplayer : ComponentReplayer<ComponentEvent, IPointerEnterHandler>
    {
        protected override void OnGetComponent(ComponentEvent model, IPointerEnterHandler component)
        {
            component.OnPointerEnter(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerExitReplayer : ComponentReplayer<ComponentEvent, IPointerExitHandler>
    {
        protected override void OnGetComponent(ComponentEvent model, IPointerExitHandler component)
        {
            component.OnPointerExit(PointerEventDataFactory.Create(model));
        }
    }

    public class ToggleReplayer : ComponentReplayer<ToggleEvent, Toggle>
    {
        protected override void OnGetComponent(ToggleEvent model, Toggle component)
        {
            component.isOn = model.NewValue;
        }
    }
    
    public class SliderReplayer : ComponentReplayer<SliderEvent, Slider>
    {
        protected override void OnGetComponent(SliderEvent model, Slider component)
        {
            component.value = model.NewValue;
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
}