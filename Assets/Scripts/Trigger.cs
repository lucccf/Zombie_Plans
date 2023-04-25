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
    public Fixpoint AilveTime = new Fixpoint(0,0);
    public long id;
    public void Startx()
    {
        Fixpoint AilveTime = new Fixpoint(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explore()
    {
        Debug.Log("Explore" + AilveTime.to_float());
        if(AilveTime < new Fixpoint(3,0))
        {
            return;
        }
        Main_ctrl.NewAttack(f.pos, new Fix_vector2(0, 0), new Fixpoint(6, 0), new Fixpoint(6, 0), new Fixpoint(100,0), 120, id, 1f, false, 2, 3, "");//最后一个参数是击飞类型
        Main_ctrl.Desobj(id);
        GameObject obj = Instantiate((GameObject)AB.getobj("Bomb2"));
        Instantiate(obj, transform.position + new Vector3(0,1.1f,0), transform.rotation);
    }

    public void BoxTriggerIn()
    {
        GetComponent<WolfBoxInMap>().TriggerIn();
    }

    public void BoxTriggerOut()
    {
        GetComponent<WolfBoxInMap>().Triggerout();
    }

    public void Updatex()
    {
        AilveTime += Dt.dt;
        if(triggername == "ItemSample")
        {
            if(f.onground)
            {
                r.velocity = new Fix_vector2(0, 0);
            }
        }
        //transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
}
