﻿using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkTable : MonoBehaviour
{
    //public Bag bag;
    public GameObject worktable;
    public GameObject CloseButton;
    public GameObject MakeButton;
    public GameObject ItemListParent;
    public GameObject NeedItemParent;
    public Text ItemDescription;
    public Item[] MakedItemList;

    private GameObject TmpMakedItemButton;

    //private Dictionary<int, Item> OriginItem = new Dictionary<int, Item>();
    private bool OpenNemu = false;
    private int NowChecking = -1;

    void Start()
    {
        
        CloseButton.GetComponent<Button>().onClick.AddListener(CloseButtonOnClick);
        MakeButton.GetComponent<Button>().onClick.AddListener(MakeButonOnClick);
        for(int i = 0; i < MakedItemList.Length; ++i)
        {
            TmpMakedItemButton = (GameObject)Resources.Load("Prefabs/UI/Button");
            TmpMakedItemButton.GetComponent<Image>().sprite = MakedItemList[i].image;
            //TmpMakedItemButton.GetComponentInChildren<Text>().text = MakedItemList[i].name;
            //GameObject NewItem = Instantiate(TmpMakedItemButton, transform.position, transform.rotation);
            GameObject NewItem = Instantiate(TmpMakedItemButton, ItemListParent.transform);
            //NewItem.transform.parent = ItemListParent.transform;
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

    private GameObject TmpNeedItem;
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
            TmpNeedItem = (GameObject)Resources.Load("Prefabs/UI/NeeItemPrefabs");
            TmpNeedItem.GetComponentInChildren<Image>().sprite = Main_ctrl.GetItemById(MakedItemList[x].MakeNeeds[i]).image;
            TmpNeedItem.GetComponentInChildren<Text>().text = MakedItemList[x].NeedsNumber[i].ToString();
            //GameObject NewItem = Instantiate(TmpNeedItem, transform.position, transform.rotation);
            Instantiate(TmpNeedItem, NeedItemParent.transform);
            //NewItem.transform.parent = NeedItemParent.transform;
        }
    }
    void MakeButonOnClick()
    {
        if(NowChecking == -1)
        {
            return;
        }


        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.MoveItem;
        x.Userid = (int)Main_ctrl.user_id;
        x.Itemid = MakedItemList[NowChecking].id;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
    }

    void CloseButtonOnClick()
    {
        OpenNemu = false;
        worktable.SetActive(false);
    }
}
