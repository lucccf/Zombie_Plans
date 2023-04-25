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
    public Fix_vector2 with_pos;
    public int type;
    public int attacker_type;
    public int hited_fly_type;
    public string MusicName;

    protected Fixpoint AliveTime = new Fixpoint(0, 0);

    AudioSource audiosource;
    public void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        //Debug.Log("Music " + Time.time + " " + MusicName);
        if (MusicName == "") return;
        audiosource.Stop();
        audiosource.clip = (AudioClip)AB.getobj(MusicName);
        audiosource.Play();
    }

    public virtual void Updatex()
    {
        AliveTime += Dt.dt;
        //Debug.Log(with_attacker);
        if(with_attacker == true)
        {
            //Debug.Log("x:" + with_pos.x.to_float());
            //Debug.Log("y:" + with_pos.y.to_float());
            if (attacker_type == 1)
            {
                BasicCharacter p = (BasicCharacter)(Main_ctrl.All_objs[attakcer_id].modules[Object_ctrl.class_name.Moster]);
                if (p.AnimaToward > 0) f.pos.x = p.f.pos.x + with_pos.x;
                else f.pos.x = p.f.pos.x - with_pos.x;
                f.pos.y = p.f.pos.y + with_pos.y;
                transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float());
            } else
            {
                BasicCharacter p = (BasicCharacter)(Main_ctrl.All_objs[attakcer_id].modules[Object_ctrl.class_name.Player]);
                if (p.AnimaToward > 0) f.pos.x = p.f.pos.x + with_pos.x;
                else f.pos.x = p.f.pos.x - with_pos.x;
                f.pos.y = p.f.pos.y + with_pos.y;
                transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float());
            }
        }
        if(AliveTime > new Fixpoint(3,1))
        {
            Main_ctrl.Desobj(id);
        }
    }
}
