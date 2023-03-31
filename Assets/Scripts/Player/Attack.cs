using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public long id;
    public Fix_col2d f;
    public Fixpoint HpDamage;
    public int ToughnessDamage;
    public long attakcer_id = 0;
    public float toward;
    public bool with_attacker = false;

    private Fixpoint AliveTime = new Fixpoint(0, 0); 

    public void Updatex()
    {
        AliveTime += Dt.dt;
        //Debug.Log(with_attacker);
        if(with_attacker == true)
        {
            Player p =(Player)(Main_ctrl.All_objs[attakcer_id].modules[Object_ctrl.class_name.Player]);
            if (p.AnimaToward > 0) f.pos.x = p.f.pos.x + new Fixpoint(1, 0);
            else f.pos.x = p.f.pos.x - new Fixpoint(1, 0);
            f.pos.y = p.f.pos.y;
            transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float());
        }
        if(AliveTime > new Fixpoint(3,1))
        {
            Main_ctrl.Desobj(id);
        }
    }
}
