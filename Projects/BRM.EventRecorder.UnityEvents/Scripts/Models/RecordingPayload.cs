using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BRM.EventRecorder.UnityEvents.Models
{
    [Serializable]
    public class EventAndAppPayload
    {
        [SerializeField] private AppData AppData = new AppData();
        [SerializeField] private DeviceData DeviceData = new DeviceData();
        [SerializeField] private EventModelCollection EventModels = new EventModelCollection();

        public int EventCount => EventModels.EventCount;

        public void SetAppValues(string gitSha, string server)
        {
            AppData.SetValues(gitSha, server);
        }
        
        public void UpdateProperties()
        {
            AppData.SetVersion();
            DeviceData.SetProperties();
        }

        public void AppendNewEventsUniquely(EventModelCollection newEvents)
        {
            EventModels.AppendNewEventsUniquely(newEvents);
        }

        public EventModelCollection GetEventModels()
        {
            EventModels.SortByTimestamp();
            return EventModels;
        }

        public void ClearRecording()
        {
            EventModels = new EventModelCollection();
        }
    }
    
    [Serializable]
    public class AppData
    {
        public string AppVersion;
        public string UnityVersion;
        public bool IsEditor;

        public string GitSha = "unassigned";
        public string Server = "unassigned";

        public void SetVersion()
        {
            AppVersion = Application.version;
            UnityVersion = Application.unityVersion;
            IsEditor = Application.isEditor;
        }

        public void SetValues(string gitSha, string server)
        {
            GitSha = gitSha;
            Server = server;
        }
    }

    [Serializable]
    public class DeviceData
    {
        public string DeviceModel;
        public string DeviceType;
        public string GraphicsDeviceName;
        public string GraphicsDeviceType;
        public int MemorySizeMB;
        public string OperatingSystemFamily;
        public Vector2Int ScreenSizePixels;
        
        public void SetProperties()
        {
            DeviceModel = SystemInfo.deviceModel;
            DeviceType = SystemInfo.deviceType.ToString();
            GraphicsDeviceName = SystemInfo.graphicsDeviceName;
            GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
            MemorySizeMB = SystemInfo.systemMemorySize;
            OperatingSystemFamily = SystemInfo.operatingSystemFamily.ToString();
            ScreenSizePixels = new Vector2Int(Screen.width, Screen.height);
        }
    }

    //todo: acceptable ocp violation. modify when event data type is added
    [Serializable]
    public class EventModelCollection
    {
        public List<SceneChangedEvent> SceneChangedEvents = new List<SceneChangedEvent>();
        public List<PositionEvent> SimpleTouchEvents = new List<PositionEvent>();
        public List<StringEvent> StringEvents = new List<StringEvent>();
        public List<PointerEvent> PointerEvents = new List<PointerEvent>();
        public List<TextInputEvent> TextInputEvents = new List<TextInputEvent>();
        public List<SliderEvent> SliderEvents = new List<SliderEvent>();
        public List<DropdownEvent> DropdownEvents = new List<DropdownEvent>();
        public List<ToggleEvent> ToggleEvents = new List<ToggleEvent>();
        public List<TransformEvent> TransformEvents = new List<TransformEvent>();

        public List<EventModelBase> GetAllEvents()
        {
            var allEvents = new List<EventModelBase>();
            allEvents.AddRange(SceneChangedEvents);
            allEvents.AddRange(SimpleTouchEvents);
            allEvents.AddRange(StringEvents);
            allEvents.AddRange(PointerEvents);
            allEvents.AddRange(TextInputEvents);
            allEvents.AddRange(SliderEvents);
            allEvents.AddRange(DropdownEvents);
            allEvents.AddRange(ToggleEvents);
            allEvents.AddRange(TransformEvents);
            return allEvents;
        }

        public int EventCount => SceneChangedEvents.Count + SimpleTouchEvents.Count + StringEvents.Count + PointerEvents.Count + 
                                 TextInputEvents.Count + SliderEvents.Count + DropdownEvents.Count + 
                                 ToggleEvents.Count + TransformEvents.Count;
        
        /// <summary>
        /// Removes any events already in the collection
        /// </summary>
        public void AppendNewEventsUniquely(EventModelCollection newCollection)
        {
            AddUnique(SceneChangedEvents, newCollection.SceneChangedEvents);
            AddUnique(SimpleTouchEvents, newCollection.SimpleTouchEvents);
            AddUnique(StringEvents, newCollection.StringEvents);
            AddUnique(PointerEvents, newCollection.PointerEvents);
            AddUnique(TextInputEvents, newCollection.TextInputEvents);
            AddUnique(SliderEvents, newCollection.SliderEvents);
            AddUnique(DropdownEvents, newCollection.DropdownEvents);
            AddUnique(ToggleEvents, newCollection.ToggleEvents);
            AddUnique(TransformEvents, newCollection.TransformEvents);
        }
        
        public void SortByTimestamp()
        {
            Func<EventModelBase, long> orderFunc = item => item.TimestampMillis;
            
            SceneChangedEvents = SceneChangedEvents.OrderBy(item => orderFunc).ToList();
            SimpleTouchEvents = SimpleTouchEvents.OrderBy(item => orderFunc).ToList();
            StringEvents = StringEvents.OrderBy(item => orderFunc).ToList();
            PointerEvents = PointerEvents.OrderBy(item => orderFunc).ToList();
            TextInputEvents = TextInputEvents.OrderBy(item => orderFunc).ToList();
            SliderEvents = SliderEvents.OrderBy(item => orderFunc).ToList();
            DropdownEvents = DropdownEvents.OrderBy(item => orderFunc).ToList();
            ToggleEvents = ToggleEvents.OrderBy(item => orderFunc).ToList();
            TransformEvents = TransformEvents.OrderBy(item => orderFunc).ToList();
        }

        private IEnumerable<T> GetUnique<T>(IEnumerable<T> existingList, IEnumerable<T> newList) where T : class
        {
            var uniqueItems = newList.Where(newEvent => !existingList.Any(tEvent => tEvent.Equals(newEvent)));
            return uniqueItems;
        }

        private void AddUnique<T>(List<T> existingList, IEnumerable<T> newList) where T : class
        {
            var unique = GetUnique(existingList, newList);
            existingList.AddRange(unique);
        }
    }
}