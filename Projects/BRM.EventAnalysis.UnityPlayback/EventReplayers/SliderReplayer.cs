using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.UI;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class SliderReplayer : ComponentReplayer<SliderEvent, Slider>
    {
        protected override void OnGetComponent(SliderEvent model, Slider component)
        {
            component.value = model.NewValue;
        }
    }
}