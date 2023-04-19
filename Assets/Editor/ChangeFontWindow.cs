using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Timeline;
using System.Collections.Generic;
using System.IO;

public class ChangeFontWindow : EditorWindow
{
    [MenuItem("Tools/更换字体")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(ChangeFontWindow));
    }
    Font toChange;
    static Font toChangeFont;
    FontStyle toFontStyle;
    static FontStyle toChangeFontStyle;
    private static string toSavePrefabPath = "Assets/Resources/Prefabs/UI";

    void OnGUI()
    {
        EditorGUILayout.LabelField("预制体保存路径：:", toSavePrefabPath, GUILayout.Width(110));
        toSavePrefabPath = EditorGUILayout.TextArea(toSavePrefabPath, GUILayout.Width(250));

        toChange = (Font)EditorGUILayout.ObjectField(toChange, typeof(Font), true, GUILayout.MinWidth(100f));
        toChangeFont = toChange;
        toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(100f));
        toChangeFontStyle = toFontStyle;
        if (GUILayout.Button("更换"))
        {
            ModifyPrefab(toSavePrefabPath);
        }
    }
    public static void Change()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        if (!canvas)
        {
            Debug.Log("NO Canvas");
            return;
        }
        Transform[] tArray = canvas.GetComponentsInChildren<Transform>();
        for (int i = 0; i < tArray.Length; i++)
        {
            Text t = tArray[i].GetComponent<Text>();
            if (t)
            {
                //这个很重要，博主发现如果没有这个代码，unity是不会察觉到编辑器有改动的，自然设置完后直接切换场景改变是不被保存
                //的  如果不加这个代码  在做完更改后 自己随便手动修改下场景里物体的状态 在保存就好了
                Undo.RecordObject(t, t.gameObject.name);
                t.font = toChangeFont;
                t.fontStyle = toChangeFontStyle;
                //相当于让他刷新下 不然unity显示界面还不知道自己的东西被换掉了  还会呆呆的显示之前的东西
                EditorUtility.SetDirty(t);
            }
        }
        Debug.Log("Succed");
    }

    private void ModifyPrefab(string path)
    {
        //获取文件下所有预制体文件
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfos = info.GetFiles("*.prefab");
        List<GameObject> prefabs = new List<GameObject>();
        foreach (var item in fileInfos)
        {
            string paths = $"{path}/{item.Name}";
            GameObject prefab = AssetDatabase.LoadAssetAtPath(paths, typeof(GameObject)) as GameObject;
            prefabs.Add(prefab);
        }
        //修改属性
        for (int i = 0; i < prefabs.Count; i++)
        {
            Transform[] tArray = prefabs[i].GetComponentsInChildren<Transform>();
            for (int j = 0; j < tArray.Length; j++)
            {
                Text t = tArray[j].GetComponent<Text>();
                if (t)
                {
                    //这个很重要，博主发现如果没有这个代码，unity是不会察觉到编辑器有改动的，自然设置完后直接切换场景改变是不被保存
                    //的  如果不加这个代码  在做完更改后 自己随便手动修改下场景里物体的状态 在保存就好了
                    Undo.RecordObject(t, t.gameObject.name);
                    t.font = toChangeFont;
                    t.fontStyle = toChangeFontStyle;
                    PrefabUtility.SavePrefabAsset(prefabs[i]);
                    //相当于让他刷新下 不然unity显示界面还不知道自己的东西被换掉了  还会呆呆的显示之前的东西
                    //EditorUtility.SetDirty(t);
                }
            }
            Debug.Log("Succed");
            //if (prefabs[i].transform.childCount > 0 && prefabs[i].transform.GetChild(0) != null)
            //{
            //    if (prefabs[i].GetComponent<Rigidbody>() != null)
            //    {
            //        prefabs[i].GetComponent<Rigidbody>().isKinematic = false;
            //        PrefabUtility.SavePrefabAsset(prefabs[i]);
            //    }
            //}
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}