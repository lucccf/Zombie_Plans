using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "bag/items")]
public class Item : ScriptableObject
{
    public string itemname;
    public string description;
    public Sprite image;
    public int id;
    public int[] MakeNeeds;
    public int[] NeedsNumber;
}
