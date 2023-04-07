using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny_map_cre : MonoBehaviour
{
    public const int pos_x = -1000;
    public const int pos_y = 1000;
    public static GameObject Create_tiny(Obj_info info)
    {
        GameObject obj = Instantiate((GameObject)AB.getobj(info.name + "_m"));
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(info.wid.to_float() / 3, info.hei.to_float() / 3);
        obj.transform.position = new Vector3(pos_x + info.pos.x.to_float() / 3, pos_y + info.pos.y.to_float() / 3, 0);
        return obj;
    }
}
