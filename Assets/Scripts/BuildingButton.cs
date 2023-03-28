using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    GameObject playerpanel;
    GameObject tmp;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(HandleUI);
        playerpanel = GameObject.Find("PlayerPanel");
        if (gameObject.name == "home(clone)")
        {
            tmp = playerpanel.transform.Find("HomeUI").gameObject;
        }
        else
        {
            tmp = playerpanel.transform.Find("HomeUI").gameObject;
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

    void OnDestroy()
    {
        if (tmp.activeSelf == true) {
            tmp.SetActive(false);
        }
    }
}
