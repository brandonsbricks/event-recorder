using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;

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
            if (component == null || ReferenceEquals(null, component))//null ref check required for interfaces on monobehaviours
            {
                _debugger.LogWarning($"Component :{model.ComponentType} not found on gameobject :{model.GameObjectName} for event {model.EventType}. Skipping replay");
                return;
            }
            
            OnGetComponent(model, component);
        }

        protected abstract void OnGetComponent(TEventModel model, TComponent component);
    }
}