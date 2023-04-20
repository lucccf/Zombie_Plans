using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    protected PlayerStatus status = new PlayerStatus(100,10);
    //碰撞体变量
    public Fix_col2d f;
    public Fix_rig2d r;
    public long id;

    public int type2 = 0;

    protected Queue<Fix_col2d_act> AttackQueue = new Queue<Fix_col2d_act>();
    protected Queue<Fix_col2d_act> TriggerQueue = new Queue<Fix_col2d_act>();

    protected Animator animator;
    protected int AnimaStatus = 0;
    public float AnimaToward = 0;
    protected int AnimaHited = 0;
    protected StatusType RealStatus;
    protected Fixpoint StatusTime = new Fixpoint(0, 0);

    protected int CharacterType = 0;

    protected int[] ToughnessStatus;
    protected Fix_vector2[] HitFlySpeed = new Fix_vector2[4] { new Fix_vector2(new Fixpoint(0,0),new Fixpoint(9,0)),
            new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(8, 1)), //击飞1,x轴y轴速度
            new Fix_vector2(new Fixpoint(187, 2), new Fixpoint(38, 1)), //2
            new Fix_vector2(new Fixpoint(4, 0), new Fixpoint(66, 1)) };//3
    protected Fix_vector2 Rebound = new Fix_vector2(new Fixpoint(10, 0), new Fixpoint(3, 0));
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
        Fire,
        Fire1,
        Fire2,
        Bomb,
        CallMagic,
        CannonMagic,
        SuckerPunch,
        Roll,
        Kick,
        Upattack,
        HeavyAttack,
        Stay
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void ClearNumber()
    {

    }

    protected void SetStatus(int hp,int attack)
    {
        status = new PlayerStatus(hp, attack);
    }

    public virtual void Updatex()
    {

    }

    
    /*
    protected void ChangeStatus(int animastatus)
    {
        AnimaStatus = animastatus;
        StatusTime = new Fixpoint(0, 0);
    }
    */
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
    
    protected void CreateAttack(Fix_vector2 pos, Fixpoint wide, Fixpoint high, Fixpoint HpDamage, int toughness, float Toward,int Flytype)
    {
        Main_ctrl.NewAttack(pos, new Fix_vector2(0, 0), wide, high, HpDamage, toughness, id, Toward , false,CharacterType , Flytype);
    }

    protected void CreateAttackWithCharacter(Fix_vector2 pos , Fix_vector2 with_pos, Fixpoint wide, Fixpoint high, Fixpoint HpDamage, int toughness, float Toward,int Flytype)
    {
        Main_ctrl.NewAttack(pos, with_pos, wide, high, HpDamage, toughness, id, Toward, true, CharacterType,Flytype);
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

    protected void Preform(int damage)
    {
        if (damage > 0)
        {
            GameObject beat = (GameObject)AB.getobj("beat");
            beat.transform.localScale = new Vector3(3f, 3f, 1f);
            Instantiate(beat, transform.position, transform.rotation);
        }
        GameObject num = (GameObject)AB.getobj("HurtNumber");
        GameObject num2 = Instantiate(num, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        num2.GetComponent<BeatNumber>().ChangeNumber(damage);
    }

    private int hit_fly_type = 0;
    protected bool GetHited()
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
            if (attack.attacker_type == CharacterType && attack.attacker_type != 0) continue;
            if (attack.attacker_type == 1 && CharacterType == 2) continue;
            if (attack.attacker_type == 2 && CharacterType == 1) continue;
            if (attack.attacker_type != 2 && CharacterType == 4) continue;
            if (attack.attakcer_id == id)
            {
                continue;
            }
            if (!Player_ctrl.checkattack((int)attack.attakcer_id, (int)id) && attack.attacker_type == CharacterType)
            {
                continue;
            }
            
            AnimaToward = -attack.toward;
            this_hited = true;
            hit_fly_type = attack.hited_fly_type;

            if(!f.onground && status.toughness < 100000)
            {
                status.toughness = 0;
            }

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

    protected int CheckToughnessStatus()
    {
        if (status.GetToughness() < 0)
        {
            status.toughness = -500;
            AnimaHited = ToughnessStatus.Length;
            Fix_vector2 speed = HitFlySpeed[hit_fly_type].Clone();
            if(AnimaToward > 0)
            {
                speed.x = new Fixpoint(0,0) - speed.x;
            }
            r.velocity = speed;
            ChangeStatus(StatusType.Hit);
            return 2;
        }
        for(int i = ToughnessStatus.Length - 1 ; i >= 0 ; --i)
        {
            if(status.GetToughness() < ToughnessStatus[i])
            {
                AnimaHited = i + 1;
                ChangeStatus(StatusType.Hit);
                return 1;
            }
        }
        return 0;
    }

    protected int BasicCharacterGetHited()
    {
        bool hited = GetHited();
        if(hited == true)
        {
            return CheckToughnessStatus();
        }
        return 0;
    }

    protected Fixpoint[] HitTime;
    protected Fixpoint[] HitSpeed;
    protected void Hited()
    {
        int HitType = BasicCharacterGetHited();
        if(HitType == 0)
        {
            if(AnimaHited == ToughnessStatus.Length)
            {
                if (f.onground && StatusTime > new Fixpoint(1,1))
                {
                    if (status.toughness < -1000)
                    {
                        //Debug.Log("Ground2");
                        AnimaHited = 0;
                        ChangeStatus(StatusType.Ground);
                        r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(0, 0));
                    } else
                    {
                        //Debug.Log("Ground1");
                        animator.Rebind();
                        status.toughness = -2000;
                        ChangeStatus(StatusType.Hit);
                        Fix_vector2 speed = Rebound.Clone();
                        if (AnimaToward > 0)
                        {
                            speed.x = new Fixpoint(0, 0) - speed.x;
                            r.velocity = speed;
                        }
                        r.velocity = speed;
                    }
                }
                return;
            }
            if(StatusTime > HitTime[AnimaHited])
            {
                AnimaHited = 0;
                ChangeStatus(StatusType.Normal);
                return;
            }
        }
    }
    protected Fixpoint GroundTime = new Fixpoint(1, 0);
    protected void Ground()
    {
        RemoveHited();
        if(StatusTime > GroundTime)
        {
            status.toughness = status.max_toughness;
            ChangeStatus(StatusType.Normal);
            AnimaHited = 0;
            return;
        }
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
        Main_ctrl.NewItem(f.pos.Clone(), name, num, size,new Fix_vector2(0,0));
    }

    public float CheckHealth()
    {
        return 1f * status.hp / status.max_hp;
    }
}
