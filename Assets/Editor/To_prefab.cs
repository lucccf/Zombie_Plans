using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class To_prefab : EditorWindow
{
    [MenuItem("Tools/To_prefab")]
    static void Init()
    {
        To_prefab window = (To_prefab)EditorWindow.GetWindow(typeof(To_prefab));
        window.Show();
    }

    string prefabName = "NewPrefab";
    string destinationFolder = "Assets/Prefabs/";

    void OnGUI()
    {
        GUILayout.Label("Create New Prefab", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.Label("Texture Floder");
        prefabName = EditorGUILayout.TextField(prefabName);

        GUILayout.Label("Destination Folder");
        destinationFolder = EditorGUILayout.TextField(destinationFolder);

        if (GUILayout.Button("Work"))
        {
            GameObject newObject = new GameObject(prefabName);
            newObject.AddComponent<MeshFilter>();
            newObject.AddComponent<MeshRenderer>();

            // Convert to prefab
            string prefabPath = destinationFolder + prefabName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(newObject, prefabPath);

            // Destroy temporary game object
            DestroyImmediate(newObject);

            AssetDatabase.Refresh();

            Debug.Log("New prefab created!");
        }
    }
}