using System.Collections.Generic;
using BRM.InteractionRecorder.UnityUi.Models;
using UnityEngine.SceneManagement;

namespace BRM.InteractionRecorder.UnityUi.Subscribers
{
    public class SceneChangedSubscriber : UiEventSubscriber
    {
        public override string Name => nameof(DropdownSubscriber);
        
        private readonly List<SceneChangedEvent> _sceneChangedEvents = new List<SceneChangedEvent>();

        public override void ResetSubscriptions()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        public override void UnsubscribeAll()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        ~SceneChangedSubscriber()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene previousScene, Scene newScene)
        {
            var sceneChangedEvent = new SceneChangedEvent
            {
                OldSceneName = previousScene.name,
                NewSceneName = newScene.name,
            };
            _sceneChangedEvents.Add(sceneChangedEvent);
        }

        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.SceneChangedEvents.AddRange(_sceneChangedEvents);
            _sceneChangedEvents.Clear();
            return collection;
        }
    }
}