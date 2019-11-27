using BRM.InteractionRecorder.UnityUi;
using UnityEditor;
using UnityEngine;

namespace BRM.InteractionRecorder.UnityEditor
{
    public class UnityUiRecorderWindow : RecorderWindow
    {
        [MenuItem("/" + Constants.PackageDeveloper + "/" + Constants.DisplayedAppName + ": Unity Ui")]
        private static void ShowWindow()
        {
            var window = GetWindow<UnityUiRecorderWindow>("Unity Ui Interactions");
            window.minSize = Vector2.one * 300;
            window.Show();
        }
        
        protected override EventService _eventService => _eventServiceLocal ?? (_eventServiceLocal = new UnityEventServiceFactory().Create());
    }
}