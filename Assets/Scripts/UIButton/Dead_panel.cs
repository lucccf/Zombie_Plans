using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead_panel : MonoBehaviour
{
    private static GameObject dr1, def1, de1, vic1;
    public GameObject dr, def, de, vic;
    // Start is called before the first frame update

    static bool flag = false;

    void Start()
    {
        flag = false;
        dr.SetActive(false);
        dr1 = dr;
        def.SetActive(false);
        def1 = def;
        de.SetActive(false);
        de1 = de;
        vic.SetActive(false);
        vic1 = vic;
    }

    public void deadstart()
    {
        Flow_path.Dead_start();
    }

    public static void draw()
    {
        if (flag) return;
        flag = true;
        dr1.SetActive(true);
    }

    public static void defeated()
    {
        if (flag) return;
        flag = true;
        def1.SetActive(true);
    }

    public static void dead()
    {
        de1.SetActive(true);
    }

    public static void victory()
    {
        if (flag) return;
        flag = true;
        vic1.SetActive(true);
    }
}
