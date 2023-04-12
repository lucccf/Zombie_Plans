using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : Knight
{
    private Animator animator;
    private float DevilAnimaSpeed = 0f;
    private int DevilAnimaAttack = 0;
    void Start()
    {
        CharacterType = 1;
        SetStatus(1000, 10);//血量，基础攻击力
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
                DevilAttack(false);
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
                ChangeStatus(StatusType.Attack);
                DevilAttack(true);
                return;
            }
            else if (Dis > new Fixpoint(3, 0) && Dis < new Fixpoint(5, 0))//距离判定
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
            }
        }
        else //否则寻路
        {
            ChangeStatus(StatusType.Search);
        }
    }

    private void DevilAttack(bool first)
    {

    }

    private void CannonMagic()
    {

    }

    private void Bomb()
    {

    }

    private void CallMagic()
    {

    }

    private void DevilHited()
    {

    }

    private void SuckerPunch()
    {

    }

    private int DevilGetHited()
    {
        return 0;
    }

}
