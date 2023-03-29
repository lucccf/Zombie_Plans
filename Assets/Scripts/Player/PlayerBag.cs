using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag
{
    public PlayerBag() { }
    private Dictionary<int, int> BagItem = new Dictionary<int, int>();
    public int BagGetItemsNums(int id)
    {
        if (!BagItem.ContainsKey(id))
        {
            BagItem.Add(id, 0);
        }
        return BagItem[id];
    }

    public bool BagCheckItemNums(int id, int num)
    {
        if (!BagItem.ContainsKey(id))
        {
            BagItem.Add(id, 0);
        }
        if (BagItem[id] < num) return false;
        else return true;
    }

    public bool BagGetItem(int id, int num)
    {
        if (!BagItem.ContainsKey(id))
        {
            BagItem.Add(id, 0);
        }
        if (num >= 0)
        {
            BagItem[id] += num;
            return true;
        }
        else
        {
            if (BagItem[id] < -num) return false;
            else
            {
                BagItem[id] += num;
                return true;
            }
        }
    }
}
