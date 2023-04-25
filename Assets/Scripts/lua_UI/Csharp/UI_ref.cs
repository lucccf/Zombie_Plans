using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum objtype
{
    Object,
    Button,
    Text,
    Image,
    AudioSource,
    Slider,
    InputField
    /*other type*/
}

[Serializable]
public struct LuaUIobj
{
    public UnityEngine.Object obj;
    public objtype type;
}

[DisallowMultipleComponent]
public class UI_ref : MonoBehaviour
{
    public LuaUIobj[] refs = null;
}
