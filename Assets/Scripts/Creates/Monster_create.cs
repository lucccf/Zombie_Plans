using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_create : MonoBehaviour
{
    public static List<Fix_vector2> pos_monster = new List<Fix_vector2>();
    public static List<Fix_vector2> size_monster = new List<Fix_vector2>();
    public static List<Fix_vector2> pos_zombies = new List<Fix_vector2>();
    public static List<Fix_vector2> size_zombies = new List<Fix_vector2>();

    public static void Mon_create1()  //生成资源怪
    {
        for(int i = 0; i < pos_monster.Count; i++)
        {
            Obj_info p = new Obj_info();
            if (pos_monster[i].x < new Fixpoint(130, 0))
            {
                p.name = "Monster1";
            }
            else
            {
                p.name = "knight";
            }
            p.hei = size_monster[i].y.Clone();
            p.wid = size_monster[i].x.Clone();
            p.pos = pos_monster[i];
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
            p.name = "Monster1";
            p.hei = size_zombies[i].y.Clone();
            p.wid = size_zombies[i].x.Clone();
            p.pos = pos_zombies[i];
            p.col_type = Fix_col2d.col_status.Collider;
            p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
            p.classnames.Add(Object_ctrl.class_name.Moster);
            Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
            Main_ctrl.CreateObj(p);
            Flow_path.zombie_cnt++;
        }
    }
}
