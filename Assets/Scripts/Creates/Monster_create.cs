using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_create : MonoBehaviour
{
    public static List<Fix_vector2> pos_monster = new List<Fix_vector2>();
    public static List<Fix_vector2> size_monster = new List<Fix_vector2>();
    public static List<Fix_vector2> pos_zombies = new List<Fix_vector2>();
    public static List<Fix_vector2> size_zombies = new List<Fix_vector2>();

    public static void Mon_create()
    {
        for(int i = 0; i < pos_monster.Count; i++)
        {
            Obj_info p = new Obj_info();
            p.name = "Monster1";
            p.hei = size_monster[i].y.Clone();
            p.wid = size_monster[i].x.Clone();
            p.pos = pos_monster[i];
            p.col_type = Fix_col2d.col_status.Collider;
            p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
            p.classnames.Add(Object_ctrl.class_name.Moster);
            Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
            Main_ctrl.CreateObj(p);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
