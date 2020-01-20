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
        private event Action _onGui;

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
            var guiers = _recordingService.GetGuiers();
            guiers.ForEach(guier =>
            {
                if (!record)
                {
                    _onGui -= guier.OnGui;
                }
                else
                {
                    _onGui -= guier.OnGui;
                    _onGui += guier.OnGui;
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
                return;
            }
            _onUpdate?.Invoke();
            
            if (Input.GetMouseButtonDown(MouseButton.Left) || Input.GetMouseButtonUp(MouseButton.Left))
            {
                ResetSubscriptions();
            }
        }

        private void OnGUI()
        {
            if (!_recording)
            {
                return;
            }
            _onGui?.Invoke();
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
            var guiers = _recordingService.GetGuiers();
            guiers.ForEach(guier =>
            {
                _onGui -= guier.OnGui;
                _onGui += guier.OnGui;
            });
            _recordingService.ResetSubscriptions();
        }
        #endregion
    }
}