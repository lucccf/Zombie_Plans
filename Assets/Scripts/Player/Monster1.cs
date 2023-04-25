﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : Monster
{
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;

    private Player player = null;

    public override void InitStatic()
    {
        Attack1DuringTime = new Fixpoint(1, 0);//攻击的持续时间
        Attack2DuringTime = new Fixpoint(33, 2);
        Attack3DuringTime = new Fixpoint(33, 2);
        Attack4DuringTime = new Fixpoint(58, 2);

        Attack1BeginToHitTime = new Fixpoint(83, 2);//攻击的判定时间
        Attack2BeginToHitTime = new Fixpoint(8, 2);
        Attack3BeginToHitTime = new Fixpoint(8, 2);
        Attack4BeginToHitTime = new Fixpoint(17, 2);

        Attack1Damage = new Fixpoint(3, 0);
        Attack2Damage = new Fixpoint(3, 0);
        Attack3Damage = new Fixpoint(3, 0);
        Attack4Damage = new Fixpoint(3, 0);
    }

    public override void InitNormal()
    {
        status.attack = 10;//基础攻击力
        status.WalkSpeed = new Fixpoint(5, 0);//走路速度
        status.max_hp = 620;//最大血量
        status.hp = 620;//血量
        status.max_toughness = 100;//最大韧性值
        status.toughness = 100;//韧性值
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(49, 2), new Fixpoint(49, 2), new Fixpoint(9, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(5, 1), new Fixpoint(5, 1), new Fixpoint(3, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
    }

    public override void Startx()
    {
        transform.rotation = Quaternion.identity;
        SetStatus(620, 10);
        animator = GetComponent<Animator>();
        CharacterType = 1 + type2;
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(49, 2), new Fixpoint(49, 2), new Fixpoint(9, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(5, 1), new Fixpoint(5, 1), new Fixpoint(3, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
        audiosource = GetComponent<AudioSource>();
        SetFindStatus();
        HitMisuc = "新的挨打";
        //ToHome();
    }

    public override void Updatex()
    {
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(20, 0));//韧性值恢复
        if (AnimaStatus != 6) CheckDeath();
        switch (RealStatus)
        {
            case StatusType.Normal:
                AnimaStatus = 0;
                Normal();
                break;
            case StatusType.Roll:
                AnimaStatus = 1;
                Roll();
                break;
            case StatusType.Attack:
                AnimaStatus = 2;
                Attack(false);
                break;
            case StatusType.Hit:
                AnimaStatus = 3;
                Hited();
                break;
            case StatusType.Ground:
                AnimaStatus = 5;
                Ground();
                break;
            case StatusType.Death:
                AnimaStatus = 6;
                Death();
                break;
            case StatusType.LittleJump:
                AnimaStatus = 114;
                LittleJump(0);
                break;
            case StatusType.Jump:
                AnimaStatus = 514;
                Jump(0);
                break;
            case StatusType.Fall:
                AnimaStatus = 1919;
                Fall();
                break;
        }
    }
    private void CheckDeath()
    {
        if (status.death == true)
        {
            AnimaAttack = 0f;
            AnimaHited = 0;
            ChangeStatus(StatusType.Death);
        }
    }
    private void Normal()
    {
        NormalUpdate();
        AnimaSpeed = 5f;
        int hited = BasicCharacterGetHited();
        if (hited != 0) return;

        if (!f.onground)
        {
            ChangeStatus(StatusType.Fall);
            return;
        }


        int Location = 0;
        int Normalstatus = NormalFind(ref Location);
        if (Normalstatus == 1 || Normalstatus == 2)
        {
            int Pos = Main_ctrl.CalPos(LockPos.x, LockPos.y);
            Fixpoint Nearx = LockPos.x;

            if (Location == Pos) //如果在同一区域
            {
                Fixpoint Dis = f.pos.x - Nearx;
                if (Dis < new Fixpoint(0, 0)) Dis = new Fixpoint(0, 0) - Dis;

                if (Dis < new Fixpoint(14, 1)) //攻击
                {
                    if (f.pos.x < Nearx) AnimaToward = 1;
                    else AnimaToward = -1;
                    ChangeStatus(StatusType.Attack);
                    Attack(true);
                    return;
                }
                else if (Dis > new Fixpoint(5, 0) && Dis < new Fixpoint(7, 0)) //翻滚
                {
                    ChangeStatus(StatusType.Roll);
                    if (f.pos.x < Nearx) AnimaToward = 1;
                    else AnimaToward = -1;
                    return;
                }
                else //靠近
                {
                    if (f.pos.x < Nearx)
                    {
                        Moves(1);
                    }
                    else
                    {
                        Moves(-1);
                    }
                }
            }
            else
            {
                SearchX(Pos);
            }
        }
    }
    protected Fixpoint GetNearDistance(Fixpoint x)
    {
        if (f.pos.x > x)
        {
            return f.pos.x - x;
        }
        else
        {
            return x - f.pos.x;
        }
    }
    private void Moves(int toward)
    {
        if (toward == -1)
        {

            AnimaSpeed = status.WalkSpeed.to_float();
            f.pos.x = f.pos.x - Dt.dt * status.WalkSpeed;
            AnimaToward = -1f;
        }
        else if (toward == 1)
        {
            AnimaSpeed = status.WalkSpeed.to_float();
            f.pos.x = f.pos.x + Dt.dt * status.WalkSpeed;
            AnimaToward = 1f;
        }
        else
        {
            AnimaSpeed = 0f;
        }
    }
    private void Roll()
    {
        RemoveHited();
        if (StatusTime > new Fixpoint(1, 0))
        {
            ChangeStatus(StatusType.Normal);
        }
        if (AnimaToward == 1.0f)
        {
            f.pos.x = f.pos.x + Dt.dt * status.WalkSpeed;
        }
        else
        {
            f.pos.x = f.pos.x - Dt.dt * status.WalkSpeed;
        }
    }

    private bool CreatedAttack = false;
    private Fixpoint Attack1DuringTime = new Fixpoint(1, 0);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(33, 2);
    private Fixpoint Attack3DuringTime = new Fixpoint(33, 2);
    private Fixpoint Attack4DuringTime = new Fixpoint(58, 2);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(83, 2);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(8, 2);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(8, 2);
    private Fixpoint Attack4BeginToHitTime = new Fixpoint(17, 2);

    private Fixpoint Attack1Damage = new Fixpoint(2, 0);
    private Fixpoint Attack2Damage = new Fixpoint(3, 0);
    private Fixpoint Attack3Damage = new Fixpoint(4, 0);
    private Fixpoint Attack4Damage = new Fixpoint(5, 0);
    private void AttackToNext()
    {
        CreatedAttack = false;
        AnimaAttack = AnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }

    private void MonsterCreateAttack(Fixpoint damage)
    {
        CreatedAttack = true;
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * damage, 30, AnimaToward, 3, "");//最后一个参数是击飞类型

    }
    private void RemoveAttack()
    {
        AnimaAttack = 0f;
        ChangeStatus(StatusType.Normal);
        return;
    }

    private string[] Music = new string[4]
    {
        "啊？！（王）","来！","易建联~","Fack you~（van）"
    };

    private void Attack(bool first)
    {
        int hited = BasicCharacterGetHited();
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
        else if (AnimaAttack > 0.5f && AnimaAttack <= 1.5f)
        {
            if (StatusTime > Attack1DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack1BeginToHitTime && CreatedAttack == false)
            {
                PlayMusic(Music[Random.Range(0, 4)]);
                MonsterCreateAttack(Attack1Damage);
            }
        }
        else if (AnimaAttack > 1.5f && AnimaAttack <= 2.5f)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && CreatedAttack == false)
            {
                PlayMusic(Music[Random.Range(0, 4)]);
                MonsterCreateAttack(Attack2Damage);
            }
        }
        else if (AnimaAttack > 2.5f && AnimaAttack <= 3.5f)
        {
            if (StatusTime > Attack3DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && CreatedAttack == false)
            {
                PlayMusic(Music[Random.Range(0, 4)]);
                MonsterCreateAttack(Attack3Damage);
            }
        }
        else if (AnimaAttack > 3.5f)
        {
            if (StatusTime > Attack4DuringTime)
            {
                RemoveAttack();
            }
            if (StatusTime > Attack4BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack4Damage);
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
    private void Death()
    {
        if (StatusTime == Dt.dt)
        {
            PlayMusic("啊~~~（王）");
        }
        if(f.onground)
        {
            r.velocity = new Fix_vector2(0, 0);
        }
        if (StatusTime > new Fixpoint(3, 0))
        {
            if (type2 == 1)
            {
                Flow_path.zombie_cnt--;
            }
            DeathFall();
            Main_ctrl.Desobj(id);
        }
    }
    // Update is called once per frame
    void Update()
    {
        SetBlueAndRed();
        if (ToHomeFlag == true)
        {
            ToHome();
            BeAngry();
        }

        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetInteger("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
}
