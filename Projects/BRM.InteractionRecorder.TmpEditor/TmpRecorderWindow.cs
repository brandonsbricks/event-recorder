using BRM.InteractionRecorder.TmpUi;
using BRM.InteractionRecorder.UnityEditor;
using BRM.InteractionRecorder.UnityUi;
using UnityEditor;

namespace BRM.InteractionRecorder.TmpEditor
{
    public class TmpRecorderWindow : RecorderWindow<TmpEventServiceFactory>
    {
        [MenuItem("/" + Constants.PackageName + "/" + Constants.DisplayedAppName + ": Unity+Tmp Ui")]
        private static void ShowWindow()
        {
            var window = GetWindow<TmpRecorderWindow>($"{Constants.DisplayedAppName}");
            window.Show();
        }
    }
}