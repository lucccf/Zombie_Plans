using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    GameObject playerpanel;
    GameObject tmp;
    GameObject titletext;
    GameObject closebutton;
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
<<<<<<< HEAD
        /*if (tmp.activeSelf == true) {
            tmp.SetActive(false);
        }*/
=======
        if (tmp != null) 
        {
            if (tmp.activeSelf == true)
            {
                tmp.SetActive(false);
            }
        }
>>>>>>> 821c4be6f702e29654dcdacaa89180b4e50b521c
    }
}
