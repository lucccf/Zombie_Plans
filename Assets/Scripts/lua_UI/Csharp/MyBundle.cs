using LuaInterface;
using System.Collections.Generic;
using UnityEngine;

public class MyBundle : MonoBehaviour
{
    // Start is called before the first frame update
    public MyBundle LoadFromFile(string path)
    {
        MyBundle bundle = new MyBundle();
        bundle.assetBundle = AssetBundle.LoadFromFile(path);
        return bundle;
    }

    public void LoadAssetAsync(string name)
    {
        assetBundleRequests.Add(name, assetBundle.LoadAssetAsync(name));
    }

    public Object GetAssetAsync(string name)
    {
        if (assetBundleRequests.ContainsKey(name))
        {
            if (assetBundleRequests[name].isDone)
            {
                return assetBundleRequests[name].asset;
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    [NoToLua]
    public AssetBundle assetBundle;
    public Dictionary<string, AssetBundleRequest> assetBundleRequests = new Dictionary<string, AssetBundleRequest>();
}
