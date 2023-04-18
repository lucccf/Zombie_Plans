using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_panel : MonoBehaviour
{
    public GameObject dr, def, de, vic;
    // Start is called before the first frame update
    void Start()
    {
        dr.SetActive(false);
        def.SetActive(false);
        de.SetActive(false);
        vic.SetActive(false);
    }

    public void draw()
    {
        dr.SetActive(true);
    }

    public void defeated()
    {
        def.SetActive(true);
    }

    public void dead()
    {
        de.SetActive(true);
    }

    public void victory()
    {
        vic.SetActive(true);
    }
}
