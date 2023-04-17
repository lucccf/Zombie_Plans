using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject op;
    public Button but;
    void Start()
    {
        but.onClick.AddListener(opt);
    }

    void opt()
    {
        op.SetActive(true);
    }
}
