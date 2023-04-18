﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Chatbutton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chatUI;
    public GameObject closebutton;
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ShowChatUI);
        closebutton.GetComponent<Button>().onClick.AddListener(CloseUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChatUI.t + 5f < Time.time)
        {
            ChatUI.commitbuttonable = true;
        }
    }
    void CloseUI() {
        if (chatUI.activeSelf) {
            chatUI.SetActive(false);
            ClientSend.Send = true;
        }
    }
    void ShowChatUI() {
        if (ChatUI.commitbuttonable == true) {
            bool showUI = !chatUI.activeSelf;
            ClientSend.Send = chatUI.activeSelf;
            chatUI.SetActive(showUI);
        }
    }
}
