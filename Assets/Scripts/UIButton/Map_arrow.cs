using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_arrow : MonoBehaviour
{
    public GameObject home, taliban;
    // Start is called before the first frame update
    void Start()
    {
        home.SetActive(false);
        taliban.SetActive(false);
    }

    bool inside(GameObject obj)
    {
        if (obj == null) return true;
        int k = 15;
        bool flag = true;
        Vector2 t1 = obj.transform.position;
        Vector2 t2 = Main_ctrl.camara.transform.position;
        if (t1.x - t2.x > 2 * k) flag = false;
        if (t2.x - t1.x > 2 * k) flag = false;
        if (t1.y - t2.y > k) flag = false;
        if (t2.y - t1.y > k) flag = false;
        return flag;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inside(Main_ctrl.taliban) && !taliban.activeSelf) {
            taliban.SetActive(true);
        }
        if (inside(Main_ctrl.taliban) && taliban.activeSelf)
        {
            taliban.SetActive(false);
        }
        if (!inside(Main_ctrl.home) && !home.activeSelf)
        {
            home.SetActive(true);
        }
        if (inside(Main_ctrl.home) && home.activeSelf)
        {
            home.SetActive(false);
        }
        if (taliban.activeSelf)
        {
            
        }
    }
}
