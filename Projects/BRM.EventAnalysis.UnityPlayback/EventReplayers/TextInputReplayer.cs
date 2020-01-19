using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.UI;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class TextInputReplayer : ComponentReplayer<TextInputEvent, InputField>
    {
        protected override void OnGetComponent(TextInputEvent model, InputField component)
        {
            component.text = model.NewValue;
            component.onEndEdit.Invoke(model.NewValue);
        }
    }
}