using BRM.InteractionRecorder.TmpUi;
using BRM.InteractionRecorder.UnityEditor;
using BRM.InteractionRecorder.UnityUi;
using UnityEditor;
using UnityEngine;

namespace BRM.InteractionRecorder.TmpEditor
{
    public class TmpRecorderWindow : RecorderWindow
    {
        [MenuItem("/" + Constants.PackageDeveloper + "/" + Constants.DisplayedAppName + ": Unity+Tmp Ui")]
        private static void ShowWindow()
        {
            var window = GetWindow<TmpRecorderWindow>("Unity + Tmp Ui Interactions");
            window.minSize = Vector2.one * 300;
            window.Show();
        }

        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new TmpEventServiceFactory().Create());
    }
}