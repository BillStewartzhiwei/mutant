#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bill.Mutant.Core.Editor
{
    public static class LogEditorMenu
    {
        [MenuItem("Mutant/Core/Create Default Log Config")]
        public static void CreateDefaultLogConfig()
        {
            var asset = ScriptableObject.CreateInstance<LogConfig>();

            const string folder = "Assets/MutantGenerated";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                AssetDatabase.CreateFolder("Assets", "MutantGenerated");
            }

            string path = Path.Combine(folder, "LogConfig.asset").Replace("\\", "/");
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }

        [MenuItem("Mutant/Core/Open Log Viewer")]
        public static void OpenLogViewer()
        {
            LogViewerWindow.ShowWindow();
        }
    }
}
#endif
