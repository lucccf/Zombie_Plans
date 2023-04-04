using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bag : MonoBehaviour
{
    public GameObject bag;
    public GameObject CloseButton;
    public GameObject ItemParent;
    public Text ItemNameText;
    public Text ItemDescriptionText;

    private bool OpenBag = false;
    private Dictionary<int, Item> OriginItem = new Dictionary<int, Item>();
    private Dictionary<int, GameObject> ItemManager = new Dictionary<int, GameObject>();
    private Dictionary<int, int> ItemNumber = new Dictionary<int, int>();
    void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(CloseButtonOnClick);
        Item[] item = Resources.LoadAll<Item>("items/");
        for (int i = 0; i < item.Length; ++i)
        {
            OriginItem.Add(item[i].id, item[i]);
            ItemNumber.Add(item[i].id, 0);
        }
        bag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OpenBag = !OpenBag;
            bag.SetActive(OpenBag);
        }
    }
    public GameObject TmpBagItem;
    public void GetItem(int id,int number)
    {
        //Debug.Log(number);
        //if (ItemManager.ContainsKey(x.id))
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
            TmpBagItem.GetComponent<Image>().sprite = OriginItem[id].image;
            GameObject NewItem = Instantiate(TmpBagItem.gameObject, transform.position, transform.rotation);
            NewItem.transform.parent = ItemParent.transform;
            NewItem.GetComponent<Button>().onClick.AddListener(
                delegate ()
                {
                    OnClick(id);
                });
            ItemManager.Add(id, NewItem);
            ItemNumber[id] += number;
            //ItemNumber.Add(x.id, number);
            //OriginItem.Add(x.id, x);
            NewItem.GetComponentInChildren<Text>().text = number.ToString();
        }
    }
    public bool CheckItem(int id,int number)
    {
        if (ItemNumber[id] < number) return false;
        else return true;
    }
    void OnClick(int id)
    {
        ItemNameText.text = OriginItem[id].name;
        ItemDescriptionText.text = OriginItem[id].description;
    }
    void CloseButtonOnClick()
    {
        OpenBag = false;
        bag.SetActive(false);
    }
}
