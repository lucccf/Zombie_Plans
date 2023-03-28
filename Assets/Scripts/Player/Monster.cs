using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Monster : MonoBehaviour
{
    public long id;

    public Fix_col2d f;
    public Fix_rig2d r;
    //public SpriteRenderer spriteRenderer;
    private PlayerStatus status = new PlayerStatus(100, 10);
    //private Fixpoint WalkSpeed = new Fixpoint(5, 0);

    private Animator animator;
    private int AnimaStatus = 0;
    private float AnimaToward = 1f;
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private float AnimaHited = 0f;
    private bool AnimaRoll = false;
    private bool AnimaGround = false;
    private Fixpoint StatusTime = new Fixpoint(0, 0);

    //private Fixpoint HpDamage;
    //private int ToughnessDamage;

    private Player player = null;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Updatex()
    {
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(25,0));
        if(AnimaStatus != 6)CheckDeath();
        switch(AnimaStatus)
        {
            case 0:
                Normal();
                break;
            case 1:
                Roll();
                break;
            case 2:
                Attack(false);
                break;
            case 3:
                Hited();
                break;
            case 4:
                HitedFly();
                break;
            case 5:
                HitedOnGround();
                break;
            case 6:
                Death();
                break;
        }
        //Debug.Log("Toughness:" + status.GetToughness());
        //Debug.Log("Status:" + AnimaStatus);
        //Debug.Log("HP:" + status.hp);
    }
    private void CheckDeath()
    {
        if(status.death == true)
        {
            AnimaAttack = 0f;
            AnimaHited = 0f;
            AnimaRoll = false;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 6;
        }
    }
    private void Normal()
    {
        int hited = GetHited();
        if (hited != 0) return;

        Fixpoint Pos = GetNear();
        Fixpoint Dis = GetNearDistance(Pos);
        if (Dis > new Fixpoint(15, 0)) // 巡逻
        {
            if (StatusTime > new Fixpoint(2, 0)) StatusTime -= new Fixpoint(2, 0);
            if (StatusTime > new Fixpoint(1, 0)) Moves(1);
            else Moves(-1);
            return;
        }
        else if (Dis < new Fixpoint(15, 1)) //攻击
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
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 1;
            AnimaRoll = true;
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
            return;
        }
        else //靠近
        {
            if (f.pos.x + new Fixpoint(2, 0) < Pos)
            {
                Moves(1);
            }
            else if (f.pos.x - new Fixpoint(2, 0) > Pos)
            {
                Moves(-1);
            }
        }
    }
    private Fixpoint GetNear()
    {
        List<Fixpoint> list = new List<Fixpoint>();
        foreach (Player i in Player_ctrl.plays)
        {
            if (i.f.pos.y - f.pos.y <= new Fixpoint(1, 0) && i.f.pos.y - f.pos.y >= new Fixpoint(-1, 0))
            {
                list.Add(i.f.pos.x);
            }
        }
        if (list.Count > 0)
        {
            Fixpoint min = new Fixpoint(114514, 0);
            Fixpoint ans = new Fixpoint(0, 0);
            foreach (Fixpoint i in list)
            {
                Fixpoint x = f.pos.x, y = i;
                if (x < y)
                {
                    Fixpoint t = x;
                    x = y;
                    y = t;
                }
                if (x - y < min)
                {
                    min = x - y;
                    ans = i;
                }
            }
            return ans;
        }
        else return new Fixpoint(10000,0);
    }
    private Fixpoint GetNearDistance()
    {
        Fixpoint x = GetNear();
        if(f.pos.x > x)
        {
            return f.pos.x - x;
        } else
        {
            return x - f.pos.x;
        }
    }
    private Fixpoint GetNearDistance(Fixpoint x)
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
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaRoll = false;
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
            if (a.type != Fix_col2d_act.col_action.Attack) continue;
            ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
            long AttackId = a.opsite.id;
            if (!Main_ctrl.All_objs.ContainsKey(AttackId)) continue;
            Attack attack = (Attack)(Main_ctrl.All_objs[AttackId].modules[Object_ctrl.class_name.Attack]);
            if (a.type != Fix_col2d_act.col_action.Attack) continue;
            if (attack.attakcer_id == id) continue;
            //Debug.Log("GetHited");

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
            AnimaStatus = 3;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 50 && status.GetToughness() >= 25)
        {
            AnimaHited = 2;
            AnimaStatus = 3;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 25 && status.GetToughness() >= 0)
        {
            AnimaHited = 3;
            AnimaStatus = 3;
            StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else
        {
            AnimaHited = 4;
            AnimaStatus = 4;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(5, 0));
            StatusTime = new Fixpoint(0, 0);
            return 2;
        }
    }
    private void Hited()
    {
        int hited = GetHited();
        if(hited == 0)
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
            AnimaStatus = 5;
            AnimaHited = 0;
            AnimaGround = true;
            status.RecoverToughness(new Fixpoint(1000, 0));
            return;
        }
        if (AnimaToward > 0)
        {
            f.pos.x -= (new Fixpoint(6, 0) - new Fixpoint(4,0) * StatusTime) * Dt.dt ;
        }
        else
        {
            f.pos.x += (new Fixpoint(6, 0) - new Fixpoint(4, 0) * StatusTime) * Dt.dt;
        }
    }
    private void HitedOnGround()
    {
        if (StatusTime > new Fixpoint(1, 0))
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaHited = 0;
            AnimaGround = false;
        }
    }
    private void Attack(bool first)
    {
        int hited = GetHited();
        if (hited != 0) {
            AnimaAttack = 0;
            return; 
        }
        
        Fixpoint Near = GetNearDistance();
        if (first == true || StatusTime > new Fixpoint(1, 0))
        {
            if (Near > new Fixpoint(15, 1) || AnimaAttack > 3.5f)
            {
                StatusTime = new Fixpoint(0, 0);
                AnimaAttack = 0;
                AnimaStatus = 0;
                return;
            }
            else
            {
                Fix_vector2 AttackPos = f.pos.Clone();
                if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
                else AttackPos.x -= new Fixpoint(1, 0);
                Main_ctrl.NewAttack(AttackPos, new Fixpoint(2, 0), new Fixpoint(2, 0), status.Damage(), 50, id);
                ++AnimaAttack;
                StatusTime = new Fixpoint(0, 0);
            }
        }

    }
    private void Death()
    {
        AnimaGround = true;
        //Debug.Log(StatusTime.to_float());
        if(StatusTime > new Fixpoint(3,0))
        {
            Debug.Log("Death");
            Main_ctrl.Desobj(id);
            //AnimaStatus = 100;
        }
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetFloat("hited", AnimaHited);
        animator.SetBool("roll", AnimaRoll);
        animator.SetBool("ground", AnimaGround);
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
}
