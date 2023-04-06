using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BasicCharacter
{

    private Animator animator;
    private float AnimaToward = 1f;
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private float AnimaHited = 0f;

    private Player player = null;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Updatex()
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
    }
    public float CheckHealth()
    {
        return 1f * status.hp / status.max_hp;
    }
    private void CheckDeath()
    {
        if(status.death == true)
        {
            AnimaAttack = 0f;
            AnimaHited = 0f;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 6;
        }
    }
    private void Normal()
    {
        int hited = MonsterGetHited();
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
        else if (Dis < new Fixpoint(14, 1)) //攻击
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
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
            return;
        }
        else //靠近
        {
            if (f.pos.x  < Pos)
            {
                Moves(1);
            }
            else
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

    private int CheckToughStatus(bool this_hited)
    {
        if (status.GetToughness() >= 75)
        {
            AnimaHited = 0;
            return 0;
        }
        else if (status.GetToughness() < 75 && status.GetToughness() >= 50)
        {
            AnimaHited = 1;
            AnimaStatus = 3;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 50 && status.GetToughness() >= 25)
        {
            AnimaHited = 2;
            AnimaStatus = 3;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 25 && status.GetToughness() >= 0)
        {
            AnimaHited = 3;
            AnimaStatus = 3;
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else
        {
            AnimaHited = 4;
            AnimaStatus = 4;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(5, 0));
            if (this_hited == true)
                StatusTime = new Fixpoint(0, 0);
            return 2;
        }
    }
    
    private void Preform(int damage)
    {
        GameObject beat = (GameObject)AB.getobj("beat");
        beat.transform.localScale = new Vector3(3f, 3f, 1f);
        Instantiate(beat, transform.position, transform.rotation);
        GameObject num = (GameObject)AB.getobj("HurtNumber");
        GameObject num2 = Instantiate(num, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        num2.GetComponent<BeatNumber>().ChangeNumber(damage);
    }

    private int MonsterGetHited()
    {
        bool x =GetHited(AnimaToward);
        return CheckToughStatus(x);
    }

    private void Hited()
    {
        int hited = MonsterGetHited();
        if(hited == 0)
        {
            AnimaHited = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            return;
        }
        if (StatusTime < new Fixpoint(2,1))
        {
            if (AnimaToward > 0)
            {
                f.pos.x = f.pos.x - Dt.dt * new Fixpoint(1, 1);
            }
            else
            {
                f.pos.x = f.pos.x + Dt.dt * new Fixpoint(1, 1);
            }
        }

    }
    private void HitedFly()
    {
        if (StatusTime > new Fixpoint(15, 1) && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 5;
            AnimaHited = 0;
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
        RemoveHited();
        if (StatusTime > new Fixpoint(1, 0))
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaHited = 0;
        }
    }

    private bool CreatedAttack = false;
    private Fixpoint Attack1DuringTime = new Fixpoint(78, 2);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(39, 2);
    private Fixpoint Attack3DuringTime = new Fixpoint(39, 2);
    private Fixpoint Attack4DuringTime = new Fixpoint(79, 2);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(55, 2);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(13, 2);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(13, 2);
    private Fixpoint Attack4BeginToHitTime = new Fixpoint(27, 2);

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
        CreateAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * damage, 30, AnimaToward);

    }
    private void RemoveAttack()
    {
        AnimaAttack = 0f;
        StatusTime = new Fixpoint(0, 0);
        AnimaStatus = 0;
        return;
    }

    private void Attack(bool first)
    {
        int hited = MonsterGetHited();
        if (hited != 0) {
            AnimaAttack = 0;
            return; 
        }
        
        Fixpoint Near = GetNearDistance();
        if(first == true)
        {
            AttackToNext();
        } else if(AnimaAttack > 0.5f && AnimaAttack <= 1.5f)
        {
            if(StatusTime > Attack1DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack1BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack1Damage);
        } else if (AnimaAttack > 1.5f && AnimaAttack <= 2.5f)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack2Damage);
        } else if (AnimaAttack > 2.5f && AnimaAttack <= 3.5f)
        {
            if (StatusTime > Attack3DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && CreatedAttack == false) MonsterCreateAttack(Attack3Damage);
        } else if(AnimaAttack > 3.5f)
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
        if(StatusTime > new Fixpoint(3,0))
        {
            DeathFall("Medicine",3,1f);
            Main_ctrl.Desobj(id);
        }
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetFloat("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
}
