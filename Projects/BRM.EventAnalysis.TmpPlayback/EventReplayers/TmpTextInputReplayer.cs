using BRM.EventAnalysis.UnityPlayback;
using BRM.EventRecorder.UnityEvents.Models;
using TMPro;

namespace BRM.EventAnalysis.TmpPlayback
{
    public class TmpTextInputReplayer : ComponentReplayer<TextInputEvent, TMP_InputField>
    {
        protected override void OnGetComponent(TextInputEvent model, TMP_InputField component)
        {
            component.text = model.NewValue;
            component.onEndEdit.Invoke(model.NewValue);
        }
    }
}