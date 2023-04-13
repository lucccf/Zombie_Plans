using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Devil : Knight
{
    private Animator animator;
    private float DevilAnimaSpeed = 0f;
    private int DevilAnimaAttack = 0;

    private Fixpoint BombCD = new Fixpoint(0,0);
    private Fixpoint MagicCannonCD = new Fixpoint(0, 0);
    private Fixpoint SuckerPunchCd = new Fixpoint(0, 0);
    void Start()
    {
        CharacterType = 1;
        SetStatus(10000, 10);//血量，基础攻击力
        animator = GetComponent<Animator>();
        status.max_toughness = 200;
        status.toughness = 200;
        status.WalkSpeed = new Fixpoint(3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("attack", DevilAnimaAttack);
        animator.SetInteger("status", AnimaStatus);
        animator.SetFloat("speed", DevilAnimaSpeed);
    }

    public override void Updatex()
    {
        StatusTime += Dt.dt;
        BombCD -= Dt.dt;
        MagicCannonCD -= Dt.dt;
        SuckerPunchCd -= Dt.dt;
        if (BombCD < new Fixpoint(0, 0)) BombCD = new Fixpoint(0, 0);
        if (MagicCannonCD < new Fixpoint(0, 0)) MagicCannonCD = new Fixpoint(0, 0);
        if (SuckerPunchCd < new Fixpoint(0, 0)) SuckerPunchCd = new Fixpoint(0, 0);
        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if (status.death == true && AnimaStatus != 8)
        {
            ChangeStatus(StatusType.Death);
        }
        if (RealStatus != StatusType.Death && RealStatus != StatusType.Ground && RealStatus != StatusType.CallMagic && RealStatus != StatusType.Hit)
        {
            int hited = DevilGetHited();
            if (hited != 0)
            {
                DevilAttackTimes = 0;
                DevilCannonMagicShooted = false;
                DevilBonmTimes = 0;
                ChangeStatus(StatusType.Hit);
            }
        }
        switch (RealStatus)
        {
            case StatusType.Normal:
                AnimaStatus = 0;
                Normal();
                break;
            case StatusType.LittleJump:
                AnimaStatus = 1;
                LittleJump(0);
                break;
            case StatusType.Jump:
                AnimaStatus = 2;
                Jump(0);
                break;
            case StatusType.Attack:
                AnimaStatus = 3;
                DevilAttack();
                break;
            case StatusType.CannonMagic:
                AnimaStatus = 4;
                CannonMagic();
                break;
            case StatusType.Bomb:
                AnimaStatus = 5;
                Bomb();
                break;
            case StatusType.CallMagic:
                AnimaStatus = 6;
                CallMagic();
                break;
            case StatusType.Hit:
                AnimaStatus = 7;
                DevilHited();
                break;
            case StatusType.Ground:
                AnimaStatus = 8;
                Ground();
                break;
            case StatusType.Fall:
                AnimaStatus = 9;
                Fall();
                break;
            case StatusType.Death:
                AnimaStatus = 10;
                Death();
                break;
            case StatusType.Search:
                AnimaStatus = 11;
                Search();
                break;
            case StatusType.SuckerPunch:
                AnimaStatus = 12;
                SuckerPunch();
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }

    private void Normal()
    {
        DevilAnimaSpeed = 5f;

        if (!f.onground)
        {
            ChangeStatus(StatusType.Fall);
            return;
        }

        int Location = Main_ctrl.CalPos(f.pos.x, f.pos.y);
        if (Location == -1)
        {
            Moves(AnimaToward, status.WalkSpeed);
        }
        Fixpoint Nearx = new Fixpoint(0, 0);
        int Pos = KnightGetNear(ref Nearx);
        if (Pos == -1) // 如果距离太远，巡逻
        {
            Main_ctrl.node area = Main_ctrl.GetMapNode(f.pos.x, f.pos.y);
            Fixpoint Left = new Fixpoint(area.left, 0) + new Fixpoint(15, 1);
            Fixpoint Right = new Fixpoint(area.right, 0) - new Fixpoint(15, 1);
            if (f.pos.x < Left)
            {
                AnimaToward = 1;
            }
            else if (f.pos.x > Right)
            {
                AnimaToward = -1;
            }
            Moves(AnimaToward, status.WalkSpeed);
            return;
        }
        else if (Location == Pos) //如果在同一区域
        {
            Fixpoint Dis = f.pos.x - Nearx;
            if (Dis < new Fixpoint(0, 0)) Dis = new Fixpoint(0, 0) - Dis;

            if (Dis < new Fixpoint(14, 1)) //攻击
            {   
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                if (BombCD == new Fixpoint(0,0))
                {
                    BombCD = new Fixpoint(5, 0);
                    ChangeStatus(StatusType.Bomb);
                }
                else
                {
                    ChangeStatus(StatusType.Attack);
                }
                return;
            }
            else if (Dis > new Fixpoint(3, 0) && Dis < new Fixpoint(9, 0))//距离判定
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                if (SuckerPunchCd == new Fixpoint(0, 0))
                {
                    SuckerPunchCd = new Fixpoint(5, 0);
                    ChangeStatus(StatusType.SuckerPunch);
                } else
                {
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            }
            else if(Dis > new Fixpoint(9,0) && Dis < new Fixpoint(15,0))
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                if (MagicCannonCD == new Fixpoint(0, 0))
                {
                    MagicCannonCD = new Fixpoint(3, 0);
                    ChangeStatus(StatusType.CannonMagic);
                } else
                {
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            } else
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                Moves(AnimaToward, status.WalkSpeed);
            }
            return;
        }
        else //否则寻路
        {
            ChangeStatus(StatusType.Search);
        }
    }


    private static Fixpoint DevilAttack1HitTime = new Fixpoint(3, 1);
    private static Fixpoint DevilAttack2HitTime = new Fixpoint(6, 1);
    private static Fixpoint DevilAttack3HitTime = new Fixpoint(9, 1);
    private static Fixpoint DevilAttackQuitTime = new Fixpoint(1, 0);
    private static Fixpoint DevilAttack1Damage = new Fixpoint(4, 0);
    private static Fixpoint DevilAttack2Damage = new Fixpoint(4, 0);
    private static Fixpoint DevilAttack3Damage = new Fixpoint(4, 0);
    private int DevilAttackTimes = 0;

    private void DevilCreateAttack(Fixpoint HPDamage, int ToughnessDamage)
    {
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(2, 0), new Fixpoint(2, 0), HPDamage, ToughnessDamage, AnimaToward);
    }
    private void DevilAttack()
    {
        if(StatusTime <= DevilAttack1HitTime)
        {
            return;
        }
        else if(StatusTime <= DevilAttack2HitTime && DevilAttackTimes == 0)
        {
            ++DevilAttackTimes;
            DevilCreateAttack(status.Damage() * DevilAttack1Damage,40);
        } else if (StatusTime <= DevilAttack3HitTime && DevilAttackTimes == 1)
        {
            ++DevilAttackTimes;
            DevilCreateAttack(status.Damage() * DevilAttack2Damage, 40);
        } else if(StatusTime <= DevilAttackQuitTime && DevilAttackTimes == 2)
        {
            ++DevilAttackTimes;
            DevilCreateAttack(status.Damage() * DevilAttack3Damage, 40);
        } else
        {
            DevilAttackTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    private static Fixpoint DevilCannonMagicShootTime = new Fixpoint(7, 1);
    private static Fixpoint DevilCannonMagicQuitTime = new Fixpoint(1, 0);
    private static Fixpoint DevilCannonMagicAttack = new Fixpoint(50, 0);
    private bool DevilCannonMagicShooted = false;
    private void CannonMagic()
    {
        if(StatusTime > DevilCannonMagicShootTime && DevilCannonMagicShooted == false)
        {
            DevilCannonMagicShooted = true;
            Main_ctrl.NewAttack2("MagicCannon", f.pos, new Fixpoint(2, 0), new Fixpoint(2, 0), status.Damage() * DevilCannonMagicAttack, 120, id, AnimaToward, CharacterType);
        }
        if(StatusTime > DevilCannonMagicQuitTime)
        {
            DevilCannonMagicShooted = false;
            ChangeStatus(StatusType.Normal);
            return;
        }
    }

    private static Fixpoint DevilBombHitTime = new Fixpoint(66, 2);
    private static Fixpoint DevilBombHitBetween = new Fixpoint(3, 1);
    private static Fixpoint DevilBombQuitTime = new Fixpoint(166, 2);
    private static Fixpoint DevilBombAttack = new Fixpoint(5, 1);
    private int DevilBonmTimes = 0;
    private void Bomb()
    {
        if(StatusTime > DevilBombHitTime + new Fixpoint(DevilBonmTimes,0) * DevilBombHitBetween)
        {
            if(DevilBonmTimes == 0)
            {
                GameObject x = (GameObject)Resources.Load("Prefabs/bomb");
                GameObject y = Instantiate(x, new Vector3(f.pos.x.to_float(), f.pos.y.to_float() + 1.8f, 0), Quaternion.identity);
                y.GetComponent<Bomb>().toward = AnimaToward;
            }
            ++DevilBonmTimes;
            Fix_vector2 pos = f.pos.Clone();
            pos.y += new Fixpoint(18,1);
            CreateAttack(pos, new Fixpoint(45, 1), new Fixpoint(7, 0), status.Damage() * DevilBombAttack, 60, AnimaToward);
        }
        if(StatusTime > DevilBombQuitTime)
        {
            DevilBonmTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    private void CallMagic()
    {

    }

    private int DevilGetHited()
    {
        bool hited = GetHited(ref AnimaToward);
        return DevilCheckHitStatus(hited);
    }

    private int DevilCheckHitStatus(bool this_hited)
    {
        if (status.GetToughness() >= 100)//韧性值
        {
            KnightAnimaHited = 0;
            return 0;
        }
        else if (status.GetToughness() >= 70)
        {
            KnightAnimaHited = 1;
            RealStatus = StatusType.Hit;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if(status.GetToughness() >= 40)
        {
            KnightAnimaHited = 2;
            RealStatus = StatusType.Hit;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        } else if(status.GetToughness() >= 0)
        {
            KnightAnimaHited = 3;
            RealStatus = StatusType.Hit;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() > -1000)
        {
            KnightAnimaHited = 4;
            RealStatus = StatusType.Hit;
            if (this_hited == true)
            {
                StatusTime = new Fixpoint(0, 0);
                r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(5, 0));//受击击飞的，y轴的上升速度
            }
            return 2;
        }
        else
        {
            KnightAnimaHited = 4;
            KnightAnimaHited = 2;
            RealStatus = StatusType.Hit;
            if (this_hited == true)
            {
                StatusTime = new Fixpoint(0, 0);
                if (AnimaToward > 0)
                    r.velocity = new Fix_vector2(new Fixpoint(-10, 0), new Fixpoint(38, 1));//空中被击飞的x,y轴的上升速度
                else r.velocity = new Fix_vector2(new Fixpoint(10, 0), new Fixpoint(38, 1));
            }
            return 2;
        }
    }

    private static Fixpoint DevilSuckerPunchAttack = new Fixpoint(15, 0);
    private static Fixpoint DevilSuckerPunckBeginTime = new Fixpoint(35, 2);
    private static Fixpoint DevilSuckerPunckQuitTime = new Fixpoint(1, 0);
    private static Fixpoint DevilSuckerPunckSpeed = new Fixpoint(20, 0);
    private bool DevilSuckerPunckCreatedAttack = false;
    private void SuckerPunch()
    {
        if(StatusTime > DevilSuckerPunckBeginTime)
        {
            Moves(AnimaToward,DevilSuckerPunckSpeed);
            if (DevilSuckerPunckCreatedAttack == false)
            {
                DevilSuckerPunckCreatedAttack = true;
                CreateAttackWithCharacter(f.pos, new Fix_vector2(0, 0), new Fixpoint(3, 0), new Fixpoint(2, 0), status.Damage() * DevilSuckerPunchAttack, 120, AnimaToward);
            }
        }
        if(StatusTime > DevilSuckerPunckQuitTime)
        {
            DevilSuckerPunckCreatedAttack = false;
            ChangeStatus(StatusType.Normal);
        }
    }
     
    private void DevilHited()
    {
        int hit = DevilGetHited();
        if (hit == 0)
        {
            ChangeStatus(StatusType.Normal);
        }
        else if (hit == 2)
        {
            status.toughness = -100;
            if (f.onground && StatusTime > new Fixpoint(3, 1))
            {
                ChangeStatus(StatusType.Ground);
            }
        }
    }

}
