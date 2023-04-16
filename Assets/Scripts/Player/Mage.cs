using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Knight
{
    private Animator animator;
    void Start()
    {
        CharacterType = 1;
        SetStatus(250, 10);//血量，基础攻击力
        animator = GetComponent<Animator>();
        HitTime = new Fixpoint[3] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[3] { new Fixpoint(0, 0) , new Fixpoint(5, 1), new Fixpoint(2, 1) };
        ToughnessStatus = new int[3] { 60, 30, 0 };//阶段
    }
    void Update()
    {
        animator.SetFloat("speed", KnightAnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("attack", KnightAnimaAttack);
        animator.SetInteger("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
    }
    public override void Updatex()
    {
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if (status.death == true && RealStatus != StatusType.Death)
        {
            ChangeStatus(StatusType.Death);
        }
        if (AnimaStatus != 6 && AnimaStatus != 7 && AnimaStatus != 8 && AnimaStatus != 9 && AnimaStatus != 11)
        {
            int hited = BasicCharacterGetHited();
            if (hited != 0)
            {
                ChangeStatus(StatusType.Hit);
                KnightAnimaAttack = 0;
                Fired = false;
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
                Attack(false);
                break;
            case StatusType.Fire:
                AnimaStatus = 4;
                Fire();
                break;
            case StatusType.Recover:
                AnimaStatus = 5;
                Recover();
                break;
            case StatusType.Disappear:
                AnimaStatus = 6;
                Disappaer();
                break;
            case StatusType.Appear:
                AnimaStatus = 7;
                Appear();
                break;
            case StatusType.Hit:
                AnimaStatus = 8;
                Hited();
                break;
            case StatusType.Ground:
                AnimaStatus = 9;
                Ground();
                break;
            case StatusType.Fall:
                AnimaStatus = 10;
                Fall();
                break;
            case StatusType.Death:
                AnimaStatus = 11;
                Death();
                break;
            case StatusType.Search:
                AnimaStatus = 12;
                Search();
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
    /*
    private int MageGetHited()
    {
        bool x = GetHited(ref AnimaToward);
        return CheckToughStatus(x);
    }
    */

    private void Normal()
    {
        KnightAnimaSpeed = 5f;

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
            Main_ctrl.node area = Main_ctrl.GetMapNode(f.pos.x, f.pos.y);
            Fixpoint Left = new Fixpoint(area.left, 0) + new Fixpoint(15, 1);
            Fixpoint Right = new Fixpoint(area.right, 0) - new Fixpoint(15, 1);
            Fixpoint Dis = f.pos.x - Nearx;
            if (Dis < new Fixpoint(0, 0)) Dis = new Fixpoint(0, 0) - Dis;
            if(Rand.rand() % ((ulong)status.max_hp * 10) < (ulong)(status.max_hp - status.hp))//使用技能的概率
            {
                if(Rand.rand()%2 == 1 || Dis > new Fixpoint(10,0))
                {
                    ChangeStatus(StatusType.Recover);
                } else
                {
                    ChangeStatus(StatusType.Disappear);
                }
                return;
            }
            if (Dis < new Fixpoint(14, 1)) //攻击
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                StatusTime = new Fixpoint(0, 0);
                ChangeStatus(StatusType.Attack);
                Attack(true);
                return;
            }
            else if (Dis < new Fixpoint(3,0)) //靠近攻击
            {
                if (f.pos.x < Nearx)
                {
                    AnimaToward = 1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                else
                {
                    AnimaToward = -1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            } else if(Dis < new Fixpoint(9,0) && f.pos.x > Left && f.pos.x < Right ) //远离射击
            {
                if (f.pos.x > Nearx)
                {
                    AnimaToward = 1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                else
                {
                    AnimaToward = -1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            }
            else if (Dis < new Fixpoint(15, 0) || f.pos.x < Left || f.pos.x > Right) //射击
            {
                if (f.pos.x < Nearx)
                {
                    AnimaToward = 1;
                }
                else
                {
                    AnimaToward = -1;
                }
                ChangeStatus(StatusType.Fire);
                return;
            }
            else //靠近
            {
                if (f.pos.x < Nearx)
                {
                    AnimaToward = 1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                else
                {
                    AnimaToward = -1;
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            }
        }
        else //否则寻路
        {
            ChangeStatus(StatusType.Search);
        }
    }

    private bool MageCreatedAttack = false;
    private Fixpoint Attack1DuringTime = new Fixpoint(1, 0);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(29, 2);
    private Fixpoint Attack3DuringTime = new Fixpoint(84, 2);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(83, 2);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(1, 1);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(41, 2);

    private Fixpoint Attack1Damage = new Fixpoint(3, 0);//伤害倍率
    private Fixpoint Attack2Damage = new Fixpoint(3, 0);
    private Fixpoint Attack3Damage = new Fixpoint(3, 0);

    protected override void AttackToNext()
    {
        MageCreatedAttack = false;
        KnightAnimaAttack = KnightAnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }
    protected override void RemoveAttack()
    {
        KnightAnimaAttack = 0;
        ChangeStatus(StatusType.Normal);
        return;
    }
    protected void Attack(bool first)
    {
        Fixpoint Near = GetNearDistance();
        if (first == true)
        {
            AttackToNext();
        }
        else if (KnightAnimaAttack == 1)
        {
            if (StatusTime > Attack1DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack1BeginToHitTime && MageCreatedAttack == false) KnightCreateAttack(Attack1Damage, ref MageCreatedAttack);
        }
        else if (KnightAnimaAttack == 2)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && MageCreatedAttack == false) KnightCreateAttack(Attack2Damage, ref MageCreatedAttack);
        }
        else if (KnightAnimaAttack == 3)
        {
            if (StatusTime > Attack3DuringTime)
            {
                RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && MageCreatedAttack == false) KnightCreateAttack(Attack3Damage, ref MageCreatedAttack);
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

    private static Fixpoint FireBeginToHitTime = new Fixpoint(5, 1);
    private static Fixpoint FireDuringTime = new Fixpoint(1, 0);
    private static Fixpoint FireAttack = new Fixpoint(2, 0);
    private bool Fired = false;
    private void Fire()
    {
        if(StatusTime > FireBeginToHitTime && Fired == false)
        {
            Fired = true;
            Main_ctrl.NewAttack2("FireBall",f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * FireAttack, 40, id, AnimaToward, CharacterType);
        }
        if(StatusTime > FireDuringTime)
        {
            Fired = false;
            ChangeStatus(StatusType.Normal);
        }
    }

    private static Fixpoint RecoverTime = new Fixpoint(1, 0);//回血的时间，默认最后一帧回血
    private static int RecoverHp = 100;//回血量
    private void Recover()
    {
        if(StatusTime > RecoverTime)
        {
            status.RecoverHp(RecoverHp);
            ChangeStatus(StatusType.Normal);
        }
    }
    private static Fixpoint DisappearTime = new Fixpoint(1, 0);//瞬移的时间，默认最后一帧消失
    private void Disappaer()
    {
        if(StatusTime > DisappearTime)
        {
            int Location = Main_ctrl.CalPos(f.pos.x, f.pos.y);
            if (Location == -1)
            {
                Moves(AnimaToward, status.WalkSpeed);
            }
            else
            {
                Main_ctrl.node area = Main_ctrl.GetMapNode(f.pos.x, f.pos.y);
                Fixpoint Left = new Fixpoint(area.left, 0) + new Fixpoint(15, 1);
                Fixpoint Right = new Fixpoint(area.right, 0) - new Fixpoint(15, 1);
                Fixpoint DisL = f.pos.x - Left;
                Fixpoint DisR = Right - f.pos.x;
                if(DisL > DisR)
                {
                    f.pos.x = Left + new Fixpoint(15, 1);
                } else
                {
                    f.pos.x = Right - new Fixpoint(15, 1);
                }
            }
            ChangeStatus(StatusType.Appear);
        }
    }
    private static Fixpoint AppearTime = new Fixpoint(1, 0);//出现所需时间
    private void Appear()
    {
        if(StatusTime > AppearTime)
        {
            ChangeStatus(StatusType.Normal);
        }
    }

}
