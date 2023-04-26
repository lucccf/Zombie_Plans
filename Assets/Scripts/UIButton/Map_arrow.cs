using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Map_arrow : MonoBehaviour
{
    public GameObject home;
    public GameObject taliban;
    public GameObject map_bk;
    // Start is called before the first frame update
    void Start()
    {
        home.SetActive(false);
        taliban.SetActive(false);
    }

    int k = 15;
    bool inside(GameObject obj)
    {
        if (obj == null) return true;
        bool flag = true;
        Vector2 t1 = obj.transform.position;
        Vector2 t2 = Main_ctrl.camara.transform.position;
        if (t1.x - t2.x > 2 * k) flag = false;
        if (t2.x - t1.x > 2 * k) flag = false;
        if (t1.y - t2.y > k) flag = false;
        if (t2.y - t1.y > k) flag = false;
        return flag;
    }

    Vector3 getpos(GameObject obj)
    {
        if (obj == null) return new Vector3(0, 0, 0);
        Vector3 pos = obj.transform.position;
        Vector3 p2 = Main_ctrl.camara.transform.position;
        Vector3 p3 = pos - p2;
        if (p3.x >= 0)
        {
            if (p3.x > 2 * k)
            {
                p3 = p3 * (2 * k / p3.x);
            }
        }
        else
        {
            if (p3.x < -2 * k)
            {
                p3 = p3 * (-2 * k / p3.x);
            }
        }
        if (p3.y >= 0)
        {
            if (p3.y > k)
            {
                p3 = p3 * (k / p3.y);
            }
        }
        else
        {
            if (p3.y < -k)
            {
                p3 = p3 * (-k / p3.y);
            }
        }
        p3.z = 0;
        p3 = p3 * 6.2f;
        return p3;
    }

    Vector3 dz(Vector3 x)
    {
        return new Vector3(x.x, x.y, 0);
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
            //Vector2 xx = Main_ctrl.camara.transform.position - Main_ctrl.taliban.transform.position;
            taliban.transform.localPosition = getpos(Main_ctrl.taliban);
            taliban.transform.rotation = Quaternion.FromToRotation(dz(Main_ctrl.camara.transform.position), dz(Main_ctrl.taliban.transform.position));
            //taliban.transform.rotation.SetEulerAngles(0, 0, math.acos(xx.x, xx.));
        }
        if (home.activeSelf)
        {
            home.transform.localPosition = getpos(Main_ctrl.home);
            home.transform.rotation = Quaternion.FromToRotation(dz(Main_ctrl.camara.transform.position), dz(Main_ctrl.home.transform.position));
        }
    }
}
