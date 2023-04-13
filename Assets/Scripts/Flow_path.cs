using System.Collections.Generic;
using UnityEngine;

public class Flow_path : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<long, Facility> facilities = new Dictionary<long, Facility>();

    public static Fixpoint countdown = new Fixpoint(300, 0);

    private static int cnt_flag;

    public static int zombie_cnt = 0;

    public static long Now_fac = 0;

    public static void Updatex()
    {
        switch (cnt_flag)
        {
            case 0:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 1;
                    countdown = new Fixpoint(300, 0);
                    //召唤第一波僵尸
                    Monster_create.Zom_create1();
                }
                break;
            case 1:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0) || zombie_cnt == 0)
                {
                    cnt_flag = 2;
                    countdown = new Fixpoint(180, 0);
                }
                break;
            case 2:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 3;
                    countdown = new Fixpoint(180, 0);
                    //召唤第二波僵尸
                    Monster_create.Zom_create1();
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
}
