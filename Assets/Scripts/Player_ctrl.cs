using System.Collections.Generic;
using UnityEngine;

public class Player_ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<Player> plays = new List<Player>();

    public static Dictionary<int, Item> ItemList = new Dictionary<int, Item>();
    static Item[] item;

    public static Bag BagUI;
    //public static GameObject MakeSuccessUI;
   // public static GameObject MakeFailedUI;
    public static void Init_bag()
    {
        BagUI = GameObject.Find("Canvas/Bag").GetComponent<Bag>();
        //MakeSuccessUI = GameObject.Find("Canvas/MakeSuccess");
        //MakeFailedUI = GameObject.Find("Canvas/MakeFail");
        //MakeSuccessUI.SetActive(false);
        //MakeFailedUI.SetActive(false);
        item = Resources.LoadAll<Item>("Prefabs/items/");
        for (int i = 0; i < item.Length; ++i)
        {
            Player_ctrl.ItemList.Add(item[i].id, item[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
