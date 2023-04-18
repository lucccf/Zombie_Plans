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
    //public static GameObject MakeSuccessUI;
   // public static GameObject MakeFailedUI;
    public static void Init_bag()
    {
        BagUI = GameObject.Find("Canvas/PlayerPanel/BagButton").GetComponent<NewBag>();
        QCD = GameObject.Find("QCD").GetComponentInChildren<Text>();
        ECD = GameObject.Find("ECD").GetComponentInChildren<Text>();
        //MakeSuccessUI = GameObject.Find("Canvas/MakeSuccess");
        //MakeFailedUI = GameObject.Find("Canvas/MakeFail");
        //MakeSuccessUI.SetActive(false);
        //MakeFailedUI.SetActive(false);
        item = Resources.LoadAll<Item>("Prefabs/items/");
        ItemList.Clear();
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
