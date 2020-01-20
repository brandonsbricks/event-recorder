using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class TransformRecorder : EventRecorder, IUpdate
    {
        public override string Name => nameof(TransformRecorder);

        private class TransformRecordTimes
        {
            public Transform Transform;
            public float TimeBetweenRecordsSec;

            public float TimeOfLastRecordSec;

            public TransformRecordTimes(Transform transform, float timeBetweenRecordsSec)
            {
                Transform = transform;
                TimeBetweenRecordsSec = timeBetweenRecordsSec;
            }
        }

        private readonly List<TransformRecordTimes> _transformTimes = new List<TransformRecordTimes>();
        private readonly List<TransformEvent> _records = new List<TransformEvent>();

        public void AddTransform(Transform tran, float timeBetweenRecordsSec)
        {
            if (tran == null || timeBetweenRecordsSec <= 0f)
            {
                return;
            }

            _transformTimes.Add(new TransformRecordTimes(tran, timeBetweenRecordsSec));
        }

        public void RemoveTransform(Transform tran)
        {
            _transformTimes.RemoveAll(tTime => tTime.Transform == tran);
        }

        public void ClearTransforms()
        {
            _transformTimes.Clear();
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.TransformEvents.AddRange(_records);
            _records.Clear();

            return collection;
        }

        public void OnUpdate()
        {
            for (int i = 0; i < _transformTimes.Count; i++)
            {
                var tranTime = _transformTimes[i];
                if (tranTime == null || tranTime.Transform == null)
                {
                    continue;
                }

                if (Time.time - tranTime.TimeOfLastRecordSec > tranTime.TimeBetweenRecordsSec)
                {
                    _records.Add(new TransformEvent
                    {
                        ComponentType = typeof(Transform).FullName,
                        GameObjectName = UnityNamingUtils.GetHierarchyName(tranTime.Transform),
                        Position = tranTime.Transform.position,
                        Rotation = tranTime.Transform.rotation
                    });
                    tranTime.TimeOfLastRecordSec = Time.time;
                }
            }
        }
    }
}