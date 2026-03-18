using UnityEditor;
using UnityEngine;

namespace YourCompany.YourPackage.Editor
{
    /// <summary>
    /// Custom Inspector for YourComponent.
    /// </summary>
    [CustomEditor(typeof(YourComponent))]
    public class YourComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Do Something"))
            {
                ((YourComponent)target).DoSomething();
            }
        }
    }
}
