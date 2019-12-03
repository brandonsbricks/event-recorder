﻿using BRM.InteractionAnalysis.UnityPlayback;
using BRM.InteractionRecorder.UnityUi.Models;
using TMPro;

namespace BRM.InteractionAnalysis.TmpPlayback
{
    public class TmpTextInputReplayer : ComponentReplayer<TextInputEvent, TMP_InputField>
    {
        protected override void OnGetComponent(TextInputEvent model, TMP_InputField component)
        {
            component.text = model.NewValue;
            component.onEndEdit.Invoke(model.NewValue);
        }
    }

    public class TmpDropdownReplayer : ComponentReplayer<DropdownEvent, TMP_Dropdown>
    {
        protected override void OnGetComponent(DropdownEvent model, TMP_Dropdown component)
        {
            component.value = model.NewIntValue;
        }
    }
}