using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class PrefabLoaderWindow : EditorWindow
{
    private Dictionary<string, List<GameObject>> prefabsByFolder = new Dictionary<string, List<GameObject>>();
    private string currentFolderPath;

    [MenuItem("Seville/Asset Prefabs Group")]
    public static void ShowWindowPrefabs()
    {
        var window = EditorWindow.GetWindow<PrefabLoaderWindow>();
        window.LoadPrefabs("Assets/_Sandboxing/Prefabs");
        window.currentFolderPath = "Assets/_Sandboxing/Prefabs";
        window.titleContent = new GUIContent("Smarteye Virtual Learning | Asset Prefabs Group");
    }

    // [MenuItem("Seville/Environment 3D")]
    // public static void ShowWindowPrefabs2()
    // {
    //     var window = EditorWindow.GetWindow<PrefabLoaderWindow>();
    //     window.LoadPrefabs("Assets/SEVILLE/Package Resources/Prefabs/Tesing/3D");
    //     window.currentFolderPath = "Assets/SEVILLE/Package Resources/Prefabs/Tesing/3D";
    //     window.titleContent = new GUIContent("Environment 3D");
    // }

    private void LoadPrefabs(string folderPath)
    {
        var prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
        prefabsByFolder.Clear();

        foreach (var path in prefabPaths)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var directory = Path.GetDirectoryName(path).Replace("\\", "/");
            var folderName = directory.Split('/').Last();

            if (!prefabsByFolder.ContainsKey(folderName))
            {
                prefabsByFolder[folderName] = new List<GameObject>();
            }

            prefabsByFolder[folderName].Add(prefab);
        }
    }

    void OnGUI()
    {
        GUILayout.Label($"Available Prefabs from '{currentFolderPath}'", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (prefabsByFolder.Count > 0)
        {
            foreach (var pair in prefabsByFolder)
            {
                var style = new GUIStyle(EditorStyles.largeLabel) { fontStyle = FontStyle.Bold, fontSize = 15 };
                GUILayout.Label(pair.Key, style);
                GUILayout.Space(10);

                foreach (var prefab in pair.Value)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(prefab.name, GUILayout.Width(200));

                    if (GUILayout.Button("Add to Scene"))
                    {
                        PrefabUtility.InstantiatePrefab(prefab);
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(20);
            }
        }
        else
        {
            GUILayout.Label("No prefabs found.");
        }
    }
}
