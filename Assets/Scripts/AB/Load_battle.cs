using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_battle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;

    void Start()
    {
        Instantiate((GameObject)AB.getobj("Battle_scene"), canvas.transform, false);
        Camera c1 = Instantiate((GameObject)AB.getobj("All_camera")).GetComponent<Camera>();
        c1.targetTexture = (RenderTexture)AB.getobj("All_map");
        Camera c2 = Instantiate((GameObject)AB.getobj("Tiny_camera")).GetComponent<Camera>();
        c2.targetTexture = (RenderTexture)AB.getobj("Tiny_map");
    }
}
