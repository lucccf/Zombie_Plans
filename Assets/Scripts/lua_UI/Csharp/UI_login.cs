using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI_ref u = GetComponent<UI_ref>();
        UImanager.DoLuafile("Open_UIlogin.lua");
        UImanager.GetLuafunc("UI_login_addlis").Call(u);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
