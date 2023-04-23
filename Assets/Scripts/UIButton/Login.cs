﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    public Button login;
    public Button register;

    void Start()
    {
        login.onClick.AddListener(LoginController.LoginClicked);
        register.onClick.AddListener(LoginController.RegisterClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
