using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace BRM.EventRecorder.UnityUi.Models
{
    [Serializable]
    public abstract class EventModelBase : IEquatable<EventModelBase>
    {
        public string EventType;
        public long TimestampMillis;
        public bool IsFromEventSubscription;

        protected EventModelBase(string eventTypeName)
        {
            EventType = eventTypeName;
            TimestampMillis = UnixTime;
        }

        public bool Equals(EventModelBase other)
        {
            if (other == null) return false;

            return
                TimestampMillis == other.TimestampMillis &&
                EventType == other.EventType;
        }

        protected static long UnixTime => (long) DateTime.UtcNow.Subtract(UnixStartTime).TotalMilliseconds;
        private static DateTime UnixStartTime => new DateTime(1970, 1, 1); 
        public string GetDisplayTime()
        {
            var dateTime = UnixStartTime.AddMilliseconds(TimestampMillis).ToUniversalTime();
            return $"{dateTime.ToShortDateString()}, {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}:{dateTime.Millisecond}";
        }
    }

    [Serializable]
    public abstract class ComponentEventModel : EventModelBase, IEquatable<ComponentEventModel>
    {
        public string ComponentType;
        public string GameObjectName;

        protected ComponentEventModel(string eventTypeName) : base(eventTypeName)
        {
        }

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

        public SceneChangedEvent() : base(nameof(SceneChangedEvent))
        {
        }
    }

    
    [Serializable]
    public class SimpleTouchEvent : EventModelBase
    {
        public SimpleTouchEvent() : base(nameof(SimpleTouchEvent))
        {
        }

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
    public class ComponentEvent : ComponentEventModel
    {
        public Vector3 TouchPoint
        {
            get => new Vector3(TouchPointPixels.x, TouchPointPixels.y);
            set => TouchPointPixels = new Vector2Int((int) value.x, (int) value.y);
        }
        
        [SerializeField] private Vector2Int TouchPointPixels;

        public ComponentEvent(string eventTypeName) : base(eventTypeName)
        {
        }

        public const string IPointerUpEvent = nameof(IPointerUpEvent);
        public const string IPointerDownEvent = nameof(IPointerDownEvent);
        public const string IPointerClickEvent = nameof(IPointerClickEvent);
        public const string IPointerEnterEvent = nameof(IPointerEnterEvent);
        public const string IPointerExitEvent = nameof(IPointerExitEvent);
    }
    
    [Serializable]
    public class SliderEvent : ComponentEvent
    {
        public SliderEvent() : base(nameof(SliderEvent))
        {
            IsFromEventSubscription = true;
        }

        public string PropertyName;
        public float NewValue;
    }
    
    [Serializable]
    public class DropdownEvent : ComponentEvent
    {
        public DropdownEvent(string eventTypeName) : base(eventTypeName)
        {
            IsFromEventSubscription = true;
        }

        public string PropertyName;
        public int NewIntValue;
        public string NewStringValue;
        
        public const string UnityDropdownEvent = nameof(UnityDropdownEvent);
    }

    [Serializable]
    public class ToggleEvent : ComponentEvent
    {
        public ToggleEvent() : base(nameof(ToggleEvent))
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
        public const string UnityTextInputEvent = nameof(UnityTextInputEvent);
        
        public TextInputEvent(string eventTypeName) : base(eventTypeName)
        {
            IsFromEventSubscription = true;
        }
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
        public List<ComponentEvent> IPointerEvents = new List<ComponentEvent>();
        public List<TextInputEvent> TextInputEvents = new List<TextInputEvent>();
        public List<SliderEvent> SliderEvents = new List<SliderEvent>();
        public List<DropdownEvent> DropdownEvents = new List<DropdownEvent>();
        public List<ToggleEvent> ToggleEvents = new List<ToggleEvent>();

        public List<EventModelBase> GetAllEvents()
        {
            var allEvents = new List<EventModelBase>();
            allEvents.AddRange(SceneChangedEvents);
            allEvents.AddRange(SimpleTouchEvents);
            allEvents.AddRange(IPointerEvents);
            allEvents.AddRange(TextInputEvents);
            allEvents.AddRange(SliderEvents);
            allEvents.AddRange(DropdownEvents);
            allEvents.AddRange(ToggleEvents);
            return allEvents;
        }

        public int EventCount => SceneChangedEvents.Count + SimpleTouchEvents.Count + IPointerEvents.Count + TextInputEvents.Count + SliderEvents.Count + DropdownEvents.Count + ToggleEvents.Count;
        
        /// <summary>
        /// Removes any events already in the collection
        /// </summary>
        public void AppendNewEventsUniquely(EventModelCollection newCollection)
        {
            AddUnique(SceneChangedEvents, newCollection.SceneChangedEvents);
            AddUnique(SimpleTouchEvents, newCollection.SimpleTouchEvents);
            AddUnique(IPointerEvents, newCollection.IPointerEvents);
            AddUnique(TextInputEvents, newCollection.TextInputEvents);
            AddUnique(SliderEvents, newCollection.SliderEvents);
            AddUnique(DropdownEvents, newCollection.DropdownEvents);
            AddUnique(ToggleEvents, newCollection.ToggleEvents);
        }
        
        public void SortByTimestamp()
        {
            Func<EventModelBase, long> orderFunc = item => item.TimestampMillis;
            
            SceneChangedEvents = SceneChangedEvents.OrderBy(item => orderFunc).ToList();
            SimpleTouchEvents = SimpleTouchEvents.OrderBy(item => orderFunc).ToList();
            IPointerEvents = IPointerEvents.OrderBy(item => orderFunc).ToList();
            TextInputEvents = TextInputEvents.OrderBy(item => orderFunc).ToList();
            SliderEvents = SliderEvents.OrderBy(item => orderFunc).ToList();
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

        public void ClearRecording()
        {
            EventModels = new EventModelCollection();
        }
    }
}