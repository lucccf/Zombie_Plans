using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindLua : MonoBehaviour
{

    private static LuaCSFunction bindLuaUI = new LuaCSFunction(BindLuaUI);

    public static void Register(LuaState L)
    {
        L.BeginModule(null);
        L.RegFunction("BindLuaUI", bindLuaUI);
        L.EndModule();
    }
    public static void BindUI(IntPtr L)
    {
        if (LuaDLL.lua_gettop(L) == 1)
        {
            var r = ToLua.ToObject(L, 1);
            UI_ref uiRefs = (UI_ref)r;
            LuaUIobj[] refs = uiRefs.refs;
            LuaDLL.lua_createtable(L, refs.Length, 0);
            LuaDLL.lua_pushstring(L, "Root");
            ToLua.Push(L, uiRefs.gameObject);
            LuaDLL.lua_rawset(L, -3);
            BindLuaTable(L, refs);
        }
        else
        {
            throw new LuaException("Must be one table arg");
        }
    }

    public static void BindLuaTable(IntPtr L, LuaUIobj[] comRefs)
    {
        for (int i = 0; i < comRefs.Length; i++)
        {
            string name;
            if (comRefs[i].type != objtype.AudioSource)
            {
                GameObject item = (GameObject)comRefs[i].obj;
                name = item.name;
                int n;
                for (n = 0; n < name.Length; n++)
                {
                    if (name[n] == '(') break;
                }
                name = name.Substring(0, n);
                LuaDLL.lua_pushstring(L, name);
                ToLua.Push(L, GenComponent(item, comRefs[i].type));
            }
            else
            {
                AudioSource item = (AudioSource)comRefs[i].obj;
                name = item.name;
                int n;
                for (n = 0; n < name.Length; n++)
                {
                    if (name[n] == '(') break;
                }
                name = name.Substring(0, n);
                LuaDLL.lua_pushstring(L, name);
                ToLua.Push(L, item);
            }
            LuaDLL.lua_rawset(L, -3);
        }
    }

    public static UnityEngine.Object GenComponent(GameObject go, objtype nodetype)
    {
        switch (nodetype)
        {
            case objtype.Button: return go.GetComponent<Button>();
            case objtype.Image: return go.GetComponent<Image>();
            case objtype.Text: return go.GetComponent<Text>();
            case objtype.InputField: return go.GetComponent<InputField>();
            case objtype.AudioSource: return go.GetComponent<AudioSource>();
            case objtype.Slider: return go.GetComponent<Slider>();
        }
        return go;
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    static int BindLuaUI(IntPtr L)
    {
        try
        {
            BindUI(L);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
}