using System;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents
{
    public class UnityEventGatherer : MonoBehaviour
    {
        [SerializeField] private bool _recording;

        protected virtual RecordingService _recordingService => _recordingServiceLocal ?? (_recordingServiceLocal = new UnityRecordingServiceFactory().Create());
        protected RecordingService _recordingServiceLocal;

        private event Action _onUpdate;
        private int _lastTouchCount;

        #region Public

        /// <summary>
        /// Use this to define which events get recorded
        /// Set this during the Awake/Start function of a Monobehavior
        /// The <typeparamref name="UnityRecordingServiceFactory"/> will generate a default set of event recorders if
        /// this is not used
        /// </summary>
        public void SetEventRecordingService(RecordingService service)
        {
            _recordingServiceLocal = service;
        }

        public void SetAppData(string gitSha, string server)
        {
            _recordingService.SetAppValues(gitSha, server);
        }

        public EventAndAppPayload GetPayload()
        {
            _recordingService.UpdatePayload();
            return _recordingService.Payload;
        }

        public void ToggleRecording(bool record)
        {
            _recording = record;
            var updaters = _recordingService.GetUpdaters();
            updaters.ForEach(updater =>
            {
                if (!record)
                {
                    _onUpdate -= updater.OnUpdate;
                }
                else
                {
                    _onUpdate -= updater.OnUpdate;
                    _onUpdate += updater.OnUpdate;
                }
            });

            _recordingService.ToggleRecording(record);
        }

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            ResetSubscriptions();
        }

        private void Update()
        {
            if (!_recording)
            {
                _lastTouchCount = 0;
                return;
            }

            _onUpdate?.Invoke();

            if (Input.anyKeyDown || Input.GetMouseButtonUp(MouseButton.Left) || Input.touchCount != _lastTouchCount)
            {
                ResetSubscriptions();
            }

            _lastTouchCount = Input.touchCount;
        }

        #endregion

        #region Private

        private void ResetSubscriptions()
        {
            var updaters = _recordingService.GetUpdaters();
            updaters.ForEach(updater =>
            {
                _onUpdate -= updater.OnUpdate;
                _onUpdate += updater.OnUpdate;
            });
            _recordingService.ResetSubscriptions();
        }

        #endregion
    }
}