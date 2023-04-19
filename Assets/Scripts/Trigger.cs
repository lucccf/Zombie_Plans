using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string triggertype;
    public string triggername;
    public int itemid;
    public int itemnum;
    public Fix_col2d f;
    public Fix_rig2d r;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Updatex()
    {
        if(triggername == "ItemSample")
        {
            if(f.onground)
            {
                r.velocity = new Fix_vector2(0, 0);
            }
        }
    }
}
