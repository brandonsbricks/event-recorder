using System;
using System.Collections.Generic;
using BRM.DataSerializers.Interfaces;
using UnityEditor;
using UnityEngine;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.InteractionRecorder.UnityUi;
using BRM.InteractionRecorder.UnityUi.Models;
using BRM.TextSerializers;

namespace BRM.InteractionRecorder.UnityEditor
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

        protected virtual string JsonFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(_jsonFilePath))
                {
                    string location = $"{Application.dataPath}/{Constants.FileAppName}/JSON/interaction_data.json";
                    _jsonFilePath = FileUtilities.GetUniqueFilePath(location);
                }

                return _jsonFilePath;
            }
            set => _jsonFilePath = value;
        }

        private string _jsonFilePath;
        private bool _showToggles = true;
        private bool _showDropdowns = true;
        private bool _showTextInputs = true;
        private bool _showComponentTouches = true;
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
                EditorGUILayout.LabelField("Start the editor to begin collecting interactions.");
                return;
            }

            #region initialize variables only editable in ongui

            var wrapStyle = new GUIStyle(GUI.skin.label) {wordWrap = true, alignment = TextAnchor.MiddleLeft};
            var headerLabel = new GUIStyle(wrapStyle) {fontSize = 14, fontStyle = FontStyle.Bold, stretchWidth = true};
            var prefixStyle = new GUIStyle(headerLabel) {fixedWidth = 30};
            if (_toggleButtonStyleNormal == null)
            {
                _toggleButtonStyleNormal = "Button";
                _toggleButtonStyleToggled = new GUIStyle(_toggleButtonStyleNormal);
                _toggleButtonStyleToggled.normal.background = _toggleButtonStyleToggled.active.background;
            }

            #endregion

            DisplayWindowHeader(wrapStyle);
            DisplaySearch();
            DisplaySubscriberToggles();

            var eventCollection = _eventService.Payload.GetEventModels();
            if (eventCollection.EventCount != _lastEventCount)
            {
                _eventService.UpdatePayload();
                eventCollection = _eventService.Payload.GetEventModels();
                _lastEventCount = eventCollection.EventCount;
            }

            DisplayEventModelToggles(eventCollection);

            DisplayRefreshButton();
            DisplayClearButton();
            DisplayEventsHeader(prefixStyle, headerLabel);


            DisplayEvents(prefixStyle, wrapStyle, eventCollection);
        }

        private void DisplayWindowHeader(GUIStyle wrapStyle)
        {
            EditorGUILayout.LabelField($"Collecting interaction data to json file at:\n{JsonFilePath}", wrapStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tap, toggle, input text, and interact with UI elements to track these interactions.");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Unique interactions collected: {_eventService.EventCount}");
            EditorGUILayout.Space();
        }

        private void DisplaySubscriberToggles()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Show/Hide: ");
            DisplayToggleButton("Subscribed Events", ref _showSubscribedEvents);
            DisplayToggleButton("Non-Subscribed Events", ref _showNonSubscribedEvents);
            GUILayout.EndHorizontal();
        }

        private void DisplaySearch()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Use Search", _useSearch ? _toggleButtonStyleToggled : _toggleButtonStyleNormal))
            {
                _useSearch = !_useSearch;
            }

            _searchTerm = _useSearch ? EditorGUILayout.TextField("Search JSON: ", _searchTerm) : "";
            GUILayout.EndHorizontal();
        }

        private void DisplayEventModelToggles(EventModelCollection collection)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Show/Hide: ");
            DisplayToggleButton("Simple Touches", ref _showSimpleTouches, collection.SimpleTouchEvents.Count);
            DisplayToggleButton("Comp. Touches", ref _showComponentTouches, collection.ComponentTouchEvents.Count);
            DisplayToggleButton("Text Inputs", ref _showTextInputs, collection.TextInputEvents.Count);
            DisplayToggleButton("Dropdowns", ref _showDropdowns, collection.DropdownEvents.Count);
            DisplayToggleButton("Toggles", ref _showToggles, collection.ToggleEvents.Count);
            DisplayToggleButton("Scene Changes", ref _showSceneChanges, collection.SceneChangedEvents.Count);
            GUILayout.EndHorizontal();
        }

        private void DisplayToggleButton(string label, ref bool showItems, int numItems)
        {
            string newLabel = $"{label} ({numItems})";
            DisplayToggleButton(newLabel, ref showItems);
        }

        private void DisplayToggleButton(string label, ref bool showItems)
        {
            if (GUILayout.Button(label, showItems ? _toggleButtonStyleToggled : _toggleButtonStyleNormal))
            {
                showItems = !showItems;
            }
        }

        private void DisplayRefreshButton()
        {
            if (GUILayout.Button("Refresh"))
            {
                ResetSubscriptions();
                _fileWriter.Write(_jsonFilePath, _eventService.Payload);
                AssetDatabase.Refresh();
            }

            EditorGUILayout.Space();
        }

        private void DisplayClearButton()
        {
            if (GUILayout.Button("Clear Recording"))
            {
                _eventService.ClearRecording();
                ResetSubscriptions();
                _fileWriter.Write(_jsonFilePath, _eventService.Payload);
            }
        }

        private void DisplayEventsHeader(GUIStyle prefixStyle, GUIStyle headerLabel)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("#", prefixStyle, GUILayout.MaxWidth(prefixStyle.fixedWidth));
            GUILayout.Label("Interaction Data", headerLabel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DisplayEvents(GUIStyle prefixStyle, GUIStyle wrapStyle, EventModelCollection collection) //todo: flatten events to single array and display chronologically
        {
            _verticalScrollPosition = EditorGUILayout.BeginScrollView(_verticalScrollPosition);
            int totalIndex = 0;

            if (_showSceneChanges)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.SceneChangedEvents);
            }

            if (_showToggles)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.ToggleEvents);
            }

            if (_showDropdowns)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.DropdownEvents);
            }

            if (_showTextInputs)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.TextInputEvents);
            }

            if (_showComponentTouches)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.ComponentTouchEvents);
            }

            if (_showSimpleTouches)
            {
                totalIndex = DisplayCategory(totalIndex, prefixStyle, wrapStyle, collection.SimpleTouchEvents);
            }

            EditorGUILayout.EndScrollView();
        }

        private int DisplayCategory<TModel>(int totalIndex, GUIStyle prefixStyle, GUIStyle guiStyle, List<TModel> events) where TModel : EventModelBase
        {
            for (int i = 0; i < events.Count; i++)
            {
                var item = events[i];
                if (DisplayItem(totalIndex, item, prefixStyle, guiStyle))
                {
                    totalIndex++;
                }
            }

            return totalIndex;
        }

        private bool DisplayItem<TModel>(int totalIndex, TModel item, GUIStyle prefixStyle, GUIStyle guiStyle) where TModel : EventModelBase
        {
            var fieldJson = _serializer.AsString(item, true);
            if (_useSearch && !fieldJson.ToLowerInvariant().Contains(_searchTerm.ToLowerInvariant()) || (!_showSubscribedEvents && item.IsFromEventSubscription) ||
                (!_showNonSubscribedEvents && !item.IsFromEventSubscription))
            {
                return false;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label((totalIndex + 1).ToString(), prefixStyle, GUILayout.MaxWidth(prefixStyle.fixedWidth));
            GUILayout.Label(fieldJson, guiStyle);
            EditorGUILayout.EndHorizontal();
            return true;
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
            _fileWriter.Write(_jsonFilePath, _eventService.Payload);

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
            _jsonFilePath = null;
            AssetDatabase.Refresh();
        }

        #endregion
    }
}