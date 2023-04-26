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
        op.SetActive(false);
        but.onClick.AddListener(opt);
    }

    void opt()
    {
        if (op.activeSelf)
        {
            op.SetActive(false);
        }
        else
        {
            op.SetActive(true);
        }
    }
}
