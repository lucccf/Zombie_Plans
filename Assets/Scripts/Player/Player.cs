using Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 玩家状态
    private PlayerStatus status;

    //碰撞体变量
    public Fix_col2d f;
    public Fix_rig2d r;
    //public SpriteRenderer spriteRenderer;

    //动画变量

    private Fixpoint WalkSpeed = new Fixpoint(5, 0);
    private Fixpoint RunSpeed = new Fixpoint(10, 0);

    private Animator animator;
    private int AnimaStatus = 0;
    private float AnimaToward = 1f;
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private bool AnimaFall = false;
    private bool AnimaJump = false;
    private bool AnimaRoll = false;
    private Fixpoint StatusTime = new Fixpoint(0, 0);
    public long id;

    //攻击区域
    public GameObject attack_space;


    public bool Active = false;
    //临时
    //private float GravitySpeed = 5f;
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
        //Debug.Log("Updatex");
        Active = true;
        StatusTime += Dt.dt;
       
        switch (AnimaStatus)
        {
            case 0:
                //站立，走路，跑步
                if (Press[KeyCode.A])
                {
                    if (Press[KeyCode.LeftShift])
                    {
                        AnimaSpeed = RunSpeed.to_float();
                        f.pos.x = f.pos.x - Dt.dt * RunSpeed;
                    }
                    else
                    {
                        AnimaSpeed = WalkSpeed.to_float();
                        f.pos.x = f.pos.x - Dt.dt * WalkSpeed;
                    }
                    AnimaToward = -1f;
                    transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
                }
                else if (Press[KeyCode.D])
                {
                    if (Press[KeyCode.LeftShift])
                    {
                        AnimaSpeed = RunSpeed.to_float();
                        f.pos.x = f.pos.x + Dt.dt * RunSpeed;
                    }
                    else
                    {
                        AnimaSpeed = WalkSpeed.to_float();
                        f.pos.x = f.pos.x + Dt.dt * WalkSpeed;
                    }
                    AnimaToward = 1f;
                    transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
                } else
                {
                    AnimaSpeed = 0f;
                }
                if(!f.onground)
                {
                    AnimaFall = true;
                    AnimaStatus = 6;
                    StatusTime = new Fixpoint(0, 0);
                    break;

                }
                if (Press[KeyCode.J] && f.onground)
                {
                    AnimaAttack = 1f;
                    StatusTime = new Fixpoint(0, 0);
                    AnimaStatus = 3;
                    break;
                }
                if (Press[KeyCode.L] && f.onground)
                {
                    StatusTime = new Fixpoint(0, 0);
                    AnimaRoll = true;
                    AnimaStatus = 2;
                    break;
                }
                if (Press[KeyCode.K] && f.onground)
                {
                    StatusTime = new Fixpoint(0, 0);
                    r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(15, 0));
                    AnimaJump = true;
                    AnimaStatus = 1;
                    break;
                }
                break;
            case 1:
                //跳跃
                if (StatusTime > new Fixpoint(2,1) && f.onground)
                {
                    AnimaStatus = 0;
                    StatusTime = new Fixpoint(0, 0);
                    AnimaJump = false;
                    break;
                }
                if (Press[KeyCode.A]) {
                    AnimaToward = -1;
                    f.pos.x = f.pos.x - Dt.dt * WalkSpeed;
                } else if (Press[KeyCode.D]) {
                    AnimaToward = 1;
                    f.pos.x = f.pos.x + Dt.dt * WalkSpeed;
                }
                if(StatusTime > new Fixpoint(5,1) && !f.onground)
                {
                    AnimaStatus = 6;
                    StatusTime = new Fixpoint(0, 0);
                    AnimaJump = false;
                    AnimaFall = true;
                }
                break;
            case 2:
                //翻滚
                if (StatusTime > new Fixpoint(1, 0))
                {
                    AnimaStatus = 0;
                    StatusTime = new Fixpoint(0, 0);
                    AnimaRoll = false;
                }
                if(AnimaToward == 1.0f)
                {
                    f.pos.x = f.pos.x + Dt.dt * WalkSpeed;
                }
                else
                {
                    f.pos.x = f.pos.x - Dt.dt * WalkSpeed;
                }
                break;
            case 3:
                /*
                //攻击
                if (Press[KeyCode.J] && StatusTime > new Fixpoint(1,0) && AnimaAttack < 4.5f)
                {
                    AnimaAttack = AnimaAttack + 1;
                    StatusTime = new Fixpoint(0, 0);
                    Fix_vector2 AttackPos = f.pos.Clone();
                    if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
                    else AttackPos.x -= new Fixpoint(1, 0);
                    Main_ctrl.NewAttack(AttackPos , new Fixpoint(1,0), new Fixpoint(1,0));
                    break;
                }
                if(StatusTime > new Fixpoint(15,1))
                {
                    AnimaAttack = 0f;
                    StatusTime = new Fixpoint(0, 0);
                    AnimaStatus = 0;
                    break;
                }
                break;
                */
            case 4:
                //受击
                
                break;
            case 5:
                //击飞
                break;
            case 6://下落
                if (Press[KeyCode.A] == true)
                {
                    AnimaToward = -1;
                    f.pos.x = f.pos.x - Dt.dt * WalkSpeed;
                } else if (Press[KeyCode.D] == true)
                {
                    AnimaToward = 1;
                    f.pos.x = f.pos.x + Dt.dt * WalkSpeed;
                }
                if(f.onground)
                {
                    AnimaStatus = 0;
                    AnimaFall = false;
                    StatusTime = new Fixpoint(0, 0);
                    break;
                }
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
        //transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
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
        //Debug.Log(f.pos.x.to_float() + " " + f.pos.y.to_float());
    }
}
