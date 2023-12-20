using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class Taggler
{
    static List<string> tagsList = new List<string>();
    static List<string> layersList = new List<string>();


    [MenuItem("Tools/Taggler/Backup Tags")]
    static void writeTagsBackup()
    {
        //check if folders exist
        string path = "Assets/_Sandboxing/";

        if (!File.Exists(path)) Directory.CreateDirectory(path);
        path += "Taggler/";

        if (!File.Exists(path)) Directory.CreateDirectory(path);
        path += "Tags Backup.txt";

        File.WriteAllText(path, String.Empty);

        StreamWriter writer = new StreamWriter(path, true);

        //cache, loop and write the tags into stream
        int max = UnityEditorInternal.InternalEditorUtility.tags.Length;
        for (int i = 0; i < max; i++)
        {
            writer.WriteLine(UnityEditorInternal.InternalEditorUtility.tags[i]);
        }

        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("Tags Backup");

        Debug.Log("Backup completed!");
        Debug.Log("You can find it inside Assets/_Sandboxing/Taggler/Tags Backup.txt");
    }


    [MenuItem("Tools/Taggler/Backup Layers")]
    static void writeLayersBackup()
    {
        //check if folders exist
        string path = "Assets/_Sandboxing/";

        if (!File.Exists(path)) Directory.CreateDirectory(path);
        path += "Taggler/";

        if (!File.Exists(path)) Directory.CreateDirectory(path);
        path += "Layers Backup.txt";

        File.WriteAllText(path, String.Empty);

        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i <= 31; i++)
        {
            var layerN = LayerMask.LayerToName(i);
            if (layerN.Length > 0) writer.WriteLine(layerN);
        }

        writer.Close();

        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("Layers Backup");

        Debug.Log("Backup completed!");
        Debug.Log("You can find it inside Assets/_Sandboxing/Taggler/Layers Backup.txt");
    }


    [MenuItem("Tools/Taggler/Import Tags")]
    static void importTags()
    {
        string path = chooseTagsBackupFilePath();

        using (StreamReader reader = new StreamReader(path))
        {
            while (reader.Peek() >= 0)
            {
                tagsList.Add(reader.ReadLine());
            }
        }

        addTags(tagsList);
    }


    [MenuItem("Tools/Taggler/Import Layers")]
    static void importLayers()
    {
        string path = chooseLayersBackupFilePath();

        using (StreamReader reader = new StreamReader(path))
        {
            while (reader.Peek() >= 0)
            {
                layersList.Add(reader.ReadLine());
            }
        }

        addLayers(layersList);
    }


    static void addTags(List<string> tags)
    {
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Adding a Tag
        foreach (var item in tags)
        {
            // First check if it is not already present
            bool found = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(item)) { found = true; break; }
            }

            // if not found, add it
            if (!found)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = item;
            }
        }

        tagManager.ApplyModifiedProperties();
        Debug.Log("Import completed!");
    }


    static void addLayers(List<string> layers)
    {
        SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = manager.FindProperty("layers");

        int index = 0;
        foreach (var item in layers)
        {
            bool found = false;
            for (int i = 0; i < layersProp.arraySize; i++)
            {
                SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
                if (sp.stringValue.Equals(item)) { found = true; index = i; break; }
            }

            if (!found)
            {
                layersProp.InsertArrayElementAtIndex(index);
                SerializedProperty n = layersProp.GetArrayElementAtIndex(index);
                n.stringValue = item;
            }
        }

        manager.ApplyModifiedProperties();
        Debug.Log("Import completed!");
    }


    static string chooseTagsBackupFilePath()
    {
        string path = EditorUtility.OpenFilePanel("Tags Backup.txt File", "Assets/_Sandboxing/", "txt");
        return path;
    }


    static string chooseLayersBackupFilePath()
    {
        string path = EditorUtility.OpenFilePanel("Layers Backup.txt File", "Assets/_Sandboxing/", "txt");
        return path;
    }
}