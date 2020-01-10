using System;
using UnityEngine;
using BRM.InteractionRecorder.UnityUi.Models;

namespace BRM.InteractionRecorder.UnityUi
{
    public class UnityInteractionGatherer : MonoBehaviour
    {
        [SerializeField] private bool _recording;
        
        protected virtual EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new UnityEventServiceFactory().Create());
        protected EventService _eventServiceLocal;
        private event Action _onUpdate;

        #region Public
        /// <summary>
        /// Use this to define which events get collected
        /// Set this during the Awake/Start function of a Monobehavior
        /// The <typeparamref name="UnityEventServiceFactory"/> will generate a default set of event collectors if
        /// this is not used
        /// </summary>
        public void SetEventCollectorService(EventService service)
        {
            _eventServiceLocal = service;
        }

        public EventAndAppPayload GetPayload()
        {
            _eventService.UpdatePayload();
            return _eventService.Payload;
        }

        public void SetServer(string server)
        {
            _eventService.SetServer(server);
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
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
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