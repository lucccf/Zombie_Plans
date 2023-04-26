using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_start : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;

    void Start()
    {
        Instantiate((GameObject)AB.getobj("Start_scene"), canvas.transform);
        Music.Startx();
    }
}
