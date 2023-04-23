using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    LuaState lua;
    Dictionary <string, LuaFunction> Luafunc_man;
    // Start is called before the first frame update
    void Start()
    {
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);
        DelegateFactory.Init();
        BindLua.Register(lua);
        lua.AddSearchPath(Application.dataPath + "/Scripts/lua");
        DontDestroyOnLoad(gameObject);
        //lua.DoFile("UImanager.lua");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LuaFunction GetLuafunc(string func)
    {
        if (Luafunc_man.ContainsKey(func))
        {
            return Luafunc_man[func];
        }
        LuaFunction luafunc = lua.GetFunction(func);
        Luafunc_man.Add(func, luafunc);
        return luafunc;
    }

    public void OpenUI(string name)
    {
        //lua.GetFunction("Open_" + name).Call();
        GetLuafunc("Open_" + name).Call();
    }

    public void DoLuafile(string name)
    {
        lua.DoFile("name");
    }

    private void OnDestroy()
    {
        lua.Dispose();
    }
}
