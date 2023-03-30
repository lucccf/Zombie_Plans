using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEMTYPE
{
    Mineral = 0,//矿石
    Herb,       //草药
    RedMat,
    BlueMat,
    GreenMat,
    YellowMat,
    HealingPotion,
    RedProp,
    BlueProp,
    GreenProp,
    YellowProp,
};
public class Item_Bag
{
    public int id;
    public Item item;
    public ITEMTYPE type;
    bool abilityToUse;
}
public class Item_Scene {
    public int id;
    public Item item;
    public ITEMTYPE type;
    bool abilityToUse;
    Fix_col2d f;
}