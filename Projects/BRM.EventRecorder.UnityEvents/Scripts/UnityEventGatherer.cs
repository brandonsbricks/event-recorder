using System;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents
{
    public class UnityEventGatherer : MonoBehaviour
    {
        [SerializeField] private bool _recording;
        
        protected virtual EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new UnityEventServiceFactory().Create());
        protected EventService _eventServiceLocal;
        
        private event Action _onUpdate;

        #region Public
        /// <summary>
        /// Use this to define which events get recorded
        /// Set this during the Awake/Start function of a Monobehavior
        /// The <typeparamref name="UnityEventServiceFactory"/> will generate a default set of event recorders if
        /// this is not used
        /// </summary>
        public void SetEventRecorderService(EventService service)
        {
            _eventServiceLocal = service;
        }
        
        public void SetAppData(string gitSha, string server)
        {
            _eventService.SetAppValues(gitSha, server);
        }

        public EventAndAppPayload GetPayload()
        {
            _eventService.UpdatePayload();
            return _eventService.Payload;
        }

        public void ToggleRecording(bool record)
        {
            _recording = record;
            var updaters = _eventService.GetUpdaters();
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
            
            _eventService.ToggleRecording(record);
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
        #endregion
        
        #region Private

        private void ResetSubscriptions()
        {
            var updaters = _eventService.GetUpdaters();
            updaters.ForEach(updater =>
            {
                _onUpdate -= updater.OnUpdate;
                _onUpdate += updater.OnUpdate;
            });
            _eventService.ResetSubscriptions();
        }
        #endregion
    }
}