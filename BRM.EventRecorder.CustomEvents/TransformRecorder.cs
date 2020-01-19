using System;
using System.Collections.Generic;
using BRM.EventRecorder.UnityUi;
using BRM.EventRecorder.UnityUi.Interfaces;
using BRM.EventRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.EventRecorder.CustomEvents
{
    public class TransformRecorder : EventCollector, IUpdate
    {
        private Transform _transform;
        private float _timeBetweenRecordsSec;
        private float _timeOfLastRecord;
        private List<TransformData> _records = new List<TransformData>();

        public TransformRecorder(Transform tran, float timeBetweenRecordsSec)
        {
            _transform = tran;
            _timeBetweenRecordsSec = timeBetweenRecordsSec;
        }

        public override string Name => nameof(TransformRecorder);

        public override EventModelCollection ExtractNewEvents()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            if (Time.time - _timeOfLastRecord > _timeBetweenRecordsSec)
            {
                if (_transform == null)
                {
                    return;
                }

                _records.Add(new TransformData
                {
                    ComponentType = "Transform",
                    GameObjectName = UnityNamingUtils.GetHierarchyName(_transform),
                    Position = _transform.position,
                    Rotation = _transform.rotation
                });
                _timeOfLastRecord = Time.time;
            }
        }
    }

    [Serializable]
    public class TransformData : ComponentEventModel
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public TransformData() : base(nameof(TransformData))
        {
        }
    }
}