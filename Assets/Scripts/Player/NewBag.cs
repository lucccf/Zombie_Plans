using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBag : MonoBehaviour
{
    public GameObject CloseButton;
    public GameObject ItemParent;

    private bool OpenBag = false;
    //private Dictionary<int, Item> OriginItem = new Dictionary<int, Item>();
    private Dictionary<int, GameObject> ItemManager = new Dictionary<int, GameObject>();
    private Dictionary<int, int> ItemNumber = new Dictionary<int, int>();
    void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(CloseButtonOnClick);
        Item[] item = Resources.LoadAll<Item>("Prefabs/items/");
        for (int i = 0; i < item.Length; ++i)
        {
            //OriginItem.Add(item[i].id, item[i]);
            ItemNumber.Add(item[i].id, 0);
        }
        ItemParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject TmpBagItem;
    public void GetItem(int id, int number)
    {
        if (ItemNumber[id] > 0)
        {
            ItemNumber[id] += number;
            ItemManager[id].GetComponentInChildren<Text>().text = ItemNumber[id].ToString();
            if (ItemNumber[id] <= 0)
            {
                Destroy(ItemManager[id]);
                ItemManager.Remove(id);
            }
        }
        else
        {
            TmpBagItem.GetComponent<Image>().sprite = Main_ctrl.GetItemById(id).image; //OriginItem[id].image;
            GameObject NewItem = Instantiate(TmpBagItem.gameObject, ItemParent.transform);

            ItemManager.Add(id, NewItem);
            ItemNumber[id] += number;
            NewItem.GetComponentInChildren<Text>().text = number.ToString();
        }
    }

    void CloseButtonOnClick()
    {
        OpenBag = !OpenBag;
        ItemParent.SetActive(OpenBag);
    }
}
