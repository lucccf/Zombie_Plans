using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Change_texture : EditorWindow
{
    [MenuItem("Tools/Texture Settings")]
    static void ShowWindow()
    {
        Change_texture window = (Change_texture)GetWindow(typeof(Change_texture));
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Apply Settings"))
        {
            ApplySettings();
        }
    }

    private void ApplySettings()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:texture2D");

        foreach (string textureGuid in textureGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(textureGuid);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }
        }

        Debug.Log("Texture Settings Applied");
    }
}