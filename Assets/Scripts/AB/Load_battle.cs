using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_battle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;

    void Start()
    {
        Instantiate((GameObject)AB.getobj("Battle_scene"), canvas.transform);
    }
}
