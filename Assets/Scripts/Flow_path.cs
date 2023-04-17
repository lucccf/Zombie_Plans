using System.Collections.Generic;
using UnityEngine;

public class Flow_path : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<long, Facility> facilities = new Dictionary<long, Facility>();

    public const int cons = 5;
    public static int[] conditions = new int[cons];

    private static bool exit = false;

    public static void Exit()
    {
        exit = true;
    }

    public static Fixpoint countdown = new Fixpoint(300, 0);

    private static int cnt_flag = 0;

    private static int main_flag = 0;

    public static int zombie_cnt = 0;

    public static long Now_fac = 0;

    public static GameObject play_panel;
    public static GameObject death_panel;

    public static void init()
    {
        play_panel = GameObject.Find("PlayerPanel");
        death_panel = GameObject.Find("Death");
        death_panel.SetActive(false);
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
                    Map_create.Facility_create2();
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

        switch (main_flag)
        {
            case 0:
                if (((Player)Main_ctrl.All_objs[Main_ctrl.Ser_to_cli[Main_ctrl.user_id]].modules[Object_ctrl.class_name.Player]).CheckDeath())
                {
                    main_flag = 1;
                }
                break;
            case 1:
                //切换到死亡UI，并随机选择一名玩家作为主视角
                play_panel.SetActive(false);
                death_panel.SetActive(true);
                int cnt1 = 0;
                foreach(var x in Player_ctrl.plays)
                {
                    if (!x.CheckDeath())
                    {
                        cnt1++;
                    }
                }
                Debug.Log(Main_ctrl.main_id);
                if (cnt1 > 0)
                {
                    Main_ctrl.main_id = (long)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
                    while (Player_ctrl.plays[(int)Main_ctrl.main_id].CheckDeath())
                    {
                        Main_ctrl.main_id = (long)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
                    }
                }
                Main_ctrl.main_id = Player_ctrl.plays[(int)Main_ctrl.main_id].id;
                Debug.Log(Main_ctrl.main_id);
                Debug.Log(Main_ctrl.All_objs[Main_ctrl.main_id].gameObject);
                main_flag = 2;
                break;
            case 2:
                if (exit)
                {
                    //切换到退出场景
                }
                break;
        }

        Checkwolfvic();

        Checkpeopvic();

        Checkdoublefail();
    }

    private static void Checkwolfvic()
    {
        bool vic_wolf = true;
        foreach (var x in Player_ctrl.plays)
        {
            if (x.identity == Player.Identity.Populace && !x.CheckDeath())
            {
                vic_wolf = false;
            }
        }

        if (true)
        {
            //如果家的血量没了
            vic_wolf = false;
        }

        if (vic_wolf)
        {
            //好人切换到好人失败场景，坏人切换到坏人胜利场景，进行对应的加分和扣分操作
        }
    }

    private static void Checkpeopvic()
    {
        //如果有一名玩家成功逃出，则好人胜利，根据不同的胜利方式获得不同的评分
    }

    private static void Checkdoublefail()
    {
        //如果狼人死了且好人没有成功逃出，则算作平局
    }
}
