using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Full_screen : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle.isOn = true;
        toggle.onValueChanged.AddListener(cg);
    }

    void cg(bool t)
    {
        Debug.Log(t);
        Screen.fullScreen = t;
    }
}
