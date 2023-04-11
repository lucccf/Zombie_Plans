using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProtalButton : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject allprotal;
    GameObject closebutton;
    void Start()
    {
        allprotal = GameObject.Find("PlayerPanel").transform.Find("AllProtal").gameObject;
        closebutton = allprotal.transform.Find("Background/CloseButton").gameObject;
        gameObject.GetComponent<Button>().onClick.AddListener(HandleUI);
        closebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
    }
    private void HandleUI()
    {

        bool showUI = !allprotal.activeSelf;
        allprotal.SetActive(showUI);

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
        allprotal.SetActive(false);
        gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        if (allprotal != null)
        {
            if (allprotal.activeSelf == true)
            {
                allprotal.SetActive(false);
            }
        }
    }
}
