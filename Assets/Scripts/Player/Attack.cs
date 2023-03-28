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

    private Fixpoint AliveTime = new Fixpoint(0, 0); 

    public void Updatex()
    {
        AliveTime += Dt.dt;
        if(AliveTime > new Fixpoint(3,1))
        {
            Main_ctrl.Desobj(id);
        }
    }
}
