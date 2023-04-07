using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Monster
{
    private Animator animator;
    private float AnimaSpeed = 0f;
    private int AnimaAttack = 0;
    private int AnimaHited = 0;
    void Start()
    {
        SetStatus(1000, 10);//血量，基础攻击力
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
        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if(status.death == true && AnimaStatus != 8)
        {
            ChangeStatus(8);
        }
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


    private int DefenceGetHited()
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

            if (attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                if (attack2.toward * AnimaToward < 0)
                {
                    Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), attack2.HpDamage, attack2.ToughnessDamage, id, AnimaToward);
                    Main_ctrl.Desobj(attack2.id);
                } else
                {
                    status.GetAttacked(attack.HpDamage/DefenceRate, attack.ToughnessDamage);
                    attack2.DestroySelf();
                    Preform(status.last_damage);
                }
                continue;
            }

            this_hited = true;

            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;

            if (attack.toward * AnimaToward < 0)
            {
                status.GetAttacked(HpDamage, ToughnessDamage/2);
            }
            else
            {
                status.GetAttacked(HpDamage/DefenceRate, ToughnessDamage);
            }

            Preform(status.last_damage);
        }
        return CheckToughStatus(this_hited);
    }

    private void Normal()
    {
        int hited = KnightGetHited();
        if (hited != 0) {
            ChangeStatus(5);
            return; 
        }

        if(!f.onground)
        {
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
        else if (Dis > new Fixpoint(3, 0) && Dis < new Fixpoint(5, 0))
        {
            if (Rand.rand() % 2 == 0)
            {
                ChangeStatus(3);
            } else
            {
                ChangeStatus(4);
            }
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
    private Fixpoint Attack1DuringTime = new Fixpoint(59, 2);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(61, 2);
    private Fixpoint Attack3DuringTime = new Fixpoint(64, 2);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(33, 2);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(2, 1);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(25, 2);

    private Fixpoint Attack1Damage = new Fixpoint(4, 0);//伤害倍率
    private Fixpoint Attack2Damage = new Fixpoint(4, 0);
    private Fixpoint Attack3Damage = new Fixpoint(4, 0);
    private void AttackToNext()
    {
        CreatedAttack = false;
        AnimaAttack = AnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }
    private void RemoveAttack()
    {
        AnimaAttack = 0;
        ChangeStatus(0);
        return;
    }
    private void KnightCreateAttack(Fixpoint damage)
    {
        CreatedAttack = true;
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * damage, 40, AnimaToward);

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
            if (StatusTime > Attack1BeginToHitTime && CreatedAttack == false) KnightCreateAttack(Attack1Damage);
        }
        else if (AnimaAttack == 2)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && CreatedAttack == false) KnightCreateAttack(Attack2Damage);
        }
        else if (AnimaAttack == 3)
        {
            if (StatusTime > Attack3DuringTime)
            {
                RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && CreatedAttack == false) KnightCreateAttack(Attack3Damage);
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

    private void Hited()
    {
        int hit = KnightGetHited();
        if(hit == 0)
        {
            ChangeStatus(0);
        }
        else if(hit == 2)
        {
            status.toughness = -100;
            if(f.onground && StatusTime > new Fixpoint(3,1))
            {
                ChangeStatus(6);
            }
        }
    }

    private static Fixpoint DefenceTime = new Fixpoint(5, 1);//防御时间
    private static Fixpoint DefenceRate = new Fixpoint(5, 1);//承受伤害倍率
    private void Defence()
    {
        status.defence_rate = DefenceRate;
        int hited = DefenceGetHited();
        if (hited != 0)
        {
            ChangeStatus(5);
            status.defence_rate = new Fixpoint(1, 0);
            Debug.Log(status.toughness);
            return;
        }
        if (StatusTime > DefenceTime)
        {
            status.defence_rate = new Fixpoint(1, 0);
            ChangeStatus(0);
        }
    }

    private static Fixpoint SkillBeginToHitTime = new Fixpoint(133, 2);//技能的结算开始时间
    private static Fixpoint SkillDruingTime = new Fixpoint(163, 2);
    private static Fixpoint SkillBetweenTime = new Fixpoint(21, 2);//伤害的间隔
    private static Fixpoint SkillAttackRate = new Fixpoint(5, 0);//攻击倍率
    private int SkillAttackTimes = 0;
    private void Skill()
    {
        int hited = KnightGetHited();
        if (hited != 0)
        {
            SkillAttackTimes = 0;
            return;
        }
        if(StatusTime < SkillBeginToHitTime) 
        { 
            Fixpoint Pos = GetNear();
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
        }
        if (StatusTime > SkillBeginToHitTime + SkillBetweenTime * new Fixpoint(SkillAttackTimes,0))
        {
            if(SkillAttackTimes == 0)
            {
                Vector3 pos = new Vector3(f.pos.x.to_float(), f.pos.y.to_float() + 2.5f, 0);
                if (AnimaToward < 0) pos.x -= 3f;
                else pos.x += 3f;
                Instantiate(Resources.Load("Prefabs/knightskill"),pos,Quaternion.identity);
            }
            ++SkillAttackTimes;
            Fix_vector2 tmp_pos = f.pos.Clone();
            if (AnimaToward > 0) tmp_pos.x += new Fixpoint(3, 0);
            else tmp_pos.x -= new Fixpoint(3, 0);
            tmp_pos.y += new Fixpoint(25, 1);
            CreateAttack(tmp_pos, new Fixpoint(5, 0), new Fixpoint(7, 0), status.Damage() * SkillAttackRate, 120, AnimaToward);
        }
        if(StatusTime > SkillDruingTime)
        {
            SkillAttackTimes = 0;
            ChangeStatus(0);
        }
    }

    private static Fixpoint OnGroundTime = new Fixpoint(12, 1);//倒地时间
    private void Ground()
    {
        RemoveHited();
        AnimaHited = 0;
        if (StatusTime > OnGroundTime)
        {
            status.toughness = 100;
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
        AnimaHited = 0;
        AnimaAttack = 0;
        if(StatusTime > new Fixpoint(3,0))
        {
            Main_ctrl.Desobj(id);
        }
    }
}
