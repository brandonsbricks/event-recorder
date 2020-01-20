using BRM.EventRecorder.TmpEvents;
using BRM.EventRecorder.UnityEditor;
using BRM.EventRecorder.UnityEvents;
using UnityEditor;
using UnityEngine;

namespace BRM.EventRecorder.TmpEditor
{
    public class TmpRecorderWindow : RecorderWindow
    {
        [MenuItem("/" + Constants.PackageDeveloper + "/" + Constants.DisplayedAppName + ": Unity+Tmp")]
        private static void ShowWindow()
        {
            var window = GetWindow<TmpRecorderWindow>("Unity + Tmp Events");
            window.minSize = Vector2.one * 300;
            window.Show();
        }

        protected override RecordingService _recordingService => _recordingServiceLocal ?? (_recordingServiceLocal = new TmpRecordingServiceFactory().Create());
    }
}