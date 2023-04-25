using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Monster
{
    protected float KnightAnimaSpeed = 0f;
    protected int KnightAnimaAttack = 0;
    protected int KnightAnimaHited = 0;

    private Fixpoint KnightSkillCD = new Fixpoint(0, 0);
    private static Fixpoint KnightSkillCD_MAX = new Fixpoint(10, 0);

    public override void InitStatic()
    {
        Attack1DuringTime = new Fixpoint(59, 2);//攻击的持续时间
        Attack2DuringTime = new Fixpoint(61, 2);
        Attack3DuringTime = new Fixpoint(64, 2);

        Attack1BeginToHitTime = new Fixpoint(33, 2);//攻击的判定时间
        Attack2BeginToHitTime = new Fixpoint(2, 1);
        Attack3BeginToHitTime = new Fixpoint(25, 2);

        Attack1Damage = new Fixpoint(3, 0);//伤害倍率
        Attack2Damage = new Fixpoint(3, 0);
        Attack3Damage = new Fixpoint(3, 0);

        DefenceTime = new Fixpoint(1, 0);//防御时间
        DefenceRate = new Fixpoint(5, 1);//承受伤害倍率

        SkillBeginToHitTime = new Fixpoint(133, 2);//技能的结算开始时间
        SkillDruingTime = new Fixpoint(163, 2);
        SkillBetweenTime = new Fixpoint(21, 2);//伤害的间隔
        SkillAttackRate = new Fixpoint(3, 0);//攻击倍率

        KnightSkillCD_MAX = new Fixpoint(10, 0);
    }
    public override void InitNormal()
    {
        status.attack = 10;//基础攻击力
        status.WalkSpeed = new Fixpoint(5, 0);//走路速度
        status.max_hp = 930;//最大血量
        status.hp = 930;//血量
        status.max_toughness = 100;//最大韧性值
        status.toughness = 100;//韧性值
        HitTime = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(6, 1) };
        ToughnessStatus = new int[2] { 20, 0 };//阶段
    }
    public override void Startx()
    {
        CharacterType = 1 + type2;
        SetStatus(930, 10);//血量，基础攻击力
        animator = GetComponent<Animator>();
        HitTime = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(6, 1) };
        ToughnessStatus = new int[2] { 20, 0 };//阶段
        audiosource = GetComponent<AudioSource>();
        SetFindStatus();
        HitMisuc = "骑士挨打";
        //ToHome();

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", KnightAnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("attack", KnightAnimaAttack);
        animator.SetInteger("hited", KnightAnimaHited);
        animator.SetInteger("status", AnimaStatus);
        SetBlueAndRed();
        if (ToHomeFlag == true)
        {
            ToHome();
            BeAngry();
        }
    }

    public override void Updatex()
    {
        KnightSkillCD -= Dt.dt;
        NormalUpdate();

        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if (status.death == true && AnimaStatus != 8)
        {
            ChangeStatus(StatusType.Death);
        }
        if (RealStatus != StatusType.Hit && RealStatus != StatusType.Ground && RealStatus != StatusType.Death && RealStatus != StatusType.Defence)
        {
            int hited = BasicCharacterGetHited();
            if (hited != 0)
            {
                KnightAnimaAttack = 0;
                SkillAttackTimes = 0;
            }
        }
        StatusTime += Dt.dt;
        switch (RealStatus)
        {
            case StatusType.Normal:
                AnimaStatus = 0;
                Normal();
                break;
            case StatusType.Jump:
                AnimaStatus = 1;
                Jump(0);
                break;
            case StatusType.Attack:
                AnimaStatus = 2;
                Attack(false);
                break;
            case StatusType.Defence:
                AnimaStatus = 3;
                Defence();
                break;
            case StatusType.Skill:
                AnimaStatus = 4;
                Skill();
                break;
            case StatusType.Hit:
                AnimaStatus = 5;
                Hited();
                break;
            case StatusType.Ground:
                AnimaStatus = 6;
                Ground();
                break;
            case StatusType.Fall:
                AnimaStatus = 7;
                Fall();
                break;
            case StatusType.Death:
                AnimaStatus = 8;
                Death();
                break;
            case StatusType.Search:
                AnimaStatus = 9;
                Search();
                break;
            case StatusType.LittleJump:
                AnimaStatus = 10;
                LittleJump(0);
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }

    private int DefenceGetHited()
    {

        GetColider();

        bool this_hited = false;
        while (AttackQueue.Count > 0)
        {
            Fix_col2d_act a = AttackQueue.Peek();
            AttackQueue.Dequeue();
            if (a.type != Fix_col2d_act.col_action.Attack)
            {
                Debug.Log("Attack Geted Error");
                continue;
            }
            long AttackId = a.opsite.id;
            if (!Main_ctrl.All_objs.ContainsKey(AttackId)) continue;
            Attack attack = (Attack)(Main_ctrl.All_objs[AttackId].modules[Object_ctrl.class_name.Attack]);
            if (attack.attacker_type == CharacterType) continue;

            if (attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                if (attack2.toward * AnimaToward < 0)
                {
                    PlayMusic("格挡成功");
                    Main_ctrl.NewAttack2("LightBall", f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), attack2.HpDamage, attack2.ToughnessDamage, id, AnimaToward, CharacterType, attack.hited_fly_type, "气功波命中");
                    Main_ctrl.Desobj(attack2.id);
                }
                else
                {
                    PlayMusic(attack.MusicName);
                    status.GetAttacked(attack.HpDamage / DefenceRate, attack.ToughnessDamage);
                    attack2.DestroySelf();
                    Preform(status.last_damage);
                }
                continue;
            }

            this_hited = true;

            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;

            if (attack.toward * AnimaToward < 0)
            {
                PlayMusic("格挡成功");
                status.GetAttacked(HpDamage, ToughnessDamage / 2);
            }
            else
            {
                PlayMusic(attack.MusicName);
                status.GetAttacked(HpDamage / DefenceRate, ToughnessDamage);
            }

            Preform(status.last_damage);
        }
        if (this_hited)
        {
            return CheckToughnessStatus();
        }
        else
        {
            return 0;
        }
    }

    private void Normal()
    {
        KnightAnimaSpeed = 5f;

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
                else if (Dis > new Fixpoint(3, 0) && Dis < new Fixpoint(5, 0))//距离判定
                {
                    if (Rand.rand() % 2 == 0)
                    {
                        ChangeStatus(StatusType.Defence);//随机防御
                    }
                    else if (KnightSkillCD <= new Fixpoint(0, 0))
                    {
                        KnightSkillCD = KnightSkillCD_MAX.Clone();
                        ChangeStatus(StatusType.Skill);//随机技能
                    }
                    else
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
                SearchX(Pos);
            }
        }
    }


    private bool KnightCreatedAttack = false;
    private Fixpoint Attack1DuringTime = new Fixpoint(59, 2);//攻击的持续时间
    private Fixpoint Attack2DuringTime = new Fixpoint(61, 2);
    private Fixpoint Attack3DuringTime = new Fixpoint(64, 2);

    private Fixpoint Attack1BeginToHitTime = new Fixpoint(33, 2);//攻击的判定时间
    private Fixpoint Attack2BeginToHitTime = new Fixpoint(2, 1);
    private Fixpoint Attack3BeginToHitTime = new Fixpoint(25, 2);

    private Fixpoint Attack1Damage = new Fixpoint(4, 0);//伤害倍率
    private Fixpoint Attack2Damage = new Fixpoint(4, 0);
    private Fixpoint Attack3Damage = new Fixpoint(4, 0);
    protected virtual void AttackToNext()
    {
        KnightCreatedAttack = false;
        KnightAnimaAttack = KnightAnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }
    protected virtual void RemoveAttack()
    {
        KnightAnimaAttack = 0;
        ChangeStatus(StatusType.Normal);
        return;
    }
    protected void KnightCreateAttack(Fixpoint damage, ref bool created_attack)
    {
        created_attack = true;
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * damage, 45, AnimaToward, 3, "骑士普攻命中");//最后一个参数是击飞类型
    }
    private void Attack(bool first)
    {
        Fixpoint Near = LockPos.x;
        if (first == true)
        {
            AttackToNext();
        }
        else if (KnightAnimaAttack == 1)
        {
            if (StatusTime > Attack1DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();//距离判定
                else RemoveAttack();
            }
            if (StatusTime > Attack1BeginToHitTime && KnightCreatedAttack == false)
            {
                PlayMusic("sword1");
                KnightCreateAttack(Attack1Damage, ref KnightCreatedAttack);
            }
        }
        else if (KnightAnimaAttack == 2)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && KnightCreatedAttack == false)
            {
                PlayMusic("sword2");
                KnightCreateAttack(Attack2Damage, ref KnightCreatedAttack);
            }
        }
        else if (KnightAnimaAttack == 3)
        {
            if (StatusTime > Attack3DuringTime)
            {
                RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && KnightCreatedAttack == false)
            {
                PlayMusic("sword3");
                KnightCreateAttack(Attack3Damage, ref KnightCreatedAttack);
            }
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
    private static Fixpoint DefenceTime = new Fixpoint(1, 0);//防御时间
    private static Fixpoint DefenceRate = new Fixpoint(5, 1);//承受伤害倍率
    private void Defence()
    {
        status.defence_rate = DefenceRate;
        int hited = DefenceGetHited();
        if (hited != 0)
        {
            ChangeStatus(StatusType.Hit);
            status.defence_rate = new Fixpoint(1, 0);
            return;
        }
        if (StatusTime > DefenceTime)
        {
            status.defence_rate = new Fixpoint(1, 0);
            ChangeStatus(StatusType.Normal);
        }
    }

    private static Fixpoint SkillBeginToHitTime = new Fixpoint(133, 2);//技能的结算开始时间
    private static Fixpoint SkillDruingTime = new Fixpoint(163, 2);
    private static Fixpoint SkillBetweenTime = new Fixpoint(21, 2);//伤害的间隔
    private static Fixpoint SkillAttackRate = new Fixpoint(5, 0);//攻击倍率
    private int SkillAttackTimes = 0;
    private void Skill()
    {
        //int hited = KnightGetHited();
        //if (hited != 0)
        //{
        //    SkillAttackTimes = 0;
        //    return;
        //}
        if (StatusTime < SkillBeginToHitTime)
        {
            //Fixpoint Pos = GetNear();
            Fixpoint Pos = LockPos.x;
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
        }
        if (StatusTime > SkillBeginToHitTime + SkillBetweenTime * new Fixpoint(SkillAttackTimes, 0))
        {
            if (SkillAttackTimes == 0)
            {
                PlayMusic("爆裂重击释放语音");
                Vector3 pos = new Vector3(f.pos.x.to_float(), f.pos.y.to_float() + 2.5f, 0);
                if (AnimaToward < 0) pos.x -= 3f;
                else pos.x += 3f;
                Instantiate((GameObject)AB.getobj("knightskill"), pos, Quaternion.identity);
            }
            ++SkillAttackTimes;
            Fix_vector2 tmp_pos = f.pos.Clone();
            if (AnimaToward > 0) tmp_pos.x += new Fixpoint(3, 0);
            else tmp_pos.x -= new Fixpoint(3, 0);
            tmp_pos.y += new Fixpoint(25, 1);
            CreateAttack(tmp_pos, new Fixpoint(5, 0), new Fixpoint(7, 0), status.Damage() * SkillAttackRate, 120, AnimaToward, 2, "爆裂重击命中");//最后一个参数是击飞类型
        }
        if (StatusTime > SkillDruingTime)
        {
            SkillAttackTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    protected void Death()
    {
        KnightAnimaHited = 0;
        KnightAnimaAttack = 0;
        if (f.onground)
        {
            r.velocity = new Fix_vector2(0, 0);
        }
        if (StatusTime > new Fixpoint(2, 0))//死亡到消失的时间
        {
            DeathFall();
            if (type2 == 1)
            {
                Flow_path.zombie_cnt--;
            }
            Main_ctrl.Desobj(id);
        }
    }
}
