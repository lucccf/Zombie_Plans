local panel
local GameObject = UnityEngine.GameObject
local Resources = UnityEngine.Resources
local MyBundle = MyBundle

UIload_start = function ()
    local UImanager = require "UImanager"
    local obj = UImanager.Open_UI("UI_login")
    while type(obj) == 'nil' do
        --obj = Open_UI("UI_login")
    end
    local uiRefs = obj:GetComponent("UI_ref")
    panel = BindLuaUI(uiRefs)
    panel.Start.onClick:AddListener(Onclick_st)
    panel.Option.onClick:AddListener(Onclick_op)
    panel.Quit_opt.onClick:AddListener(Onclick_qu_op)
    panel.Quit.onClick:AddListener(Onclick_qu)
end

Onclick_st = function ()
    UnityEngine.SceneManagement.SceneManager.LoadScene("Game")
end

Onclick_op = function ()
    panel.Board:SetActive(true)
end

Onclick_qu_op = function ()
    panel.Board:SetActive(false)
end

Onclick_qu = function ()
    UnityEngine.Application.Quit()
end

Volume_change = function ()
    panel.UIstart.volume = panel.Volume_slider.value
end

UIload_start()
