using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine;

namespace BRM.EventAnalysis.UnityPlayback
{
    public class ReplayController : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] private List<ReplayInstruction> _replayInstructions;
        [SerializeField] private bool _enableLogging = true;
        #pragma warning restore 0649
        
        private IDebug _debugger = new UnityDebugger();
        
        [Serializable]
        private class ReplayInstruction
        {
            public string Name;
            public bool EnableReplay = true;
            private readonly Action<EventModelBase> _onReplay;
            
            public ReplayInstruction(string eventType, Action<EventModelBase> onReplay)
            {
                Name = eventType;
                _onReplay = onReplay;
            }

            public void Replay(EventModelBase model)
            {
                _onReplay.Invoke(model);
            }
        }

        /// <summary>
        /// Set replay instructions from <see cref="ReplayInstructionFactory"/> or another customized source which maps
        /// <para>an event type to its corresponding replay action</para>
        /// </summary>
        /// <param name="replayInstructions"></param>
        public void Initialize(Dictionary<string, Action<EventModelBase>> replayInstructions)
        {
            foreach (var kvp in replayInstructions)
            {
                _replayInstructions.Add(new ReplayInstruction(kvp.Key, kvp.Value));
            }
        }

        public void Replay(EventAndAppPayload payload)
        {
            StartCoroutine(ReplayEvents(payload.GetEventModels()));
        }

        private void Update()
        {
            _debugger.Enabled = _enableLogging;
        }

        private IEnumerator ReplayEvents(EventModelCollection events)
        {
            var allEvents = events.GetAllEvents();
            allEvents = allEvents.OrderBy(item => item.TimestampMillis).ToList();
            if (allEvents.Count == 0)
            {
                yield break;
            }

            var firstTimestampMillis = allEvents[0].TimestampMillis;

            float GetTimeSinceStartSeconds(long timeStampMillis)
            {
                return (timeStampMillis - firstTimestampMillis) / 1000f;
            }

            for (int i = 0; i < allEvents.Count; i++)
            {
                var currentEvent = allEvents[i];
                var lastEventExists = i - 1 >= 0;
                var lastEvent = lastEventExists ? allEvents[i - 1] : null;
                var currentTimeSinceStartSeconds = GetTimeSinceStartSeconds(currentEvent.TimestampMillis);
                var lastTimeSinceStartSeconds = lastEventExists ? GetTimeSinceStartSeconds(lastEvent.TimestampMillis) : 0;
                var timeToWait = currentTimeSinceStartSeconds - lastTimeSinceStartSeconds;
                if (!Mathf.Approximately(0, timeToWait))
                {
                    yield return new WaitForSeconds(timeToWait);
                }

                ReplayEvent(currentEvent, currentTimeSinceStartSeconds);
            }
        }

        private void ReplayEvent(EventModelBase modelBase, float currentTimeSinceStartSeconds)
        {
            var eventType = modelBase.EventType;
            var instruction = _replayInstructions.Find(instr => instr.Name == eventType);
            if (instruction != null)
            {
                if (!instruction.EnableReplay)
                {
                    if (modelBase is ComponentEventModel comp)
                    {
                        _debugger.Log($"Replay disabled time:{currentTimeSinceStartSeconds:0.00}, type:{comp.EventType}, gameObject:{comp.GameObjectName}, comp:{comp.ComponentType}");
                    }
                    else
                    {
                        _debugger.Log($"Replay disabled time:{currentTimeSinceStartSeconds:0.00}, type:{modelBase.EventType}");
                    }
                    return;
                }
                
                if (modelBase is ComponentEventModel compo)
                {
                    _debugger.Log($"Replaying Event: time:{currentTimeSinceStartSeconds:0.00}, type:{compo.EventType}, gameObject:{compo.GameObjectName}, comp:{compo.ComponentType}");
                }
                else
                {
                    _debugger.Log($"Replaying Event: time:{currentTimeSinceStartSeconds:0.00}, type:{modelBase.EventType}");
                }

                instruction.Replay(modelBase);
                return;
            }

            _debugger.LogWarning($"No event type found named:{eventType}");
        }
    }
}