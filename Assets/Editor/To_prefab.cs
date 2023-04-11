using UnityEditor;
using UnityEngine;
using System.IO;

public class To_prefab : EditorWindow
{
    [MenuItem("Tools/To_prefab")]
    static void Init()
    {
        To_prefab window = (To_prefab)EditorWindow.GetWindow(typeof(To_prefab));
        window.Show();
    }

    string TexPath = "Assets/Resources/Prefabs";
    string destinationFolder = "Assets/Resources/Prefabs";

    void OnGUI()
    {
        GUILayout.Label("Create New Prefab", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.Label("Texture Floder");
        TexPath = EditorGUILayout.TextField(TexPath);

        GUILayout.Label("Destination Folder");
        destinationFolder = EditorGUILayout.TextField(destinationFolder);

        if (GUILayout.Button("Work"))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(TexPath);
            foreach(var x in directoryInfo.GetFiles())
            {
                string name = x.Name;
                GameObject obj = new GameObject(name);
                SpriteRenderer ren = obj.AddComponent<SpriteRenderer>();
                Texture2D texture = new Texture2D(1, 1);
                byte[] fileData = File.ReadAllBytes(x.FullName);
                texture.LoadImage(fileData);
                ren.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                Debug.Log(destinationFolder + "/" + name + ".prefab");

                //PrefabUtility.SaveAsPrefabAsset(obj, destinationFolder + "/" + name + ".prefab");
                DestroyImmediate(obj);
            }
            AssetDatabase.Refresh();

            Debug.Log("New prefab created!");
        }
    }
}