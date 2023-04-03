using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    GameObject playerpanel;
    GameObject tmp;
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
        if (tmp.activeSelf == true) {
            tmp.SetActive(false);
        }
    }
}
