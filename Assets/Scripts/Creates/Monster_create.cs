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
    public static List<Mon_pos> pos_zombies = new List<Mon_pos>();
    public static List<Fix_vector2> size_zombies = new List<Fix_vector2>();
    public static float cnt1 = 0;

    public static void Mon_create1()  //生成资源怪
    {
        for(int i = 0; i < pos_monster.Count; i++)
        {
            Obj_info p = new Obj_info();
            switch (pos_monster[i].type)
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
            /*
            if (pos_monster[i].x < new Fixpoint(130, 0))
            {
                if (Rand.rand() % 3 == 0)
                {
                    p.name = "Mage";
                }
                else if(Rand.rand()%2 == 0){ 
                    p.name = "Devil";
                } else
                {
                    p.name = "Terrorist";
                }
            }
            else
            {
                if (Rand.rand() % 2 == 0)
                {
                    p.name = "knight";
                } else
                {
                    p.name = "Monster1";
                }
            }
            */
            p.hei = size_monster[i].y.Clone();
            p.wid = size_monster[i].x.Clone();
            p.pos = pos_monster[i].pos;
            cnt1 += p.pos.x.to_int() + p.pos.y.to_int();
            p.col_type = Fix_col2d.col_status.Collider;
            p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
            p.classnames.Add(Object_ctrl.class_name.Moster);
            p.classnames.Add(Object_ctrl.class_name.Tinymap);
            //Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
            Main_ctrl.CreateObj(p);
        }
    }

    public static void Zom_create1()  //生成僵尸，后期替换模型和ai
    {
        for (int i = 0; i < pos_zombies.Count; i++)
        {
            Obj_info p = new Obj_info();
            //p.name = "Monster1";
            p.name = "Devil";
            p.hei = size_zombies[i].y.Clone();
            p.wid = size_zombies[i].x.Clone();
            p.pos = pos_zombies[i].pos;
            p.col_type = Fix_col2d.col_status.Collider;
            p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
            p.classnames.Add(Object_ctrl.class_name.Moster);
            Main_ctrl.CreateObj(p);
            Flow_path.zombie_cnt++;
        }
    }
}
