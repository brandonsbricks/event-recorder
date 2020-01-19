using System;
using System.Collections.Generic;
using System.Linq;
using BRM.DataSerializers.Interfaces;
using UnityEditor;
using UnityEngine;
using BRM.DebugAdapter;
using BRM.EventRecorder.UnityUi;
using BRM.EventRecorder.UnityUi.Models;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.TextSerializers;

namespace BRM.EventRecorder.UnityEditor
{
    public abstract class RecorderWindow : EditorWindow
    {
        #region Variables

        protected EventService _eventServiceLocal;
        private Vector2 _verticalScrollPosition;
        private static GUIStyle _toggleButtonStyleNormal;
        private static GUIStyle _toggleButtonStyleToggled;

        private ISerializeText _serializer;
        private IWriteFiles _fileWriter;

        protected abstract EventService _eventService { get; }

        protected virtual string OutputFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_outputFilePath))
                {
                    string location = $"{Application.dataPath}/{Constants.FileAppName}/Recordings/event_data.json";
                    _outputFilePath = FileUtilities.GetUniqueFilePath(location);
                }

                return _outputFilePath;
            }
            set => _outputFilePath = value;
        }

        private List<Tuple<string, long>> _eventDisplayAndTimes = new List<Tuple<string, long>>();
        private string _outputFilePath;
        
        private bool _showToggles = true;
        private bool _showSlides = true;
        private bool _showDropdowns = true;
        private bool _showTextInputs = true;
        private bool _showIPointers = true;
        private bool _showSimpleTouches = true;
        private bool _showSceneChanges = true;
        private bool _useSearch = false;
        private bool _showSubscribedEvents = true;
        private bool _showNonSubscribedEvents = true;

        private string _searchTerm;
        private bool _wasPlaying;
        private int _lastEventCount;

        private event Action _onUpdate;

        #endregion

        private void OnEnable()
        {
            _serializer = new UnityJsonSerializer();
            _fileWriter = new TextFileSerializer(_serializer, new UnityDebugger());
            _lastEventCount = 0;
        }

        #region Gui Methods

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Constants.DisplayedAppName);
            EditorGUILayout.Space();

            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("Start the editor to begin collecting events.");
                return;
            }

            #region initialize variables only editable in ongui

            var wrapStyle = new GUIStyle(GUI.skin.label) {wordWrap = true, alignment = TextAnchor.MiddleLeft};
            var headerLabel = new GUIStyle(wrapStyle) {fontSize = 14, fontStyle = FontStyle.Bold, stretchWidth = true};
            var prefixStyle = new GUIStyle(headerLabel) {fixedWidth = 30};
            if (_toggleButtonStyleNormal == null)
            {
                _toggleButtonStyleNormal = "Button";
                _toggleButtonStyleNormal.padding.left = _toggleButtonStyleNormal.padding.right = 7;
                _toggleButtonStyleToggled = new GUIStyle(_toggleButtonStyleNormal);
                _toggleButtonStyleToggled.normal.background = _toggleButtonStyleToggled.active.background;
            }

            #endregion

            var eventCollection = _eventService.Payload.GetEventModels();
            if (eventCollection.EventCount != _lastEventCount)
            {
                _eventService.UpdatePayload();
                eventCollection = _eventService.Payload.GetEventModels();
                _lastEventCount = eventCollection.EventCount;
            }
            FilterAndSortEvents(eventCollection, ref _eventDisplayAndTimes);
            
            DisplayWindowHeader(wrapStyle, _eventDisplayAndTimes.Count, eventCollection.EventCount);
            DisplaySearch();

            DisplayEventModelToggles(eventCollection);

            DisplayRefreshButton();
            DisplayClearButton();
            DisplayEventsHeader(prefixStyle, headerLabel);

            DisplayEvents(_eventDisplayAndTimes, prefixStyle, wrapStyle);
        }

        private void DisplayWindowHeader(GUIStyle wrapStyle, int numDisplayedEvents, int numTotalEvents)
        {
            EditorGUILayout.LabelField($"Collecting event data to output file at:\n{OutputFilePath}", wrapStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Displaying {numDisplayedEvents} / {numTotalEvents} events");
            EditorGUILayout.Space();
        }

        private void DisplaySearch()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Use Search", _useSearch ? _toggleButtonStyleToggled : _toggleButtonStyleNormal))
            {
                _useSearch = !_useSearch;
            }

            _searchTerm = _useSearch ? EditorGUILayout.TextField("Search Events: ", _searchTerm) : "";
            GUILayout.EndHorizontal();
        }

        private void DisplayEventModelToggles(EventModelCollection collection)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Show/Hide: ", GUILayout.ExpandWidth(false), GUILayout.Width(100));
            DisplayEventTypeToggle("Simple Touches", ref _showSimpleTouches, collection.SimpleTouchEvents.Count);
            DisplayEventTypeToggle("Text Inputs", ref _showTextInputs, collection.TextInputEvents.Count);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("           ", GUILayout.ExpandWidth(false), GUILayout.Width(100));
            DisplayEventTypeToggle("IPointers", ref _showIPointers, collection.IPointerEvents.Count);
            DisplayEventTypeToggle("Sliders", ref _showDropdowns, collection.SliderEvents.Count);
            DisplayEventTypeToggle("Toggles", ref _showToggles, collection.ToggleEvents.Count);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("           ", GUILayout.ExpandWidth(false), GUILayout.Width(100));
            DisplayEventTypeToggle("Dropdowns", ref _showDropdowns, collection.DropdownEvents.Count);
            DisplayEventTypeToggle("Scene Changes", ref _showSceneChanges, collection.SceneChangedEvents.Count);
            GUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
        }

        private void DisplayEventTypeToggle(string label, ref bool showItems, int numItems)
        {
            string newLabel = $"{label} ({numItems})";
            DisplayEventTypeToggle(newLabel, ref showItems);
        }

        private void DisplayEventTypeToggle(string label, ref bool showItems)
        {
            if (GUILayout.Button(label, showItems ? _toggleButtonStyleToggled : _toggleButtonStyleNormal, GUILayout.ExpandWidth(false)))
            {
                showItems = !showItems;
            }
        }

        private void DisplayRefreshButton()
        {
            if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(true)))
            {
                ResetSubscriptions();
                _fileWriter.Write(_outputFilePath, _eventService.Payload);
                AssetDatabase.Refresh();
            }

            EditorGUILayout.Space();
        }

        private void DisplayClearButton()
        {
            if (GUILayout.Button("Clear", GUILayout.ExpandWidth(true)))
            {
                _eventService.ClearRecording();
                ResetSubscriptions();
                _fileWriter.Write(_outputFilePath, _eventService.Payload);
            }
        }

        private void DisplayEventsHeader(GUIStyle prefixStyle, GUIStyle headerLabel)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("#", prefixStyle, GUILayout.MaxWidth(prefixStyle.fixedWidth));
            GUILayout.Label("Event", headerLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void FilterAndSortEvents(EventModelCollection collection, ref List<Tuple<string, long>> eventDisplayAndTimes)
        {
            eventDisplayAndTimes.Clear();
            if (_showSceneChanges)
            {
                AddEventDisplayText(collection.SceneChangedEvents, ref eventDisplayAndTimes);
            }
            if (_showToggles)
            {
                AddEventDisplayText(collection.ToggleEvents, ref eventDisplayAndTimes);
            }
            if (_showSlides)
            {
                AddEventDisplayText(collection.SliderEvents, ref eventDisplayAndTimes);
            }
            if (_showDropdowns)
            {
                AddEventDisplayText(collection.DropdownEvents, ref eventDisplayAndTimes);
            }
            if (_showTextInputs)
            {
                AddEventDisplayText(collection.TextInputEvents, ref eventDisplayAndTimes);
            }
            if (_showIPointers)
            {
                AddEventDisplayText(collection.IPointerEvents, ref eventDisplayAndTimes);
            }
            if (_showSimpleTouches)
            {
                AddEventDisplayText(collection.SimpleTouchEvents, ref eventDisplayAndTimes);
            }

            eventDisplayAndTimes = eventDisplayAndTimes.OrderBy(displayAndTime => displayAndTime.Item2).ToList();
        }

        private void AddEventDisplayText<TModel>(List<TModel> events, ref List<Tuple<string, long>> eventDisplayAndTimes) where TModel : EventModelBase
        {
            for (int i = 0; i < events.Count; i++)
            {
                var model = events[i];
                if (IsValidToDisplay(model))
                {
                    var displayAndTime = new Tuple<string, long>(_serializer.AsString(model, true), model.TimestampMillis);
                    eventDisplayAndTimes.Add(displayAndTime);
                }
            }
        }

        private bool IsValidToDisplay<TModel>(TModel model) where TModel : EventModelBase
        {
            var modelDisplayText = _serializer.AsString(model, true);
            var isValidSearchItem = !_useSearch || modelDisplayText.ToLowerInvariant().Contains(_searchTerm.ToLowerInvariant());
            var isValidSubscribedEvent = _showSubscribedEvents || !model.IsFromEventSubscription;
            var isValidNonSubscribedEvent = _showNonSubscribedEvents || model.IsFromEventSubscription;
            return isValidSearchItem && isValidSubscribedEvent && isValidNonSubscribedEvent;
        }
        
        private void DisplayEvents(List<Tuple<string, long>> eventDisplayAndTimes, GUIStyle prefixStyle, GUIStyle wrapStyle)
        {
            _verticalScrollPosition = EditorGUILayout.BeginScrollView(_verticalScrollPosition);
            for (int i = 0; i < eventDisplayAndTimes.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label((i + 1).ToString(), prefixStyle, GUILayout.MaxWidth(prefixStyle.fixedWidth));
                GUILayout.Label(eventDisplayAndTimes[i].Item1, wrapStyle);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region File Writing

        private void Update()
        {
            if (!_wasPlaying && Application.isPlaying)
            {
                ResetSubscriptions();
            }

            if (!Application.isPlaying && _wasPlaying)
            {
                OnEditorStop();
            }

            if (!Application.isPlaying)
            {
                _wasPlaying = false;
                return;
            }

            _onUpdate?.Invoke();

            if (Input.GetMouseButtonDown(MouseButton.Left) || Input.GetMouseButtonUp(MouseButton.Left))
            {
                OnClick();
            }

            _wasPlaying = true;
        }

        private void OnClick()
        {
            ResetSubscriptions();
            _fileWriter.Write(_outputFilePath, _eventService.Payload);

            Repaint();
        }

        private void ResetSubscriptions()
        {
            var updators = _eventService.GetUpdaters();
            updators.ForEach(updator =>
            {
                _onUpdate -= updator.OnUpdate;
                _onUpdate += updator.OnUpdate;
            });
            _eventService.ResetSubscriptions();
            _eventService.UpdatePayload();
        }

        private void OnEditorStop()
        {
            _outputFilePath = null;
            AssetDatabase.Refresh();
        }

        #endregion
    }
}