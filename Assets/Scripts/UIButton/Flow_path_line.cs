using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow_path_line : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    int p = 0;
    void Update()
    {
        if (p != Flow_path.get_flag())
        {
            p = Flow_path.get_flag();
            if (p == 1)
            {
                p1.SetActive(true);
            }
            else if (p == 2)
            {
                p2.SetActive(true);
            }
            else if (p == 3)
            {
                p3.SetActive(true);
            }
            else if (p == 4)
            {
                p4.SetActive(true);
            }
        }
    }
}
