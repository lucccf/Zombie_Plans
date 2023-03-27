using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

public class SetSpritePropertiesEditor : EditorWindow
{
    private string folderPath;

    [MenuItem("Window/Set Sprite Properties")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SetSpritePropertiesEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Sprite Properties", EditorStyles.boldLabel);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        if (GUILayout.Button("Set Properties"))
        {
            SetSpriteProperties();
        }
    }

    private void SetSpriteProperties()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            SpriteRenderer[] spriteRenderers = prefab.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.drawMode = SpriteDrawMode.Tiled;
                Sprite sprite = sr.sprite;
                TextureImporter textureImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(sprite)) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
                    textureImporter.ReadTextureSettings(textureImporterSettings);
                    textureImporterSettings.spriteMeshType = SpriteMeshType.FullRect;
                    textureImporter.SetTextureSettings(textureImporterSettings);
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(sprite));

                }
            }
            EditorUtility.SetDirty(prefab);
        }
    }
}
