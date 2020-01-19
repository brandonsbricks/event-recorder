using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.UI;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class ToggleReplayer : ComponentReplayer<ToggleEvent, Toggle>
    {
        protected override void OnGetComponent(ToggleEvent model, Toggle component)
        {
            component.isOn = model.NewValue;
        }
    }
}