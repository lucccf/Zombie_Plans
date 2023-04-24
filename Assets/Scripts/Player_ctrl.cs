using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<Player> plays = new List<Player>();

    public static Dictionary<int, Item> ItemList = new Dictionary<int, Item>();
    static Item[] item;

    public static NewBag BagUI;
    public static Text QCD;
    public static Text ECD;
    public static Fix_vector2 HomePos;

    public static Dictionary<(int, int), int> Attack = new Dictionary<(int, int), int>();
    public static Dictionary<int, WolfBox> WolfBox = new Dictionary<int, WolfBox>();

    public static bool checkattack(int sf_id, int op_id)
    {
        if (Attack.ContainsKey((sf_id, op_id)))
        {
            if (Attack[(sf_id, op_id)] != 0)
            {
                return true;
            }
        }
        return false;
    }

    public static void Init_bag()
    {
        BagUI = GameObject.Find("BagButton").GetComponent<NewBag>();
        QCD = GameObject.Find("QCD").GetComponentInChildren<Text>();
        ECD = GameObject.Find("ECD").GetComponentInChildren<Text>();
        //MakeSuccessUI = GameObject.Find("Canvas/MakeSuccess");
        //MakeFailedUI = GameObject.Find("Canvas/MakeFail");
        //MakeSuccessUI.SetActive(false);
        //MakeFailedUI.SetActive(false);
        List<object> items = AB.getitems();
        item = new Item[items.Count];
        for(int i = 0; i < items.Count; i++)
        {
            item[i] = (Item)items[i];
        }
        ItemList.Clear();
        Attack = new Dictionary<(int, int), int>();
        for (int i = 0; i < item.Length; ++i)
        {
            ItemList.Add(item[i].id, item[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
