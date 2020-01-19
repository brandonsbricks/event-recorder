using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class SceneChangedEvent : EventModelBase
    {
        public string OldSceneName;
        public string NewSceneName;

        public SceneChangedEvent() : base(nameof(SceneChangedEvent))
        {
        }
    }
}