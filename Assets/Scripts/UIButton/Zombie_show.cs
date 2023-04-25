using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_show : MonoBehaviour
{
    public GameObject g1, g2;
    int flag = 0;
    float speed = 5;
    float p;
    // Start is called before the first frame update
    void Start()
    {
        g1.SetActive(false);
        g2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (flag)
        {
            case 0:
                if (Flow_path.get_flag() == 0 && Flow_path.countdown <= new Fixpoint(10, 0))
                {
                    flag = 1;
                }
                break;
            case 1:
                g1.SetActive(true);
                g1.transform.localScale = new Vector3(5, 5, 1);
                flag = 2;
                break;
            case 2:
                while(false)
                {

                }
                break;
        }
    }
}
