using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigid_ctrl
{
    public static List<Fix_rig2d> rigs = new List<Fix_rig2d>();

    public enum side
    {
        Up,
        Down,
        Left,
        Right,
    }

    public static void col_wall(long id, side si)
    {
        if (!Main_ctrl.All_objs[id].modules.ContainsKey(Object_ctrl.class_name.Fix_rig2d))
        {
            return;
        }
        Fix_rig2d x = (Fix_rig2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_rig2d];
        if (si == side.Up || si == side.Down)
        {
            x.velocity.y = new Fixpoint(0);
        }

        if (si == side.Left || si == side.Right)
        {
            x.velocity.x = new Fixpoint(0);
        }
    }

    public static void rig_update()
    {
        for(int i = 0; i < rigs.Count; i++)
        {
            Fix_col2d f = (Fix_col2d)Main_ctrl.All_objs[rigs[i].id].modules[Object_ctrl.class_name.Fix_col2d];
            rigs[i].velocity = rigs[i].velocity + Dt.dt * rigs[i].gravity;
            f.pos = f.pos + Dt.dt * rigs[i].velocity;
        }
    }
}
