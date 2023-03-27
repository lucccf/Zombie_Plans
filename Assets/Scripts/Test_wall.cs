using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_wall : MonoBehaviour
{
    // Start is called before the first frame update
    public long id;
    Fix_col2d f;
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.bounds.size;

        //f = GetComponent<Fix_col2d>();
        f = (Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d];
        f.hei = new Fixpoint(spriteSize.y);
        f.wid = new Fixpoint(spriteSize.x);
        f.type = Fix_col2d.col_status.Wall;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
