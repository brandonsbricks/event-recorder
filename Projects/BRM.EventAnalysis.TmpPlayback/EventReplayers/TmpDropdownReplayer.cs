using BRM.EventAnalysis.UnityPlayback;
using BRM.EventRecorder.UnityEvents.Models;
using TMPro;

namespace BRM.EventAnalysis.TmpPlayback
{
    public class TmpDropdownReplayer : ComponentReplayer<DropdownEvent, TMP_Dropdown>
    {
        protected override void OnGetComponent(DropdownEvent model, TMP_Dropdown component)
        {
            component.value = model.NewIntValue;
        }
    }
}