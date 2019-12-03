using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine;

namespace BRM.InteractionAnalysis.UnityPlayback
{
    public class ReplayController : MonoBehaviour
    {
        private Dictionary<string, Action<EventModelBase>> _replayInstructions;
        private IDebug _debugger = new UnityDebugger();

        /// <summary>
        /// Set replay instructions from <see cref="ReplayInstructionFactory"/> or another customized source which maps
        /// <para>an event type to its corresponding replay action</para>
        /// </summary>
        /// <param name="replayInstructions"></param>
        public void Initialize(Dictionary<string, Action<EventModelBase>> replayInstructions)
        {
            _replayInstructions = replayInstructions;
        }

        public void Replay(EventAndAppPayload payload)
        {
            StartCoroutine(ReplayEvents(payload.GetEventModels()));
        }

        private IEnumerator ReplayEvents(EventModelCollection events)
        {
            var allEvents = events.GetAllEvents();
            allEvents = allEvents.OrderBy(item => item.TimestampMillis).ToList();
            if (allEvents.Count == 0)
            {
                yield break;
            }

            var firstTimestamp = allEvents[0].TimestampMillis;

            float GetTimeSinceStart(long eventTimestamp)
            {
                return eventTimestamp - firstTimestamp;
            }

            for (int i = 0; i < allEvents.Count; i++)
            {
                var currentEvent = allEvents[i];
                var timeToWait = GetTimeSinceStart(currentEvent.TimestampMillis);
                if (!Mathf.Approximately(0, timeToWait))
                {
                    yield return new WaitForSeconds(timeToWait);
                }

                ReplayEvent(currentEvent);
            }
        }

        private void ReplayEvent(EventModelBase modelBase)
        {
            var eventType = modelBase.EventType;
            if (_replayInstructions.TryGetValue(eventType, out var instruction))
            {
                instruction?.Invoke(modelBase);
                return;
            }

            _debugger.LogWarning($"No event type found named:{eventType}");
        }
    }
}