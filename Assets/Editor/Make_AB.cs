using UnityEditor;
using System.IO;
using UnityEngine;

public class Make_AB : EditorWindow
{
    [MenuItem("Tools/Batch Build AssetBundles")]
    static void Init()
    {
        string assetBundleDirectory = Application.dataPath + "/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        int bundleCount = manifest.GetAllAssetBundles().Length;
        Debug.LogFormat("打包了 {0} 个AssetBundle", bundleCount);

        AssetDatabase.Refresh();
    }
}
