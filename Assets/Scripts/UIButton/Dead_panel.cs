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
        dr1 = dr;
        dr1.SetActive(false);
        def1 = def;
        def1.SetActive(false);
        de1 = de;
        de1.SetActive(false);
        vic1 = vic;
        vic1.SetActive(false);
    }

    public void deadstart()
    {
        Flow_path.Dead_start();
    }

    public static void draw()
    {
        if (flag) return;
        flag = true;
        ClientSend.Send = false;
        dr1.SetActive(true);
    }

    public static void defeated()
    {
        if (flag) return;
        flag = true;
        ClientSend.Send = false;
        def1.SetActive(true);
    }

    public static void dead()
    {
        ClientSend.Send = false;
        de1.SetActive(true);
    }

    public static void victory()
    {
        if (flag) return;
        flag = true;
        ClientSend.Send = false;
        vic1.SetActive(true);
    }
}
