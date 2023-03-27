using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moster_create : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Obj_info p = new Obj_info();
        p.name = "Moster1";
        p.hei = new Fixpoint(2, 0);
        p.wid = new Fixpoint(2, 0);
        p.pos = new Fix_vector2(new Fixpoint(1 * 7 * 5, 1), new Fixpoint(-1 * 7 * 5, 1));
        p.col_type = Fix_col2d.col_status.Collider;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Moster);
        Main_ctrl.CreateObj(p);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
