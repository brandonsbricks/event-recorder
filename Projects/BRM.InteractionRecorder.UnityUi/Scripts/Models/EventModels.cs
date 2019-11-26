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
        public string ComponentType;
        public string GameObjectName;
        public string SceneName;
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
                EventType == other.EventType &&
                ComponentType == other.ComponentType &&
                GameObjectName == other.GameObjectName &&
                SceneName == other.SceneName;
        }

        protected static long UnixTime => (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    [Serializable]
    public class TouchEvent : EventModelBase
    {
        protected override string _eventTypeName => nameof(TouchEvent);
        public TouchEvent()
        {
            TimestampMillis = UnixTime;
        }

        public TouchEvent(EventModelBase model)
        {
            SceneName = model.SceneName;
            GameObjectName = model.GameObjectName;
            ComponentType = model.ComponentType;
            TimestampMillis = UnixTime;
        }

        public Vector3S TouchPointProp
        {
            get => TouchPointPixels;
            set => TouchPointPixels = value;
        }

        [SerializeField] private Vector3S TouchPointPixels;

        //generic touch events
        public const string ButtonClick = "ButtonClickEvent";
        public const string Tap = "TapEvent";
        public const string TouchDown = "TouchDownEvent";
        public const string TouchUp = "TouchUpEvent";
        public const string EventTriggerDown = "EventTriggerDown";
        public const string EventTriggerUp = "EventTriggerUp";
        public const string EventTriggerTap = "EventTriggerTap";
    }

    [Serializable]
    public class DropdownEvent : TouchEvent
    {
        protected override string _eventTypeName => nameof(DropdownEvent);
        public DropdownEvent() : base()
        {
            IsFromEventSubscription = true;
        }

        public string PropertyName;
        public int NewIntValue;
        public string NewStringValue;
    }

    [Serializable]
    public class ToggleEvent : TouchEvent
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
    public class TextInputEvent : EventModelBase
    {
        public string PropertyName;
        public string NewValue;
        protected override string _eventTypeName => nameof(TextInputEvent);
        
        public TextInputEvent() : base()
        {
            IsFromEventSubscription = true;
        }
    }

    [Serializable]
    public class AppData
    {
        public string AppVersion;
        public string UnityVersion;
        
        public string Server = "unassigned";

        public void SetProperties()
        {
            AppVersion = Application.version;
            UnityVersion = Application.unityVersion;
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
        public Vector3S ScreenSizePixels;
        
        public void SetProperties()
        {
            DeviceModel = SystemInfo.deviceModel;
            DeviceType = SystemInfo.deviceType.ToString();
            GraphicsDeviceName = SystemInfo.graphicsDeviceName;
            GraphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
            MemorySizeMB = SystemInfo.systemMemorySize;
            OperatingSystemFamily = SystemInfo.operatingSystemFamily.ToString();
            ScreenSizePixels = new Vector3S(Screen.width, Screen.height, 0);
        }
    }

    [Serializable]
    public class EventModelCollection
    {
        public List<TouchEvent> TouchEvents = new List<TouchEvent>();
        public List<TextInputEvent> TextInputEvents = new List<TextInputEvent>();
        public List<DropdownEvent> DropdownEvents = new List<DropdownEvent>();
        public List<ToggleEvent> ToggleEvents = new List<ToggleEvent>();

        /// <summary>
        /// Removes any events already in the collection
        /// </summary>
        public void AppendNewEventsUniquely(EventModelCollection newCollection)
        {
            var uniqueTouches = newCollection.TouchEvents.Where(newEvent => !TouchEvents.Any(tEvent => tEvent.Equals(newEvent)));
            TouchEvents.AddRange(uniqueTouches);

            var uniqueTextInputs = newCollection.TextInputEvents.Where(newEvent => !TextInputEvents.Any(tEvent => tEvent.Equals(newEvent)));
            TextInputEvents.AddRange(uniqueTextInputs);

            var uniqueDropdowns = newCollection.DropdownEvents.Where(newEvent => !DropdownEvents.Any(tEvent => tEvent.Equals(newEvent)));
            DropdownEvents.AddRange(uniqueDropdowns);

            var uniqueToggleEvents = newCollection.ToggleEvents.Where(newEvent => !ToggleEvents.Any(tEvent => tEvent.Equals(newEvent)));
            ToggleEvents.AddRange(uniqueToggleEvents);
        }

        public void SortByTimestamp()
        {
            Func<EventModelBase, long> orderFunc = item => item.TimestampMillis;
            TouchEvents = TouchEvents.OrderBy(item => orderFunc).ToList();
            TextInputEvents = TextInputEvents.OrderBy(item => orderFunc).ToList();
            DropdownEvents = DropdownEvents.OrderBy(item => orderFunc).ToList();
            ToggleEvents = ToggleEvents.OrderBy(item => orderFunc).ToList();
        }

        public int EventCount => TouchEvents.Count + TextInputEvents.Count + DropdownEvents.Count + ToggleEvents.Count;
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