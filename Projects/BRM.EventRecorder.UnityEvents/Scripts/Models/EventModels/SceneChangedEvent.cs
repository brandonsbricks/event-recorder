using System;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class SceneChangedEvent : EventModelBase
    {
        public const string Name = nameof(SceneChangedEvent);
        
        public string OldSceneName;
        public string NewSceneName;

        public SceneChangedEvent() : base(Name)
        {
        }
    }
}