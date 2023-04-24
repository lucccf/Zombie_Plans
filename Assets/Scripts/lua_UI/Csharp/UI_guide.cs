using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_guide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UI_ref u = GetComponent<UI_ref>();
        UImanager.DoLuafile("UI_guide.lua");
        UImanager.GetLuafunc("UI_guide_addlis").Call(u);
    }
}
