local panel

UI_guide_addlis = function (uiRefs)
    panel = BindLuaUI(uiRefs)
    print(panel.skillque)
    panel.skillque.onClick:AddListener(Onclick_skill_que)
    panel.optionque.onClick:AddListener(Onclick_option_que)
    panel.timeque.onClick:AddListener(Onclick_time_que)
    print("???????")
end

Onclick_skill_que = function ()
    if (panel.skill.activeSelf) then
        panel.skill.setactive(false)
    else
        panel.skill.setactive(true)
    end
end

Onclick_option_que = function ()
    if (panel.option.activeSelf) then
        panel.option.setactive(false)
    else
        panel.option.setactive(true)
    end
end

Onclick_time_que = function ()
    if (panel.time.activeSelf) then
        panel.time.setactive(false)
    else
        panel.time.setactive(true)
    end
end
