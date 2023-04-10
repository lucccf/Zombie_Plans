using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using static Rigid_ctrl;

public class BasicCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    protected PlayerStatus status = new PlayerStatus(100,10);
    //碰撞体变量
    public Fix_col2d f;
    public Fix_rig2d r;
    public long id;

    protected Queue<Fix_col2d_act> AttackQueue = new Queue<Fix_col2d_act>();
    protected Queue<Fix_col2d_act> TriggerQueue = new Queue<Fix_col2d_act>();

    protected int AnimaStatus = 0;
    protected StatusType RealStatus;
    protected Fixpoint StatusTime = new Fixpoint(0, 0);

    protected int CharacterType = 0;
    protected enum StatusType
    {
        Normal,
        Attack,
        LittleJump,
        Jump,
        Death,
        Fall,
        Hit,
        Ground,
        Recover,
        Appear,
        Disappear,
        Defence,
        Skill,
        Search,
        Fire
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SetStatus(int hp,int attack)
    {
        status = new PlayerStatus(hp, attack);
    }

    public virtual void Updatex()
    {

    }

    protected void ChangeStatus(int animastatus)
    {
        AnimaStatus = animastatus;
        StatusTime = new Fixpoint(0, 0);
    }
    protected void ChangeStatus(StatusType realstatus)
    {
        RealStatus = realstatus;
        StatusTime = new Fixpoint(0, 0);
    }
    protected void Moves(float toward,Fixpoint speed)
    {
        if (toward < 0)
        {
            f.pos.x -= speed * Dt.dt;
        }
        else
        {
            f.pos.x += speed * Dt.dt;
        }
    }
    
    protected void CreateAttack(Fix_vector2 pos, Fixpoint wide, Fixpoint high, Fixpoint HpDamage, int toughness, float Toward)
    {
        Main_ctrl.NewAttack(pos, new Fix_vector2(0, 0), wide, high, HpDamage, toughness, id, Toward , false,CharacterType);
    }

    protected void CreateAttackWithCharacter(Fix_vector2 pos , Fix_vector2 with_pos, Fixpoint wide, Fixpoint high, Fixpoint HpDamage, int toughness, float Toward)
    {
        Main_ctrl.NewAttack(pos, with_pos, wide, high, HpDamage, toughness, id, Toward, true, CharacterType);
    }

    protected void GetColider()
    {
        if (((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Count > 0)
        {
            Fix_col2d_act a = ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
            if (a.type == Fix_col2d_act.col_action.Trigger_in || a.type == Fix_col2d_act.col_action.Trigger_out)
            {
                TriggerQueue.Enqueue(a);
            }
            else if (a.type == Fix_col2d_act.col_action.Attack)
            {
                AttackQueue.Enqueue(a);
            }
        }
    }

    private void Preform(int damage)
    {
        GameObject beat = (GameObject)AB.getobj("beat");
        beat.transform.localScale = new Vector3(3f, 3f, 1f);
        Instantiate(beat, transform.position, transform.rotation);
        GameObject num = (GameObject)AB.getobj("HurtNumber");
        GameObject num2 = Instantiate(num, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        num2.GetComponent<BeatNumber>().ChangeNumber(damage);
    }

    protected bool GetHited(ref float Toward)
    {

        GetColider();

        bool this_hited = false;
        while (AttackQueue.Count > 0)
        {
            Fix_col2d_act a = AttackQueue.Peek();
            AttackQueue.Dequeue();
            if (a.type != Fix_col2d_act.col_action.Attack)
            {
                Debug.Log("Attack Geted Error");
                continue;
            }
            long AttackId = a.opsite.id;
            if (!Main_ctrl.All_objs.ContainsKey(AttackId)) continue;
            Attack attack = (Attack)(Main_ctrl.All_objs[AttackId].modules[Object_ctrl.class_name.Attack]);
            if (attack.attacker_type == CharacterType) continue;
            
            Toward = -attack.toward;
            this_hited = true;



            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;
            status.GetAttacked(HpDamage, ToughnessDamage);

            Preform(status.last_damage);

            //Debug.Log(HpDamage + " " + ToughnessDamage);

            if (attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                attack2.DestroySelf();
            }
        }
        return this_hited;
    }
    protected void RemoveHited()
    {
        GetColider();

        if (AttackQueue.Count > 0)
        {
            AttackQueue.Dequeue();
        }
    }

    protected void DeathFall(string name,int num,float size)
    {
        Main_ctrl.NewItem(f.pos.Clone(), name, num, size);
    }

    public float CheckHealth()
    {
        return 1f * status.hp / status.max_hp;
    }
}
