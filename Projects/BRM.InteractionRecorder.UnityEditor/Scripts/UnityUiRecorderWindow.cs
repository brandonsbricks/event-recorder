using BRM.InteractionRecorder.UnityUi;
using UnityEditor;

namespace BRM.InteractionRecorder.UnityEditor
{
    public class UnityUiRecorderWindow : RecorderWindow<UnityEventServiceFactory>
    {
        [MenuItem("/" + Constants.PackageName + "/" + Constants.DisplayedAppName + ": Unity Ui")]
        private static void ShowWindow()
        {
            var window = GetWindow<UnityUiRecorderWindow>($"{Constants.DisplayedAppName}");
            window.Show();
        }
    }
}