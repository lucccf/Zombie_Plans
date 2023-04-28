using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infomation : MonoBehaviour
{
    public GameObject Info;
    public Text txt;
    int p = 0;
    public static string w1, w2, p1, p2;

    // Start is called before the first frame update
    void Start()
    {
        if (Main_ctrl.main_identity == Player.Identity.Populace)
        {
            txt.text = p1;
        }
        else
        {
            txt.text = w1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (txt.text.Length == 0)
        {
            if (Main_ctrl.main_identity == Player.Identity.Populace)
            {
                txt.text = p1;
            }
            else
            {
                txt.text = w1;
            }
        }
        if (Flow_path.get_flag() == 2 && p != 2)
        {
            if (Main_ctrl.main_identity == Player.Identity.Populace)
            {
                txt.text = p2;
            }
            else
            {
                txt.text = w2;
            }
        }
        p = Flow_path.get_flag();
    }

    public void open()
    {
        if (Info.activeSelf)
        {
            Info.SetActive(false);
        }
        else
        {
            Info.SetActive(true);
        }
    }
}
