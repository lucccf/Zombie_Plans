using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mon_pos
{
    public int type;
    public Fix_vector2 pos;

    public Mon_pos(int type, Fix_vector2 pos)
    {
        this.type = type;
        this.pos = pos;
    }
}

public class Monster_create : MonoBehaviour
{
    public static List<Mon_pos> pos_monster = new List<Mon_pos>();
    public static List<Fix_vector2> size_monster = new List<Fix_vector2>();
    public static List<Mon_pos> pos_zombies1 = new List<Mon_pos>();
    public static List<Fix_vector2> size_zombies1 = new List<Fix_vector2>();
    public static List<Mon_pos> pos_zombies2 = new List<Mon_pos>();
    public static List<Fix_vector2> size_zombies2 = new List<Fix_vector2>();
    public static float cnt1 = 0;

    public static void Mon_create1()  //生成资源怪
    {
        for(int i = 0; i < pos_monster.Count; i++)
        {
            create(pos_monster[i], size_monster[i], 0);
        }
    }

    private static void create(Mon_pos p1, Fix_vector2 p2, int type2)
    {
        Obj_info p = new Obj_info();
        switch (p1.type)
        {
            case 1:
                p.name = "Monster1";
                break;
            case 2:
                p.name = "Mage";
                break;
            case 3:
                p.name = "knight";
                break;
            case 4:
                p.name = "Devil";
                break;
            case 5:
                p.name = "Terrorist";
                break;
        }
        p.cre_type = type2;
        p.hei = p2.y.Clone();
        p.wid = p2.x.Clone();
        p.pos = p1.pos;
        p.col_type = Fix_col2d.col_status.Collider;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Moster);
        p.classnames.Add(Object_ctrl.class_name.Tinymap);
        //Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
        Main_ctrl.CreateObj(p);
    }

    public static void Zom_create1()  //生成僵尸，后期替换模型和ai
    {
        for (int i = 0; i < pos_zombies1.Count; i++)
        {
            create(pos_zombies1[i], size_zombies1[i], 1);
        }
    }

    public static void Zom_create2()  //生成僵尸，后期替换模型和ai
    {
        for (int i = 0; i < pos_zombies2.Count; i++)
        {
            create(pos_zombies2[i], size_zombies2[i], 1);
        }
    }
}
