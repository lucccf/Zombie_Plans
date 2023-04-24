using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;

public class Flow_path : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<long, Facility> facilities = new Dictionary<long, Facility>();

    public static Dictionary<long, Facility> onlyid_facilities = new Dictionary<long, Facility>();
    public const int cons = 5;
    public static int[] conditions = new int[cons];

    private static bool exit = false;

    public static void Exit()
    {
        exit = true;
    }

    public static Fixpoint countdown = new Fixpoint(10, 0);

    private static int cnt_flag = 0;

    public static int get_flag()
    {
        return cnt_flag;
    }

    private static int main_flag = 0;

    public static int zombie_cnt = 0;

    public static long Now_fac = 0;

    public static int[] times = new int[10];

    public static GameObject play_panel;
    public static GameObject death_panel;
    public static GameObject chat_panel;
    public static void init()
    {
        play_panel = GameObject.Find("PlayerPanel");
        death_panel = GameObject.Find("Death");
        chat_panel = GameObject.Find("ChatUI");
        death_panel.SetActive(false);
        chat_panel.SetActive(false);
        inittime();
        countdown = new Fixpoint(times[1], 0);
        cnt_flag = 0;
        zombie_cnt = 0;
        Now_fac = 0;
        main_flag = 0;
        conditions = new int[cons];
    }

    private static void inittime()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/time.xml");
        XmlNodeList T = xmlDoc.SelectNodes("/time/info");

        foreach(XmlNode xx in T)
        {
            times[int.Parse(xx.SelectSingleNode("id").InnerText)] = int.Parse(xx.SelectSingleNode("second").InnerText);
        }
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
                    countdown = new Fixpoint(times[2], 0);
                    //召唤第一波僵尸
                    Debug.Log("dierbo");
                    Monster_create.Zom_create1();
                }
                break;
            case 1:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0) || zombie_cnt == 0)
                {
                    cnt_flag = 2;
                    countdown = new Fixpoint(times[3], 0);
                    Map_create.Facility_create2();
                }
                break;
            case 2:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0))
                {
                    cnt_flag = 3;
                    countdown = new Fixpoint(times[4], 0);
                    //召唤第二波僵尸
                    Monster_create.Zom_create2();
                }
                break;
            case 3:
                countdown = countdown - Dt.dt;
                if (countdown <= new Fixpoint(0) || zombie_cnt == 0)
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
                Dead_panel.dead();
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

    public static void Dead_start()
    {
        play_panel.SetActive(false);
        death_panel.SetActive(true);
        int cnt1 = 0;
        foreach (var x in Player_ctrl.plays)
        {
            if (!x.CheckDeath())
            {
                cnt1++;
            }
        }
        if (cnt1 > 0)
        {
            Main_ctrl.main_id = (long)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
            while (Player_ctrl.plays[(int)Main_ctrl.main_id].CheckDeath())
            {
                Main_ctrl.main_id = (long)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
            }
            Main_ctrl.main_id = Player_ctrl.plays[(int)Main_ctrl.main_id].id;
        }
    }
    
    public static void next()
    {
        int cnt1 = 0;
        foreach (var x in Player_ctrl.plays)
        {
            if (!x.CheckDeath())
            {
                cnt1++;
            }
        }
        if (cnt1 > 0)
        {
            int p = Player_ctrl.plays.IndexOf((Player)Main_ctrl.All_objs[Main_ctrl.main_id].modules[Object_ctrl.class_name.Player]);
            p = (p + 1) % Player_ctrl.plays.Count;
            while (Player_ctrl.plays[p].CheckDeath())
            {
                p = (p + 1) % Player_ctrl.plays.Count;
            }
            Main_ctrl.main_id = Player_ctrl.plays[p].id;
        }
    }

    public static void pre()
    {
        int cnt1 = 0;
        foreach (var x in Player_ctrl.plays)
        {
            if (!x.CheckDeath())
            {
                cnt1++;
            }
        }
        if (cnt1 > 0)
        {
            int p = Player_ctrl.plays.IndexOf((Player)Main_ctrl.All_objs[Main_ctrl.main_id].modules[Object_ctrl.class_name.Player]);
            p = (p + Player_ctrl.plays.Count - 1) % Player_ctrl.plays.Count;
            while (Player_ctrl.plays[p].CheckDeath())
            {
                p = (p + Player_ctrl.plays.Count - 1) % Player_ctrl.plays.Count;
            }
            Main_ctrl.main_id = Player_ctrl.plays[p].id;
        }
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

        if (false)
        {
            //如果家的血量没了
            vic_wolf = true;
        }

        if (vic_wolf)
        {
            if (((Player)Main_ctrl.All_objs[Main_ctrl.Ser_to_cli[Main_ctrl.user_id]].modules[Object_ctrl.class_name.Player]).identity == Player.Identity.Wolf)
            {
                Dead_panel.victory();
            }
            else
            {
                Dead_panel.defeated();
            }
            //好人切换到好人失败场景，坏人切换到坏人胜利场景，进行对应的加分和扣分操作
        }
    }

    private static void Checkpeopvic()
    {
        if (cnt_flag < 4)
        {
            return;
        }
        bool flag_po = false;
        for(int i = 0; i < Player_ctrl.plays.Count; i++)
        {
            if (!Player_ctrl.plays[i].CheckDeath() && Player_ctrl.plays[i].identity == Player.Identity.Populace)
            {
                flag_po = true;
            }
        }
        for (int i = 0; i < Player_ctrl.plays.Count; i++)
        {
            if (Player_ctrl.plays[i].CheckDeath() && Player_ctrl.plays[i].identity == Player.Identity.Wolf)
            {
                flag_po = false;
            }
        }
        if (flag_po)
        {
            if (((Player)Main_ctrl.All_objs[Main_ctrl.Ser_to_cli[Main_ctrl.user_id]].modules[Object_ctrl.class_name.Player]).identity == Player.Identity.Wolf)
            {
                Dead_panel.defeated();
            }
            else
            {
                Dead_panel.victory();
            }
        }
        //如果有一名玩家成功逃出，则好人胜利，根据不同的胜利方式获得不同的评分
    }

    private static void Checkdoublefail()
    {
        bool flag_all = false;
        for (int i = 0; i < Player_ctrl.plays.Count; i++)
        {
            if (!Player_ctrl.plays[i].CheckDeath())
            {
                flag_all = true;
            }
        }
        if (!flag_all)
        {
            Dead_panel.draw();
        }
        //如果狼人死了且好人没有成功逃出，则算作平局
    }
}
