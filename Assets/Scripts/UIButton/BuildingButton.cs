using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public long buildingid;
    GameObject playerpanel;
    GameObject WorkTable;
    GameObject tmp;
    GameObject titletext;
    GameObject closebutton;
    GameObject itemimage;
    GameObject itemtext;
    GameObject allfacility;
    GameObject homeuiclosebutton;
    GameObject worktableclosebutton;
    GameObject itemprogress;
    GameObject itemprogresstext;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HandleUI);
        playerpanel = GameObject.Find("PlayerPanel");
        if (gameObject.name == "home1(Clone)")
        {
            WorkTable = GameObject.Find("WorkTable");
            tmp = playerpanel.transform.Find("HomeUI").gameObject;
            allfacility = playerpanel.transform.Find("AllFacility").gameObject;
            homeuiclosebutton = playerpanel.transform.Find("HomeUI/Background/CloseButton").gameObject;
            homeuiclosebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
            homeuiclosebutton = playerpanel.transform.Find("AllFacility/Background/CloseButton").gameObject;
            homeuiclosebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
            worktableclosebutton = WorkTable.transform.Find("Background/background/CloseButton").gameObject;
            worktableclosebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
        }
        if (gameObject.name == "facility(Clone)")
        {
            tmp = playerpanel.transform.Find("Facility").gameObject;
            //标题
            titletext = playerpanel.transform.Find("Facility/Title/Text").gameObject;
            titletext.GetComponent<Text>().text = "设施详情";
            //关闭键
            closebutton = playerpanel.transform.Find("Facility/Background/CloseButton").gameObject;
            closebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
            //材料列表

            tmp.GetComponent<FacilityUpdate>().buildingid = buildingid;
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
        if (allfacility != null)
        {
            if (allfacility.activeSelf == true)
            {
                allfacility.SetActive(false);
            }
        }
    }
}
