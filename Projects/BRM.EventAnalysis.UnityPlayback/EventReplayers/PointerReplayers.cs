using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.EventSystems;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class PointerClickReplayer : ComponentReplayer<PointerEvent, IPointerClickHandler>
    {
        protected override void OnGetComponent(PointerEvent model, IPointerClickHandler component)
        {
            component.OnPointerClick(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerDownReplayer : ComponentReplayer<PointerEvent, IPointerDownHandler>
    {
        protected override void OnGetComponent(PointerEvent model, IPointerDownHandler component)
        {
            component.OnPointerDown(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerUpReplayer : ComponentReplayer<PointerEvent, IPointerUpHandler>
    {
        protected override void OnGetComponent(PointerEvent model, IPointerUpHandler component)
        {
            component.OnPointerUp(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerEnterReplayer : ComponentReplayer<PointerEvent, IPointerEnterHandler>
    {
        protected override void OnGetComponent(PointerEvent model, IPointerEnterHandler component)
        {
            component.OnPointerEnter(PointerEventDataFactory.Create(model));
        }
    }
    public class PointerExitReplayer : ComponentReplayer<PointerEvent, IPointerExitHandler>
    {
        protected override void OnGetComponent(PointerEvent model, IPointerExitHandler component)
        {
            component.OnPointerExit(PointerEventDataFactory.Create(model));
        }
    }
}