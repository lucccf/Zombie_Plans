using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB : MonoBehaviour
{
    // Start is called before the first frame update
    static Dictionary<string, object> objs = new Dictionary<string, object>();

    public static void LoadALL()
    {
        AssetBundleCreateRequest bundle_req = AssetBundle.LoadFromFileAsync(Application.dataPath + "../AB");
        //AssetBundleRequest request = bundle.LoadAssetAsync(assetName);
    }
    
    public static object getobj(string name)
    {
        return Resources.Load("Prefabs/" + name);
    }
}
