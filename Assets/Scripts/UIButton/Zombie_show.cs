using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zombie_show : MonoBehaviour
{
    public GameObject g1, g2;
    int flag = 0;
    int flag2 = 0;
    float speed = 5;
    float speed_c = 300;
    float p = 0;
    float a = 255;
    Image i1, i2;
    // Start is called before the first frame update
    void Start()
    {
        i1 = g1.GetComponent<Image>();
        i2 = g2.GetComponent<Image>();
        g1.SetActive(false);
        g2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (flag)
        {
            case 0:
                if ((Flow_path.get_flag() == 0 || Flow_path.get_flag() == 2) && Flow_path.countdown <= new Fixpoint(10, 0))
                {
                    flag = 1;
                }
                break;
            case 1:
                g1.SetActive(true);
                p = 5;
                g1.transform.localScale = new Vector3(p, p, 1);
                i1.color = new Color(1, 1, 1, 1);
                flag = 2;
                break;
            case 2:
                p = p - speed * Time.deltaTime;
                g1.transform.localScale = new Vector3(p, p, 1);
                if (p <= 1)
                {
                    a = 255;
                    flag = 3;
                }
                break;
            case 3:
                a = a - speed_c * Time.deltaTime;
                if (a < 0)
                {
                    flag = 4;
                }
                i1.color = new Color(1, 1, 1, a / 255);
                break;
            case 4:
                g1.SetActive(false);
                flag = 5;
                break;
            case 5:
                if (Flow_path.get_flag() == 1)
                {
                    flag = 0;
                }
                break;
        }
        switch (flag2)
        {
            case 0:
                if (Flow_path.get_flag() == 3)
                {
                    flag2 = 1;
                }
                break;
            case 1:
                g2.SetActive(true);
                p = 5;
                flag2 = 2;
                break;
            case 2:
                p = p - speed * Time.deltaTime;
                g2.transform.localScale = new Vector3(p, p, 1);
                if (p <= 1)
                {
                    a = 255;
                    flag2 = 3;
                }
                break;
            case 3:
                a = a - speed_c * Time.deltaTime;
                if (a < 0)
                {
                    flag2 = 4;
                }
                i2.color = new Color(1, 1, 1, a / 255);
                break;
            case 4:
                g2.SetActive(false);
                flag2 = 5;
                break;
        }
    }
}
