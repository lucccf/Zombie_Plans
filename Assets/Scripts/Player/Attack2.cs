using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : Attack
{
    private Animator animator;
    private bool AnimaDestroy = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public override void Updatex()
    {
        AliveTime += Dt.dt;
        GetHited();
        if(AliveTime <= new Fixpoint(25,1))//2.5s开始播放动画，停止移动，一开全要开
        {
            if(toward < 0)
            {
                f.pos.x -= new Fixpoint(10, 0) * Dt.dt;     //气功波的飞行运动速度10/s
            } else
            {
                f.pos.x += new Fixpoint(10, 0) * Dt.dt;
            }
        }
        else if (AliveTime > new Fixpoint(25, 1) && AliveTime <= new Fixpoint(3,0))//2.5s开始播放销毁动画，3s是逻辑上气功波消失的时间
        {
            AnimaDestroy = true;
        } else if(AliveTime > new Fixpoint(3,0))
        {
            Main_ctrl.Desobj(id);
        }

        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0f);
    }
    
    public void DestroySelf()
    {
        if (AliveTime < new Fixpoint(25, 1))
        {
            AliveTime = new Fixpoint(25, 1);
        }
    }
    void GetHited()
    {
        while (f.actions.Count > 0)
        {
            Fix_col2d_act fa = f.actions.Dequeue();
            if (fa.type == Fix_col2d_act.col_action.On_wall)
            {
                if(AliveTime < new Fixpoint(25, 1))
                {
                    AliveTime = new Fixpoint(25, 1);
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
