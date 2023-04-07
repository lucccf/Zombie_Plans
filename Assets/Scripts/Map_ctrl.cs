using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_ctrl : MonoBehaviour
{
    public static Dictionary<long, GameObject> Map_items = new Dictionary<long, GameObject>();

    public static void Updatex()
    {
        foreach(var x in Map_items)
        {
            Vector3 q = Main_ctrl.All_objs[x.Key].gameObject.transform.position;
            x.Value.transform.position = new Vector3(Tiny_map_cre.pos_x + q.x / 3, Tiny_map_cre.pos_y + q.y / 3, 0);
        }
    }
}
