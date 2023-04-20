using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AB : MonoBehaviour
{
    // Start is called before the first frame update
    static Dictionary<string, object> objs = new Dictionary<string, object>();
    static List<AssetBundleCreateRequest> bundle_req = new List<AssetBundleCreateRequest>();
    static List<AssetBundleRequest> bundles= new List<AssetBundleRequest>();

    public static void LoadALL()
    {
        /*
        bundle_req.Add(AssetBundle.LoadFromFileAsync("AssetBundles/prefabs"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync("AssetBundles/item"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync("AssetBundles/background"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync("AssetBundles/ui"));
        */

        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/AssetBundles/prefabs"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/AssetBundles/item"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/AssetBundles/background"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/AssetBundles/ui"));
        /*
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/../AssetBundles/prefabs"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/../AssetBundles/item"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/../AssetBundles/background"));
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/../AssetBundles/ui"));
        //AssetBundleRequest request = bundle.LoadAssetAsync(assetName);
        */
    }

    public static bool LoadAllbundles()
    {
        bool flag = true;
        foreach (var x in bundle_req)
        {
            if (!x.isDone)
            {
                flag = false;
            }
        }
        return flag;
    }

    public static void LoadAllobjs()
    {
        foreach (var x in bundle_req)
        {
            bundles.Add(x.assetBundle.LoadAllAssetsAsync());
        }
    }

    public static bool GetAllobjs()
    {
        bool flag = true;
        foreach (var x in bundles)
        {
            if (!x.isDone)
            {
                flag = false;
            }
        }
        return flag;
    }

    public static void FetchAllobjs()
    {
        foreach (var x in bundles)
        {
            foreach (var y in x.allAssets)
            {
                objs[y.name] = y;
            }
        }
        foreach (var x in bundle_req)
        {
            x.assetBundle.Unload(false);
        }
        Debug.Log("Load_OK");
        test1.p = 1;
    }

    public static object getobj(string name)
    {
        //Debug.Log(name);
        return objs[name];
        //return Resources.Load("Prefabs/" + name);
    }
}
