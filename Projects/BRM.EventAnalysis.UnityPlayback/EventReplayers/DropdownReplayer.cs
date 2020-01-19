using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.UI;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class DropdownReplayer : ComponentReplayer<DropdownEvent, Dropdown>
    {
        protected override void OnGetComponent(DropdownEvent model, Dropdown component)
        {
            component.value = model.NewIntValue;
        }
    }
}