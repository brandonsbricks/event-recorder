using System;
using UnityEngine;
using BRM.InteractionRecorder.UnityUi.Models;
using BRM.InteractionRecorder.UnityUi.Subscribers;

namespace BRM.InteractionRecorder.UnityUi
{
    public class InteractionGatherer : MonoBehaviour
    {
        protected virtual EventService _eventService
        {
            get
            {
                if (_eventServiceLocal == null)
                {
                    _eventServiceLocal = new UnityEventServiceFactory().Create();
                }

                return _eventServiceLocal;
            }
        }
        
        [SerializeField] private bool _recording;
        private EventService _eventServiceLocal;
        private event Action _onUpdate;
        

        #region Public
        /// <summary>
        /// Use this to define which events get collected
        /// Set this during the Awake/Start function of a Monobehavior
        /// The <typeparam name="EventCollectorServiceGenerator"/> will generate a default set of event collectors if
        /// this is not used
        /// </summary>
        public void SetEventCollectorService(EventService collector)
        {
            _eventServiceLocal = collector;
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
            var standardTouch = _eventService.GetCollector<StandardTouchSubscriber>();//todo: refactor for grabbing any/all implementers of IUpdate
            if (standardTouch != null)
            {
                if (!record)
                {
                    _onUpdate -= standardTouch.OnUpdate;
                }
                else
                {
                    _onUpdate -= standardTouch.OnUpdate;
                    _onUpdate += standardTouch.OnUpdate;
                }
            }
            
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
            if (_onUpdate != null && _recording)
            {
                _onUpdate();
            }
            
            if (!_recording || !(Input.GetMouseButtonDown(MouseButton.Left) || Input.GetMouseButtonUp(MouseButton.Left)))
            {
                return;
            }

            ResetSubscriptions();
        }
        #endregion
        
        #region Private

        private void ResetSubscriptions()
        {
            var standardTouch = _eventService.GetCollector<StandardTouchSubscriber>();//todo: refactor for grabbing any/all implementers of IUpdate
            if (standardTouch != null)
            {
                _onUpdate -= standardTouch.OnUpdate;
                _onUpdate += standardTouch.OnUpdate;
            }
            _eventService.ResetSubscriptions();
        }
        #endregion
    }
}