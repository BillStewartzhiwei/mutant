#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Bill.Mutant.Core.Editor
{
    [CustomEditor(typeof(LogConfig))]
    public class LogConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Utilities", EditorStyles.boldLabel);

            if (GUILayout.Button("Open Log Viewer"))
            {
                LogViewerWindow.ShowWindow();
            }

            if (GUILayout.Button("Clear Memory Logs"))
            {
                Logger.ClearMemory();
            }

            if (GUILayout.Button("Apply As Runtime Config (Play Mode)"))
            {
                var config = target as LogConfig;
                if (config != null)
                {
                    Logger.Shutdown();
                    Logger.Init(config);
                    Debug.Log("[Mutant] Applied LogConfig to Logger.");
                }
            }

            EditorGUILayout.EndVertical();

            DrawValidationHelp();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawValidationHelp()
        {
            var config = target as LogConfig;
            if (config == null) return;

            EditorGUILayout.Space(8);

            if (!config.EnableConsole && !config.EnableFile && !config.EnableMemory)
            {
                EditorGUILayout.HelpBox("No output target is enabled. Logs will be discarded.", MessageType.Warning);
            }

            if (config.EnableMemory && config.MaxMemoryEntries < 10)
            {
                EditorGUILayout.HelpBox("MaxMemoryEntries is too small. Recommend at least 100.", MessageType.Warning);
            }

            if (config.EnableFile && string.IsNullOrWhiteSpace(config.FileName))
            {
                EditorGUILayout.HelpBox("File output is enabled, but FileName is empty.", MessageType.Error);
            }
        }
    }
}
#endif
