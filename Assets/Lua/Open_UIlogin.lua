local panel
local GameObject = UnityEngine.GameObject

UI_login_addlis = function (uiRefs)
    panel = BindLuaUI(uiRefs)
    panel.Login_but.onClick:AddListener(Onclick_Login_but)
    print("??????");
    --panel.Rig_but.onClick:AddListener(Onclick_op)
end

Onclick_Login_but = function ()
    local p = Net.LoginData()
    p.Username = panel.Name_txt.text
    p.Passwd = panel.Pwd_txt.text
    p.Opt = Net.LoginData.Types.Operation.Login
    print("!!!!!!");
    Clisocket.Sendmessage(BODYTYPE.LoginData, p)
end