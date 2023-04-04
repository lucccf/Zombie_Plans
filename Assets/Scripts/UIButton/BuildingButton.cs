using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public long buildingid;
    GameObject playerpanel;
    GameObject tmp;
    GameObject titletext;
    GameObject closebutton;
    GameObject itemimage;
    GameObject itemtext;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HandleUI);
        playerpanel = GameObject.Find("PlayerPanel");
        if (gameObject.name == "home(Clone)")
        {
            tmp = playerpanel.transform.Find("HomeUI").gameObject;
            closebutton = tmp.transform.Find("Background").transform.Find("CloseButton").gameObject;
            closebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
        }
        else
        {
            tmp = playerpanel.transform.Find("Facility").gameObject;
            //标题
            titletext = tmp.transform.Find("Title").transform.Find("Text").gameObject;
            titletext.GetComponent<Text>().text = gameObject.name;
            //关闭键
            closebutton = tmp.transform.Find("Background").transform.Find("CloseButton").gameObject;
            closebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
            //材料列表

            itemimage = tmp.transform.Find("ItemTitle").transform.Find("ItemDetail").transform.Find("ItemImage").gameObject;
            itemtext = tmp.transform.Find("ItemTitle").transform.Find("ItemDetail").transform.Find("ItemImage").transform.Find("Text").gameObject;
            Facility fa = Flow_path.facilities[buildingid];
            Dictionary<int, int> curmat = fa.materials;
            foreach (KeyValuePair<int, int> mat in curmat)
            {
                Item x = Main_ctrl.GetItemById(mat.Key);
                itemimage.GetComponent<Image>().sprite = x.image;
                itemtext.GetComponent<Text>().text = "所需数量："+ mat.Value;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleUI()
    {
        
        
        bool showUI = !tmp.activeSelf;
        tmp.SetActive(showUI);

        if (showUI)
        {
            // 创建Button
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void CloseUI() 
    {
        tmp.SetActive(false);
        gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        if (tmp != null) 
        {
            if (tmp.activeSelf == true)
            {
                tmp.SetActive(false);
            }
        }
    }
}
