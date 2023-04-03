using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    public Button b;

    void Start()
    {
        b.onClick.AddListener(LoginController.LoginClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
