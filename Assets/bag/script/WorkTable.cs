using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkTable : MonoBehaviour
{
    public Bag bag;
    public GameObject worktable;
    public GameObject CloseButton;
    public GameObject MakeButton;
    public GameObject ItemListParent;
    public GameObject NeedItemParent;
    public Text ItemDescription;
    public Item[] MakedItemList;
    public GameObject MakeSuccess;
    public GameObject MakeFail;

    public GameObject TmpMakedItemButton;

    private Dictionary<int, Item> OriginItem = new Dictionary<int, Item>();
    private bool OpenNemu = false;
    private int NowChecking = -1;
    void Start()
    {

        CloseButton.GetComponent<Button>().onClick.AddListener(CloseButtonOnClick);
        MakeButton.GetComponent<Button>().onClick.AddListener(MakeButonOnClick);
        Item[] ItemList = Resources.LoadAll<Item>("Prefabs/items/");
        for (int i = 0; i < ItemList.Length; ++i)
        {
            OriginItem.Add(ItemList[i].id, ItemList[i]);
        }
        for(int i = 0; i < MakedItemList.Length; ++i)
        {
            TmpMakedItemButton.GetComponent<Image>().sprite = MakedItemList[i].image;
            TmpMakedItemButton.GetComponentInChildren<Text>().text = MakedItemList[i].name;
            GameObject NewItem = Instantiate(TmpMakedItemButton, transform.position, transform.rotation);
            NewItem.transform.parent = ItemListParent.transform;
            int j = i;
            NewItem.GetComponent<Button>().onClick.AddListener(
                delegate ()
                {
                    ItemOnClick(j);
                });
        }
        worktable.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenNemu = !OpenNemu;
            worktable.SetActive(OpenNemu);
        }
    }

    public GameObject TmpNeedItem;
    void ItemOnClick(int x)
    {
        NowChecking = x;
        ItemDescription.text = MakedItemList[x].description;
        for(int i = 0; i< NeedItemParent.transform.childCount;++i)
        {
            Destroy(NeedItemParent.transform.GetChild(i).gameObject);
        }
        for(int i = 0; i < MakedItemList[x].MakeNeeds.Length; ++i)
        {
            TmpNeedItem.GetComponent<Image>().sprite = OriginItem[MakedItemList[x].MakeNeeds[i]].image;
            TmpNeedItem.GetComponentInChildren<Text>().text = MakedItemList[x].NeedsNumber[i].ToString();
            GameObject NewItem = Instantiate(TmpNeedItem, transform.position, transform.rotation);
            NewItem.transform.parent = NeedItemParent.transform;
        }
    }
    void MakeButonOnClick()
    {
        if(NowChecking == -1)
        {
            return;
        }
        bool CanMake = true;
        for (int i = 0; i < MakedItemList[NowChecking].MakeNeeds.Length; ++i)
        {
            if (bag.CheckItem(MakedItemList[NowChecking].MakeNeeds[i], MakedItemList[NowChecking].NeedsNumber[i]) == false)
            {
                CanMake = false;
                break;
            }
        }
        if(CanMake == false)
        {
            MakeFail.SetActive(true);
            //Debug.Log("合成材料不足");
        }
        else
        {
            //Debug.Log("合成成功");
            for (int i = 0; i < MakedItemList[NowChecking].MakeNeeds.Length; ++i)
            {
                bag.GetItem(MakedItemList[NowChecking].MakeNeeds[i], -MakedItemList[NowChecking].NeedsNumber[i]);
            }
            bag.GetItem(MakedItemList[NowChecking].id, 1);
            MakeSuccess.SetActive(true);
        }

    }
    void CloseButtonOnClick()
    {
        OpenNemu = false;
        worktable.SetActive(false);
    }
}
