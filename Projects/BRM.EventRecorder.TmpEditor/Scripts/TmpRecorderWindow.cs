using BRM.EventRecorder.TmpUi;
using BRM.EventRecorder.UnityEditor;
using BRM.EventRecorder.UnityUi;
using UnityEditor;
using UnityEngine;

namespace BRM.EventRecorder.TmpEditor
{
    public class TmpRecorderWindow : RecorderWindow
    {
        [MenuItem("/" + Constants.PackageDeveloper + "/" + Constants.DisplayedAppName + ": Unity+Tmp Ui")]
        private static void ShowWindow()
        {
            var window = GetWindow<TmpRecorderWindow>("Unity + Tmp Ui Events");
            window.minSize = Vector2.one * 300;
            window.Show();
        }

        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new TmpEventServiceFactory().Create());
    }
}