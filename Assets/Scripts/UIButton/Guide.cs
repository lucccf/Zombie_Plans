using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    public GameObject g1;
    public GameObject g2;
    public GameObject g3;

    public void Start()
    {
        g1.SetActive(false);
        g2.SetActive(false);
        g3.SetActive(false);
    }

    public void G1()
    {
        if (g1.activeSelf)
        {
            g1.SetActive(false);
        }
        else
        {
            g1.SetActive(true);
        }
    }
    public void G2()
    {
        if (g2.activeSelf)
        {
            g2.SetActive(false);
        }
        else
        {
            g2.SetActive(true);
        }
    }
    public void G3()
    {
        if (g3.activeSelf)
        {
            g3.SetActive(false);
        }
        else
        {
            g3.SetActive(true);
        }
    }
}
