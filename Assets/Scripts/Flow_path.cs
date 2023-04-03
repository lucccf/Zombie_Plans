using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Flow_path : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<long,Facility> facilities = new Dictionary<long, Facility>();

    public static Fixpoint countdown = new Fixpoint(300, 0);

    private static int cnt_flag;

    public static int zombie_cnt = 0;

    void Start()
    {
        
    }

    public static void Updatex()
    {
        switch (cnt_flag)
        {
            case 0:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 1;
                    countdown = new Fixpoint(300);
                    //召唤僵尸
                }
                break;
            case 1:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0) || zombie_cnt == 0)
                {
                    cnt_flag = 2;
                    countdown = new Fixpoint(180);
                }
                break;
            case 2:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 3;
                    countdown = new Fixpoint(180);
                    //召唤第二波僵尸
                }
                break;
            case 3:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 4;
                }
                break;
            case 4:
                countdown = countdown + Dt.dt;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
