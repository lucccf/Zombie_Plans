using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WolfBox : MonoBehaviour
{
    private Dictionary<int, int> BoxItem;
    private Dictionary<int, GameObject> BoxItemObject = new Dictionary<int, GameObject>();
    
    public GameObject ItemParent;
    public GameObject GetButton;
    public GameObject CloseButton;
    public Text Description;
    public Image SelectedImage;
    public Image UnSelectedImage;


    private int CheckItemId = -1;

    public Dictionary<int, int> TestItem = new Dictionary<int, int>();

    float alivetime = 0f;
    bool Created = false;

    void Start()
    {
        GetButton.GetComponent<Button>().onClick.AddListener(GetButtonOnCilck);
        CloseButton.GetComponent<Button>().onClick.AddListener(CloseButtonOnClick);

        TestItem.Add(1, 2);
        TestItem.Add(2, 4);
        TestItem.Add(3, 8);
        TestItem.Add(4, 1);
        TestItem.Add(5, 2);
        TestItem.Add(6, 3);
        TestItem.Add(7, 4);
        TestItem.Add(8, 1);
        TestItem.Add(9, 9);
    }



    public void GetItem(Dictionary<int,int>item)
    {
        BoxItem = item;
        //GameObject ItemButton = Instantiate((GameObject)AB.getobj("UI/BagItem"));
        GameObject ItemButton = Instantiate((GameObject)AB.getobj("BagItem"));
        foreach (int i in BoxItem.Keys)
        {
            BoxItemObject.Add(i, Instantiate(ItemButton, ItemParent.transform));
            BoxItemObject[i].GetComponent<Image>().sprite = Main_ctrl.GetItemById(i).image;
            BoxItemObject[i].GetComponentInChildren<Text>().text = BoxItem[i].ToString();
            BoxItemObject[i].GetComponent<Button>().onClick.AddListener(
                delegate ()
                {
                    ItemOnClick(i);
                });
        }
    }

    private void GetItem(int id)
    {
        --BoxItem[id];
        if (BoxItem[id] == 0)
        {
            Destroy(BoxItemObject[id]);
            BoxItemObject.Remove(id);
            BoxItem.Remove(id);
            Description.text = "";
        } else
        {
            BoxItemObject[id].GetComponentInChildren<Text>().text = BoxItem[id].ToString();
        }
    }

    private void ChangeFrame(GameObject target,int type)
    {
        Image[] q = target.GetComponentsInChildren<Image>();
        if (type == 0)
        {
            q[1].sprite = UnSelectedImage.sprite;
            q[1].color = UnSelectedImage.color;
        } else
        {
            q[1].sprite = SelectedImage.sprite;
            q[1].color = SelectedImage.color;
        }
    }
    private void ItemOnClick(int x)
    {
        if (BoxItem.ContainsKey(CheckItemId))
        {
            ChangeFrame(BoxItemObject[CheckItemId], 0);
        }
        CheckItemId = x;
        ChangeFrame(BoxItemObject[CheckItemId], 1);
        Description.text = Main_ctrl.GetItemById(x).name;
    }

    private void GetButtonOnCilck()
    {
        if(BoxItem.ContainsKey(CheckItemId))
        {
            GetItem(CheckItemId);
            Player_ctrl.plays[0].bag.BagGetItem(CheckItemId, 1,Player_ctrl.BagUI);
        }
    }
    private void CloseButtonOnClick()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        alivetime += Time.deltaTime;
        if(alivetime > 1f && Created == false)
        {
            Created = true;
            GetItem(TestItem);
        }
    }
}
