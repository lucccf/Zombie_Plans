using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_login : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject canvas;

    void Start()
    {
        Instantiate((GameObject)AB.getobj("Login_scene"), canvas.transform);
    }
}
