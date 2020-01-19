using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class TransformReplayer : ComponentReplayer<TransformEvent, Transform>
    {
        protected override void OnGetComponent(TransformEvent model, Transform component)
        {
            component.position = model.Position;
            component.rotation = model.Rotation;
        }
    }
}