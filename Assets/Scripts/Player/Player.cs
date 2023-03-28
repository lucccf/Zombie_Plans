﻿using Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 玩家状态
    private PlayerStatus status = new PlayerStatus(100,10);

    //碰撞体变量
    public Fix_col2d f;
    public Fix_rig2d r;
    //public SpriteRenderer spriteRenderer;

    //动画变量

    //private Fixpoint WalkSpeed = new Fixpoint(5, 0);
    //private Fixpoint RunSpeed = new Fixpoint(10, 0);

    private Animator animator;
    private int AnimaStatus = 0;
    private float AnimaToward = 1f;
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private float AnimaHited = 0f;
    private bool AnimaFall = false;
    private bool AnimaJump = false;
    private bool AnimaRoll = false;
    private bool AnimaDeath = false;
    private bool AnimaGround = false;
    private Fixpoint StatusTime = new Fixpoint(0, 0);
    public long id;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    HashSet<PlayerOpt> list;
    Dictionary<KeyCode, bool> Press = new Dictionary<KeyCode, bool>()
    {
        [KeyCode.A] = false,
        [KeyCode.D] = false,
        [KeyCode.J] = false,
        [KeyCode.K] = false,
        [KeyCode.L] = false,
        [KeyCode.Q] = false,
        [KeyCode.E] = false,
        [KeyCode.LeftShift] = false,
        [KeyCode.Space] = false
    };

    public void DealInputs(PlayerOptData inputs)
    {
        //Debug.Log(inputs);
        switch (inputs.Opt)
        {
            case PlayerOpt.ADown:
                Press[KeyCode.A] = true;
                break;
            case PlayerOpt.AUp:
                Press[KeyCode.A] = false;
                break;
            case PlayerOpt.DDown:
                Press[KeyCode.D] = true;
                break;
            case PlayerOpt.DUp:
                Press[KeyCode.D] = false;
                break;
            case PlayerOpt.JDown:
                Press[KeyCode.J] = true;
                break;
            case PlayerOpt.JUp:
                Press[KeyCode.J] = false;
                break;
            case PlayerOpt.KDown:
                Press[KeyCode.K] = true;
                break;
            case PlayerOpt.KUp:
                Press[KeyCode.K] = false;
                break;
            case PlayerOpt.LDown:
                Press[KeyCode.L] = true;
                break;
            case PlayerOpt.LUp:
                Press[KeyCode.L] = false;
                break;
            case PlayerOpt.QDown:
                Press[KeyCode.Q] = true;
                break;
            case PlayerOpt.QUp:
                Press[KeyCode.Q] = false;
                break;
            case PlayerOpt.EDown:
                Press[KeyCode.E] = true;
                break;
            case PlayerOpt.EUp:
                Press[KeyCode.E] = false;
                break;
            case PlayerOpt.ShiftDown:
                Press[KeyCode.LeftShift] = true;
                break;
            case PlayerOpt.ShiftUp:
                Press[KeyCode.LeftShift] = false;
                break;
            case PlayerOpt.SpaceDown:
                Press[KeyCode.Space] = true;
                break;
            case PlayerOpt.SpaceUp:
                Press[KeyCode.Space] = false;
                break;
        }
    }
    public void Updatex()
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
                //翻滚
                Roll();
                break;
            case 3:
                Attack(false);
                break;
            case 4:
                //受击
                Hited();
                break;
            case 5:
                //击飞
                HitedFly();
                break;
            case 6://下落
                Fall();
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
        //transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
    private void Normal()
    {
        //站立，走路，跑步
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaSpeed = 0f;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if (Press[KeyCode.A])
        {
            if (Press[KeyCode.LeftShift])
            {
                AnimaSpeed = status.RunSpeed.to_float();
                f.pos.x = f.pos.x - Dt.dt * status.RunSpeed;
            }
            else
            {
                AnimaSpeed = status.WalkSpeed.to_float();
                f.pos.x = f.pos.x - Dt.dt * status.WalkSpeed;
            }
            AnimaToward = -1f;
            transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
        }
        else if (Press[KeyCode.D])
        {
            if (Press[KeyCode.LeftShift])
            {
                AnimaSpeed = status.RunSpeed.to_float();
                f.pos.x = f.pos.x + Dt.dt * status.RunSpeed;
            }
            else
            {
                AnimaSpeed = status.WalkSpeed.to_float();
                f.pos.x = f.pos.x + Dt.dt * status.WalkSpeed;
            }
            AnimaToward = 1f;
            transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
        }
        else
        {
            AnimaSpeed = 0f;
        }
        if (!f.onground)
        {
            AnimaFall = true;
            AnimaStatus = 6;
            StatusTime = new Fixpoint(0, 0);
            return;

        }
        if (Press[KeyCode.J] && f.onground)
        {
            //AnimaAttack = 1f;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 3;
            Attack(true);
            return;
        }
        if (Press[KeyCode.L] && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaRoll = true;
            AnimaStatus = 2;
            return;
        }
        if (Press[KeyCode.K] && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(15, 0));
            AnimaJump = true;
            AnimaStatus = 1;
            return;
        }
    }

    void Jump()
    {
        //跳跃
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaJump = false;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if (StatusTime > new Fixpoint(2, 1) && f.onground)
        {
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaJump = false;
            return;
        }
        if (Press[KeyCode.A])
        {
            AnimaToward = -1;
            f.pos.x = f.pos.x - Dt.dt * status.WalkSpeed;
        }
        else if (Press[KeyCode.D])
        {
            AnimaToward = 1;
            f.pos.x = f.pos.x + Dt.dt * status.WalkSpeed;
        }
        if (StatusTime > new Fixpoint(5, 1) && !f.onground)
        {
            AnimaStatus = 6;
            StatusTime = new Fixpoint(0, 0);
            AnimaJump = false;
            AnimaFall = true;
        }
        return;
    }
    private void Roll()
    {
        RemoveHited();
        Debug.Log(StatusTime.to_float());
        if (StatusTime > new Fixpoint(1, 0))
        {
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaRoll = false;
            return;
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
    private void Attack(bool first)
    {
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if (first == true || (Press[KeyCode.J] && StatusTime > new Fixpoint(5, 1) && AnimaAttack < 4.5f) )
        {
            AnimaAttack = AnimaAttack + 1;
            StatusTime = new Fixpoint(0, 0);
            Fix_vector2 AttackPos = f.pos.Clone();
            if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
            else AttackPos.x -= new Fixpoint(1, 0);
            Main_ctrl.NewAttack(AttackPos, new Fixpoint(2, 0), new Fixpoint(2, 0), status.Damage(), 50 ,id);
            return;
        }
        if (StatusTime > new Fixpoint(8, 1))
        {
            AnimaAttack = 0f;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            return;
        }
    }
    private void Fall()
    {
        if (Press[KeyCode.A] == true)
        {
            AnimaToward = -1;
            f.pos.x = f.pos.x - Dt.dt * status.WalkSpeed;
        }
        else if (Press[KeyCode.D] == true)
        {
            AnimaToward = 1;
            f.pos.x = f.pos.x + Dt.dt * status.WalkSpeed;
        }
        if (f.onground)
        {
            AnimaStatus = 0;
            AnimaFall = false;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
    }
    private void RemoveHited()
    {
        if (((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Count > 0)
        {
            ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
        }
    }
    private int GetHited()
    {
        while (((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Count > 0)
        {
            Fix_col2d_act a = ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Peek();
            ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
            long AttackId = a.opsite.id;
            if (!Main_ctrl.All_objs.ContainsKey(AttackId)) continue;
            Attack attack = (Attack)(Main_ctrl.All_objs[AttackId].modules[Object_ctrl.class_name.Attack]);

            if (attack.attakcer_id == id) continue;
            Debug.Log(attack.attakcer_id + " " + id);

            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;
            status.GetAttacked(HpDamage, ToughnessDamage);
        }
        if (status.GetToughness() >= 75)
        {
            return 0;
        }
        else if (status.GetToughness() < 75 && status.GetToughness() >= 50)
        {
            AnimaHited = 1;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 50 && status.GetToughness() >= 25)
        {
            AnimaHited = 2;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 25 && status.GetToughness() >= 0)
        {
            AnimaHited = 3;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else
        {
            AnimaHited = 4;
            AnimaStatus = 5;
            //AnimaStatus = 4;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(5, 0));
            StatusTime = new Fixpoint(0, 0);
            return 2;
        }
    }

    private void Hited()
    {
        int hited = GetHited();
        if (hited == 0)
        {
            AnimaHited = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            return;
        }

    }

    private void HitedFly()
    {
        if (StatusTime > new Fixpoint(15, 1) && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaHited = 0;
            AnimaGround = false;
            status.RecoverToughness(new Fixpoint(1000, 0));
            return;
        } else if (StatusTime > new Fixpoint(15, 1))
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 6;
            AnimaHited = 0;
            AnimaGround = false;
            AnimaFall = true;
            status.RecoverToughness(new Fixpoint(1000, 0));
            return;
        }
        if (AnimaToward > 0)
        {
            f.pos.x -= (new Fixpoint(6, 0) - new Fixpoint(4, 0) * StatusTime) * Dt.dt;
        }
        else
        {
            f.pos.x += (new Fixpoint(6, 0) - new Fixpoint(4, 0) * StatusTime) * Dt.dt;
        }
    }

    private void Update()
    {
        //Debug.Log("zzz");
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetBool("jump", AnimaJump);
        animator.SetBool("roll", AnimaRoll);
        animator.SetBool("fall", AnimaFall);
        animator.SetFloat("hited", AnimaHited);
        animator.SetBool("onground", AnimaGround);
    }
}
