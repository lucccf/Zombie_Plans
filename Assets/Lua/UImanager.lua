local panel
local GameObject = UnityEngine.GameObject
local Bundle = MyBundle

LoadAssets = function (file)
    local f = io.open(file, "r");
    for line in f:lines() do
        Bundle.LoadAssetAsync(line)
    end
    f:close();
end

Open_UI = function (name)
    return Bundle.GetAssetAsync(name)
end

