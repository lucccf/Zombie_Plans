local panel

UI_login_addlis = function (uiRefs)
    panel = BindLuaUI(uiRefs)
    panel.Login_but.onClick:AddListener(Onclick_Login_but)
    panel.Rig_but.onClick:AddListener(Onclick_Rig_but)
    --panel.Rig_but.onClick:AddListener(Onclick_op)
end

Onclick_Login_but = function ()
    local p = Net.LoginData()
    p.Username = panel.Name_txt.text
    p.Passwd = panel.pwd.text
    p.Opt = Net.LoginData.Types.Operation.Login
    Clisocket.Sendmessage(BODYTYPE.LoginData, p)
end

Onclick_Rig_but = function ()
    local p = Net.LoginData()
    p.Username = panel.Name_txt.text
    p.Passwd = panel.pwd.text
    p.Opt = Net.LoginData.Types.Operation.Register
    Clisocket.Sendmessage(BODYTYPE.LoginData, p)
end