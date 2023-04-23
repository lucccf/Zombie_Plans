// Create an AssetBundle for Windows.
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class BuildAssetBundlesExample : MonoBehaviour
{
    public static string Build2Path = Application.dataPath + "/../BuildABs";
    [MenuItem("AB/Build Asset Bundles")]
    static void BuildSources()
    {
        if (Directory.Exists(Build2Path))
        {
            Directory.Delete(Build2Path, true);
        }
        Directory.CreateDirectory(Build2Path);
        BuildPipeline.BuildAssetBundles(Build2Path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        Debug.Log("Windows Finish!");
    }
}