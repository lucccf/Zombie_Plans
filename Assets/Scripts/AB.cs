using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB : MonoBehaviour
{
    // Start is called before the first frame update
    static Dictionary<string, object> objs = new Dictionary<string, object>();
    static List<AssetBundleCreateRequest> bundle_req = new List<AssetBundleCreateRequest>();

    public static void LoadALL()
    {
        bundle_req.Add(AssetBundle.LoadFromFileAsync(Application.dataPath + "/../AssetBundles/prefabs"));
        //AssetBundleRequest request = bundle.LoadAssetAsync(assetName);
    }

    public static void GetAllobjs()
    {
        foreach (var x in bundle_req)
        {
            if (x.isDone)
            {
                /*
                foreach(var y in x.assetBundle)
                {

                }
                */
            }
        }
    }
    
    public static object getobj(string name)
    {
        return Resources.Load("Prefabs/" + name);
    }
}
