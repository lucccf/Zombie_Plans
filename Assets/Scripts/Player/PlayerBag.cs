using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag
{
    private long PlayerId;
    public PlayerBag(long pid)
    {
        PlayerId = pid;
    }
    private Dictionary<int, int> BagItem = new Dictionary<int, int>();
    public bool checkid()
    {
        if (PlayerId == Main_ctrl.Ser_to_cli[Main_ctrl.user_id]) return true;
        else return false;
    }
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
    
    private bool BagGetItem(int id, int num)
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

    public bool BagGetItem(int id, int num, NewBag bagui)
    {
        if (checkid() == false) return BagGetItem(id, num);
        else
        {
            if (!BagItem.ContainsKey(id))
            {
                BagItem.Add(id, 0);
            }
            if (num >= 0)
            {
                BagItem[id] += num;
                bagui.GetItem(id, num);
                return true;
            }
            else
            {
                if (BagItem[id] < -num) return false;
                else
                {
                    BagItem[id] += num;
                    bagui.GetItem(id, num);
                    return true;
                }
            }
        }
    }
}
