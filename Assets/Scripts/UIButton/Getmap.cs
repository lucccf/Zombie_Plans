using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Getmap : MonoBehaviour
{
    public RawImage ri1;
    public RawImage ri2;
    public RawImage ri3;
    // Start is called before the first frame update
    void Start()
    {
        RenderTexture rt1 = (RenderTexture)AB.getobj("Tiny_map");
        ri1.texture = rt1;
        RenderTexture rt2 = (RenderTexture)AB.getobj("All_map");
        ri2.texture = rt2;
        ri3.texture = rt2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
