using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fix_col2d_act;

public class Collider_ctrl
{
    public static List<Fix_col2d> cols = new List<Fix_col2d>();

    class Id_cmp : IComparer<int>
    {
        public int Compare(int id1, int id2)
        {
            long cmp = cols[id1].left().val() - cols[id2].left().val();
            if (cmp > 0)
            {
                return 1;
            }
            else if (cmp < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    class Id_cmp2 : IComparer<int>
    {
        public int Compare(int id1, int id2)
        {
            long cmp = cols[id1].right().val() - cols[id2].right().val();
            if (cmp < 0)
            {
                return 1;
            }
            else if (cmp > 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public static void Update_collison()
    {
        List<int> lst = new List<int>();
        for (int i = 0; i < cols.Count; i++)
        {
            lst.Add(i);
        }
        lst.Sort(new Id_cmp());
        List<int> leftside = new List<int>();
        for (int i = 0; i < cols.Count; i++)
        {
            cols[lst[i]].onground = false;
            if (leftside.Count == 0)
            {
                leftside.Add(lst[i]);
                continue;
            }

            //int k = st.Peek();
            int k = leftside[leftside.Count - 1];
            while (cols[k].right() <= cols[lst[i]].left())
            {
                leftside.RemoveAt(leftside.Count - 1);
                if (leftside.Count == 0) break;
                k = leftside[leftside.Count - 1];
            }
            if (leftside.Count == 0)
            {
                leftside.Add(lst[i]);
                continue;
            }

            for (int j = 0; j < leftside.Count; j++)
            {
                if (cols[lst[i]].up() > cols[leftside[j]].down() && cols[lst[i]].down() < cols[leftside[j]].up())
                {
                    on_collision(cols[lst[i]], cols[leftside[j]]);
                }
            }
            leftside.Add(lst[i]);
            leftside.Sort(new Id_cmp2());
        }
        for (int i = 0; i < cols.Count; i++)
        {
            if (cols[lst[i]].type == Fix_col2d.col_status.Trigger)
            {
                List<long> buf = new List<long>();
                foreach (long key in cols[lst[i]].conditions.Keys)
                {
                    if (cols[lst[i]].conditions[key] > 0)
                    {
                        if (cols[lst[i]].conditions[key] == 1)
                        {
                            if (!Main_ctrl.All_objs.ContainsKey(key)) continue;
                            cols[lst[i]].actions.Enqueue(new Fix_col2d_act(col_action.Trigger_out, (Fix_col2d)Main_ctrl.All_objs[key].modules[Object_ctrl.class_name.Fix_col2d]));

                            ((Fix_col2d)Main_ctrl.All_objs[key].modules[Object_ctrl.class_name.Fix_col2d]).actions.Enqueue(new Fix_col2d_act(col_action.Trigger_out, cols[lst[i]]));
                        }
                        //cols[lst[i]].conditions[key]--;
                        buf.Add(key);
                    }
                }
                for(int j = 0; j < buf.Count; j++)
                {
                    cols[lst[i]].conditions[buf[j]]--;
                }
            }
        }
    }

    static void on_collision(Fix_col2d f1, Fix_col2d f2)
    {

        //碰撞事件
        if (f1.type == Fix_col2d.col_status.Wall && (f2.type == Fix_col2d.col_status.Collider || f2.type == Fix_col2d.col_status.Attack2))
        {
            if (f2.type == Fix_col2d.col_status.Attack || f2.type == Fix_col2d.col_status.Wall) return;
            col_wall(f2, f1);
            return;
        }
        if (f2.type == Fix_col2d.col_status.Wall && (f1.type == Fix_col2d.col_status.Collider || f1.type == Fix_col2d.col_status.Attack2))
        {
            if (f1.type == Fix_col2d.col_status.Attack) return;
            col_wall(f1, f2);
            return;
        }

        if ((f1.type == Fix_col2d.col_status.Attack || f1.type == Fix_col2d.col_status.Attack2) && f2.type == Fix_col2d.col_status.Collider)
        {
            col_attack(f2, f1);
            return;
        }
        if ((f2.type == Fix_col2d.col_status.Attack || f2.type == Fix_col2d.col_status.Attack2) && f1.type == Fix_col2d.col_status.Collider)
        {
            col_attack(f1, f2);
            return;
        }

        //其他情况的交互
        if (f1.type == Fix_col2d.col_status.Trigger && f2.type == Fix_col2d.col_status.Collider)
        {
            col_trigger(f2, f1);
        }
        if (f2.type == Fix_col2d.col_status.Trigger && f1.type == Fix_col2d.col_status.Collider)
        {
            col_trigger(f1, f2);
        }
    }

    static void col_wall(Fix_col2d col, Fix_col2d wall)
    {
        if (col.up() > wall.up())
        {
            if (col.right() > wall.right())
            {
                if (wall.up() - col.down() > wall.right() - col.left())
                {
                    col.pos.x = col.pos.x - col.left() + wall.right();
                    Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Left);
                    return;
                }
            }

            if (col.left() < wall.left())
            {
                if (wall.up() - col.down() > col.right() - wall.left())
                {
                    col.pos.x = col.pos.x - col.right() + wall.left();
                    Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Right);
                    return;
                }
            }

            col.onground = true;
            col.pos.y = col.pos.y - col.down() + wall.up();
            Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Down);
            return;
        }

        if (col.down() < wall.down())
        {
            if (col.right() > wall.right())
            {
                if (col.up() - wall.down() > wall.right() - col.left())
                {
                    col.pos.x = col.pos.x - col.left() + wall.right();
                    Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Left);
                    return;
                }
            }

            if (col.left() < wall.left())
            {
                if (col.up() - wall.down() > col.right() - wall.left())
                {
                    col.pos.x = col.pos.x - col.right() + wall.left();
                    Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Right);
                    return;
                }
            }

            col.pos.y = col.pos.y - col.up() + wall.down();
            Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Up);
            return;
        }

        if (col.right() > wall.right())
        {
            col.pos.x = col.pos.x - col.left() + wall.right();
            Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Left);
            return;
        }

        if (col.left() < wall.left())
        {
            col.pos.x = col.pos.x - col.right() + wall.left();
            Rigid_ctrl.col_wall(col.id, Rigid_ctrl.side.Right);
            return;
        }
    }

    static void col_attack(Fix_col2d col, Fix_col2d attack)
    {
        if (!attack.conditions.ContainsKey(col.id))
        {
            attack.conditions[col.id] = 1;
            //发送受击事件
            col.actions.Enqueue(new Fix_col2d_act(col_action.Attack, attack));
        }
    }

    static void col_trigger(Fix_col2d col, Fix_col2d tri)
    {
        if (!tri.conditions.ContainsKey(col.id))
        {
            tri.conditions[col.id] = 2;
            //发送受击事件
            col.actions.Enqueue(new Fix_col2d_act(col_action.Trigger_in, tri));
        }
        else
        {
            if (tri.conditions[col.id] == 0)
            {
                tri.conditions[col.id] = 2;
                //发送受击事件
                col.actions.Enqueue(new Fix_col2d_act(col_action.Trigger_in, tri));
            }
            else
            {
                tri.conditions[col.id] = 2;
            }
        }
    }
}
