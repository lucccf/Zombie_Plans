using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainUI;
    void Start()
    {
        Button closebutton= GetComponent<Button>();
        closebutton.onClick.AddListener(CloseUI);
    }

    void CloseUI() {
        MainUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
