using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Attack2 : Attack
{
    private Animator animator;
    private bool AnimaDestroy = false;

    private Fixpoint MaxAliveTime = new Fixpoint(10, 0);
    private Fixpoint DestroyTime = new Fixpoint(95, 1);
    private Fixpoint FlySpeed = new Fixpoint(15, 0);

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetDestroyTime(Fixpoint x)
    {
        DestroyTime = MaxAliveTime - x;
    }
    public void SetAliveTime(Fixpoint x)
    {
        MaxAliveTime = x;
    }
    public void SetSpeed(Fixpoint x)
    {
        FlySpeed = x;
    }

    public override void Updatex()
    {
        AliveTime += Dt.dt;
        GetHited();
        if(AliveTime <= DestroyTime)//2.5s开始播放动画，停止移动，一开全要开
        {
            if(toward < 0)
            {
                f.pos.x -= FlySpeed * Dt.dt;     //气功波的飞行运动速度10/s
            } else
            {
                f.pos.x += FlySpeed * Dt.dt;
            }
        }
        else if (AliveTime > DestroyTime && AliveTime < MaxAliveTime )//2.5s开始播放销毁动画，3s是逻辑上气功波消失的时间
        {
            AnimaDestroy = true;
        } else if(AliveTime > MaxAliveTime)
        {
            Main_ctrl.Desobj(id);
        }

        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0f);
    }
    
    public void DestroySelf()
    {
        if (AliveTime < DestroyTime)
        {
            AliveTime = DestroyTime;
        }
    }

    void GetHited()
    {
        while (f.actions.Count > 0)
        {
            Fix_col2d_act fa = f.actions.Dequeue();
            if (fa.type == Fix_col2d_act.col_action.On_wall)
            {
                if(AliveTime < DestroyTime)
                {
                    AliveTime = DestroyTime;
                }
            }
        }
    }

    void Update()
    {
        animator.SetBool("destroy",AnimaDestroy);
        animator.SetFloat("toward", toward);
    }


}
