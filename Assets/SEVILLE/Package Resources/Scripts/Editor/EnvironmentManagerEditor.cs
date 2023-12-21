using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Seville
{
    [CustomEditor(typeof(EnvironmentManager))]
    public class EnvironmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);

            EnvironmentManager manager = (EnvironmentManager)target;

            manager.EnvAreaHandlers.RemoveAll(item => item == null);

            if (GUILayout.Button("Add New Area", SevilleStyleEditor.GreenButton))
            {
                manager.AddNewArea();
            }
        }
    }
}