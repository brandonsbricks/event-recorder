using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityUi.Models
{
    [Serializable]
    public abstract class EventModelBase : IEquatable<EventModelBase>
    {
        public string EventType;
        public long TimestampMillis;
        public bool IsFromEventSubscription;

        protected abstract string _eventTypeName { get; }

        protected EventModelBase()
        {
            EventType = _eventTypeName;
            TimestampMillis = UnixTime;
        }

        public bool Equals(EventModelBase other)
        {
            if (other == null) return false;

            return
                TimestampMillis == other.TimestampMillis &&
                EventType == other.EventType;
        }

        protected static long UnixTime => (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    [Serializable]
    public abstract class ComponentEventModel : EventModelBase, IEquatable<ComponentEventModel>
    {
        public string ComponentType;
        public string GameObjectName;
        
        public bool Equals(ComponentEventModel other)
        {
            if (other == null) return false;

            return Equals(other as EventModelBase) &&
                   ComponentType == other.ComponentType &&
                   GameObjectName == other.GameObjectName;
        }
    }

    [Serializable]
    public class SceneChangedEvent : EventModelBase
    {
        public string OldSceneName;
        public string NewSceneName;
        protected override string _eventTypeName => nameof(SceneChangedEvent);
    }

    
    [Serializable]
    public class SimpleTouchEvent : EventModelBase
    {
        protected override string _eventTypeName => nameof(SimpleTouchEvent);
        
        public Vector3 TouchPointProp
        {
            get => new Vector3(TouchPointPixels.x, TouchPointPixels.y);
            set => TouchPointPixels = new Vector2Int((int) value.x, (int) value.y);
        }

        [SerializeField] private Vector2Int TouchPointPixels;
        
        public const string TouchDown = "TouchDownEvent";
        public const string TouchUp = "TouchUpEvent";
    }

    [Serializable]
    public abstract class ComponentTouchEvent : ComponentEventModel
    {
        public Vector3 TouchPoint
        {
            get => new Vector3(TouchPointPixels.x, TouchPointPixels.y);
            set => TouchPointPixels = new Vector2Int((int) value.x, (int) value.y);
        }

        [SerializeField] private Vector2Int TouchPointPixels;

    }

    [Serializable]
    public class ButtonEvent : ComponentTouchEvent
    {
        protected override string _eventTypeName => nameof(ButtonEvent);
        public ButtonEvent() : base()
        {
            IsFromEventSubscription = true;
        }
    }
    
    [Serializable]
    public class EventTriggerEvent : ComponentTouchEvent
    {
        protected override string _eventTypeName => EventTriggerUnknownEvent;

        public EventTriggerEvent(string eventType) : base()
        {
            EventType = eventType;
            IsFromEventSubscription = true;
        }
        //event trigger touch events
        public const string EventTriggerDownEvent = nameof(EventTriggerDownEvent);
        public const string EventTriggerUpEvent = nameof(EventTriggerUpEvent);
        public const string EventTriggerClickEvent = nameof(EventTriggerClickEvent);
        public const string EventTriggerUnknownEvent = nameof(EventTriggerUnknownEvent);
    }
    
    [Serializable]
    public class DropdownEvent : ComponentTouchEvent
    {
        protected override string _eventTypeName => nameof(DropdownEvent);
        public DropdownEvent() : base()
        {
            IsFromEventSubscription = true;
        }

        public string PropertyName;
        public int NewIntValue;
        public string NewStringValue;

        public const string UnityDropdownEvent = nameof(UnityDropdownEvent);
        public const string TmpDropdownEvent = nameof(TmpDropdownEvent);
    }

    [Serializable]
    public class ToggleEvent : ComponentTouchEvent
    {
        protected override string _eventTypeName => nameof(ToggleEvent);
        public ToggleEvent() : base()
        {
            IsFromEventSubscription = true;
        }

        public string PropertyName;
        public bool NewValue;
    }

    [Serializable]
    public class TextInputEvent : ComponentEventModel
    {
        public string PropertyName;
        public string NewValue;
        protected override string _eventTypeName => nameof(TextInputEvent);
        
        public TextInputEvent() : base()
        {
            IsFromEventSubscription = true;
        }

        public const string UnityTextInputEvent = nameof(UnityTextInputEvent);
        public const string TmpTextInputEvent = nameof(TmpTextInputEvent);
    }

    [Serializable]
    public class AppData
    {
        public string AppVersion;
        public string UnityVersion;

        public string GitSha = "";
        public string Server = "unassigned";

        public void SetProperties()
        {
            AppVersion = Application.version;
            UnityVersion = Application.unityVersion;
        }

        public void SetValues(string gitSha, string server = "unassigned")
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

    [Serializable]
    public class EventModelCollection
    {
        public List<SceneChangedEvent> SceneChangedEvents = new List<SceneChangedEvent>();
        public List<SimpleTouchEvent> SimpleTouchEvents = new List<SimpleTouchEvent>();
        public List<ComponentTouchEvent> ComponentTouchEvents = new List<ComponentTouchEvent>();
        public List<TextInputEvent> TextInputEvents = new List<TextInputEvent>();
        public List<DropdownEvent> DropdownEvents = new List<DropdownEvent>();
        public List<ToggleEvent> ToggleEvents = new List<ToggleEvent>();

        public List<EventModelBase> GetAllEvents()
        {
            var allEvents = new List<EventModelBase>();
            allEvents.AddRange(SceneChangedEvents);
            allEvents.AddRange(SimpleTouchEvents);
            allEvents.AddRange(ComponentTouchEvents);
            allEvents.AddRange(TextInputEvents);
            allEvents.AddRange(DropdownEvents);
            allEvents.AddRange(ToggleEvents);
            return allEvents;
        }

        public int EventCount => SceneChangedEvents.Count + SimpleTouchEvents.Count + ComponentTouchEvents.Count + TextInputEvents.Count + DropdownEvents.Count + ToggleEvents.Count;
        
        /// <summary>
        /// Removes any events already in the collection
        /// </summary>
        public void AppendNewEventsUniquely(EventModelCollection newCollection)
        {
            AddUnique(SceneChangedEvents, newCollection.SceneChangedEvents);
            AddUnique(SimpleTouchEvents, newCollection.SimpleTouchEvents);
            AddUnique(ComponentTouchEvents, newCollection.ComponentTouchEvents);
            AddUnique(TextInputEvents, newCollection.TextInputEvents);
            AddUnique(DropdownEvents, newCollection.DropdownEvents);
            AddUnique(ToggleEvents, newCollection.ToggleEvents);
        }
        
        public void SortByTimestamp()
        {
            Func<EventModelBase, long> orderFunc = item => item.TimestampMillis;
            
            SceneChangedEvents = SceneChangedEvents.OrderBy(item => orderFunc).ToList();
            SimpleTouchEvents = SimpleTouchEvents.OrderBy(item => orderFunc).ToList();
            ComponentTouchEvents = ComponentTouchEvents.OrderBy(item => orderFunc).ToList();
            TextInputEvents = TextInputEvents.OrderBy(item => orderFunc).ToList();
            DropdownEvents = DropdownEvents.OrderBy(item => orderFunc).ToList();
            ToggleEvents = ToggleEvents.OrderBy(item => orderFunc).ToList();
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

    [Serializable]
    public class EventAndAppPayload
    {
        [SerializeField] private AppData AppData = new AppData();
        [SerializeField] private DeviceData DeviceData = new DeviceData();
        [SerializeField] private EventModelCollection EventModels = new EventModelCollection();

        public int EventCount => EventModels.EventCount;

        public void UpdateProperties()
        {
            AppData.SetProperties();
            DeviceData.SetProperties();
        }

        public void AppendNewEventsUniquely(EventModelCollection newEvents)
        {
            EventModels.AppendNewEventsUniquely(newEvents);
        }

        public void SetServer(string server)
        {
            AppData.Server = server;
        }

        public EventModelCollection GetEventModels()
        {
            EventModels.SortByTimestamp();
            return EventModels;
        }
    }
}