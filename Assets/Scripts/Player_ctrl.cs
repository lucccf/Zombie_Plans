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
    public static Image QCD_mask;
    public static Image ECD_mask;
    public static Fix_vector2 HomePos;
    public static PublicAudio audiox;

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

    public static void init_color()
    {
        for(int i = 0; i < plays.Count; i++)
        {
            plays[i].Name.color = PlayerColor.playercolors[i];
            plays[i].Name.text = "user" + (i + 1);
        }
    }

    public static void Init_bag()
    {
        BagUI = GameObject.Find("BagButton").GetComponent<NewBag>();
        audiox = GameObject.Find("PublicAudio").GetComponent<PublicAudio>();
        GameObject QCD_Object = GameObject.Find("QCD");
        GameObject ECD_Object = GameObject.Find("ECD");
        QCD = QCD_Object.GetComponentInChildren<Text>();
        ECD = ECD_Object.GetComponentInChildren<Text>();
        Image[] children = QCD_Object.GetComponentsInChildren<Image>();
        foreach(Image child in children)
        {
            if(child.name == "mask")
            {
                QCD_mask = child;
            }
        }
        children = ECD_Object.GetComponentsInChildren<Image>();
        foreach(Image child in children)
        {
            if(child.name == "mask")
            {
                ECD_mask = child;
            }
        }
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
