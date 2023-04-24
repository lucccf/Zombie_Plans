using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    static LuaState lua;
    static Dictionary<string, LuaFunction> Luafunc_man = new Dictionary<string, LuaFunction>();
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

    public static LuaFunction GetLuafunc(string func)
    {
        if (Luafunc_man.ContainsKey(func))
        {
            return Luafunc_man[func];
        }
        LuaFunction luafunc = lua.GetFunction(func);
        Luafunc_man.Add(func, luafunc);
        return luafunc;
    }

    public static void DoLuafile(string name)
    {
        lua.DoFile(name);
    }

    private void OnDestroy()
    {
        lua.Dispose();
    }
}
