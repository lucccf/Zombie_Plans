using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Knight : Monster
{
    private Animator animator;
    private new float AnimaSpeed = 0f;
    private new int AnimaAttack = 0;
    private new int AnimaHited = 0;
    void Start()
    {
        SetStatus(1000, 100);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("attack", AnimaAttack);
        animator.SetInteger("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
    }

    public override void Updatex()
    {
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(25, 0));
        switch (AnimaStatus)
        {
            case 0:
                Normal();
                break;
            case 1:
                Jump();
                break;
            case 2:
                Attack(false);
                break;
            case 3:
                Defence();
                break;
            case 4:
                Skill();
                break;
            case 5:
                Hited();
                break;
            case 6:
                Ground();
                break;
            case 7:
                Fall();
                break;
            case 8:
                Death();
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
    private int CheckToughStatus(bool this_hited)
    {
        if (status.GetToughness() >= 20)
        {
            AnimaHited = 0;
            return 0;
        }
        else if (status.GetToughness() >= 0)
        {
            AnimaHited = 1;
            AnimaStatus = 5;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        } else
        {
            AnimaHited = 2;
            AnimaStatus = 5;
            if (this_hited == true)
            {
                StatusTime = new Fixpoint(0, 0);
                r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(5, 0));
            } 
            return 2;
        }
    }

    private int KnightGetHited()
    {
        bool x = GetHited(ref AnimaToward);
        return CheckToughStatus(x);
    }


    private bool DefenceGetHited()
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
            if (attack.attakcer_id == id) continue;

            AnimaToward = -attack.toward;
            this_hited = true;


            Preform(attack.HpDamage.to_int());

            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;
            status.GetAttacked(HpDamage, ToughnessDamage);

            Debug.Log(HpDamage + " " + ToughnessDamage);

            if (attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                attack2.DestroySelf();
            }
        }
        return this_hited;
    }

    private void Normal()
    {
        int hited = KnightGetHited();
        if (hited != 0) return;

        if(!f.onground)
        {
            AnimaSpeed = 0;
            ChangeStatus(7);
            return;
        }

        Fixpoint Pos = GetNear();
        Fixpoint Dis = GetNearDistance(Pos);
        if (Dis > new Fixpoint(15, 0)) // 巡逻
        {
            if (StatusTime > new Fixpoint(2, 0)) StatusTime -= new Fixpoint(2, 0);
            if (StatusTime > new Fixpoint(1, 0))
            {
                AnimaToward = 1f;
                AnimaSpeed = status.WalkSpeed.to_float();
                Moves(AnimaToward, status.WalkSpeed);
            }
            else {
                AnimaToward = -1f;
                AnimaSpeed = status.WalkSpeed.to_float();
                Moves(AnimaToward,status.WalkSpeed); 
            }
            return;
        }
        else if (Dis < new Fixpoint(14, 1)) //攻击
        {
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 2;
            Attack(true);
            return;
        }
        else if (Dis > new Fixpoint(5, 0) && Dis < new Fixpoint(7, 0)) //翻滚
        {
            ChangeStatus(3);
            return;
        }
        else //靠近
        {
            if (f.pos.x < Pos)
            {
                AnimaToward = 1;
                Moves(AnimaToward,status.WalkSpeed);
            }
            else
            {
                AnimaToward = -1;
                Moves(AnimaToward,status.WalkSpeed);
            }
        }
    }


    private bool CreatedAttack = false;
    private Fixpoint Attack1DuringTime = new Fixpoint(8, 1);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(8, 1);
    private Fixpoint Attack3DuringTime = new Fixpoint(8, 1);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(2, 1);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(2, 1);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(2, 1);

    private Fixpoint Attack1Damage = new Fixpoint(1, 0);
    private Fixpoint Attack2Damage = new Fixpoint(1, 0);
    private Fixpoint Attack3Damage = new Fixpoint(1, 0);
    private void AttackToNext()
    {
        CreatedAttack = false;
        AnimaAttack = AnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }
    private void RemoveAttack()
    {
        AnimaAttack = 0;
        StatusTime = new Fixpoint(0, 0);
        AnimaStatus = 0;
        return;
    }
    private void Attack(bool first)
    {
        int hited = KnightGetHited();
        if (hited != 0)
        {
            AnimaAttack = 0;
            return;
        }

        Fixpoint Near = GetNearDistance();
        if (first == true)
        {
            AttackToNext();
        }
        else if (AnimaAttack  == 1)
        {
            if (StatusTime > Attack1DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack1BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack1Damage);
        }
        else if (AnimaAttack == 2)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack2Damage);
        }
        else if (AnimaAttack == 3)
        {
            if (StatusTime > Attack3DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack3Damage);
        }

        if (StatusTime <= new Fixpoint(2, 1))
        {
            if (AnimaToward > 0)
            {
                f.pos.x = f.pos.x + Dt.dt * new Fixpoint(1, 0);
            }
            else
            {
                f.pos.x = f.pos.x - Dt.dt * new Fixpoint(1, 0);
            }
        }
    }

    private void Jump()
    {

    }

    bool HitFly = false;
    private void Hited()
    {
        int hit = KnightGetHited();
        if(hit == 0)
        {
            ChangeStatus(0);
        }
        else if(hit == 2)
        {
            HitFly = true;
            status.toughness = -100;
            if(f.onground && StatusTime > new Fixpoint(3,1))
            {
                ChangeStatus(6);
            }
        }
    }

    private static Fixpoint DefenceTime = new Fixpoint(1, 0);
    private static Fixpoint DefenceRate = new Fixpoint(2, 1);
    private void Defence()
    {
        int hit = KnightGetHited();
        if (hit != 0)
        {
            return;
        }
        status.defence_rate = DefenceRate;
        status.toughness = 100;
        if(StatusTime > DefenceTime)
        {
            status.defence_rate = new Fixpoint(1, 0);
            ChangeStatus(1);
        }
    }

    private void Skill()
    {

    }

    private static Fixpoint OnGroundTime = new Fixpoint(1, 0);
    private void Ground()
    {
        RemoveAttack();
        if(StatusTime > OnGroundTime)
        {
            ChangeStatus(0);
        }
    }

    private void Fall()
    {
        if(f.onground)
        {
            ChangeStatus(0);
        }
    }

    private void Death()
    {

    }
}
