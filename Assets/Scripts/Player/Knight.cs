using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Monster
{
    protected float KnightAnimaSpeed = 0f;
    protected int KnightAnimaAttack = 0;
    protected int KnightAnimaHited = 0;

    protected Fixpoint FindPosUp;
    protected Fixpoint FindPosDown;
    protected Fixpoint FindPosLeft;
    protected Fixpoint FindPosRight;

    protected Fixpoint CatchPosUp;
    protected Fixpoint CatchPosDown;
    protected Fixpoint CatchPosLeft;
    protected Fixpoint CatchPosRight;

    public long LockId = -1;
    protected Fix_vector2 LockPos;
    protected int HomeLocation = -1;
    protected Fix_vector2 HomePos;

    private GameObject red = null;
    private GameObject blue = null;

    public bool Check;

    void Start()
    {
        CharacterType = 1 + type2;
        SetStatus(400, 10);//血量，基础攻击力
        animator = GetComponent<Animator>();
        HitTime = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(2, 1) };
        ToughnessStatus = new int[2] { 20, 0 };//阶段

        FindPosUp = new Fixpoint(10, 0);
        FindPosDown = new Fixpoint(-3, 0);
        FindPosLeft = new Fixpoint(-20, 0);
        FindPosRight = new Fixpoint(20, 0);

        CatchPosUp = f.pos.y.Clone() + new Fixpoint(20, 0);
        CatchPosDown = f.pos.y.Clone() - new Fixpoint(20, 0);
        CatchPosLeft = f.pos.x.Clone() - new Fixpoint(40, 0);
        CatchPosRight = f.pos.x.Clone() + new Fixpoint(40, 0);

        HomePos = f.pos.Clone();
        HomeLocation = Main_ctrl.CalPos(f.pos.x.Clone(), f.pos.y.Clone());
        if (HomeLocation == -1)
        {
            Debug.LogError("Find Home Error");
        }

        Debug.Log(id + "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");

    }

    // Update is called once per frame
    private GameObject Follow = null;
    void Update()
    {
        animator.SetFloat("speed", KnightAnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("attack", KnightAnimaAttack);
        animator.SetInteger("hited", KnightAnimaHited);
        animator.SetInteger("status", AnimaStatus);
        if (Follow != null)
        {
            Follow.transform.position = new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), 0);
        }
        if (Follow == null && LockId != -1 && Check == true)
        {
            Follow = Instantiate((GameObject)Resources.Load("Prefabs/yellow"), new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), 0f), Quaternion.identity);
            Follow.transform.position = new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), -1);
        }
        if (Follow != null && LockId == -1 || Check == false)
        {
            Destroy(Follow);
            Follow = null;
        }
        if(Check == true && red == null && blue == null)
        {
            red = Instantiate((GameObject)Resources.Load("Prefabs/red"), transform);
            blue = Instantiate((GameObject)Resources.Load("Prefabs/blue"), new Vector3(HomePos.x.to_float(), HomePos.y.to_float(), 0f), Quaternion.identity);
            red.transform.localScale = new Vector3((FindPosRight.to_float() - FindPosLeft.to_float())/3, (FindPosUp.to_float() - FindPosDown.to_float())/3, 1);
            red.transform.Translate((FindPosRight.to_float() + FindPosLeft.to_float())/2,(FindPosUp.to_float() + FindPosDown.to_float())/2,0);
            blue.transform.localScale = new Vector3(CatchPosRight.to_float() - CatchPosLeft.to_float(), CatchPosUp.to_float() - CatchPosDown.to_float(), 1);
            blue.transform.Translate((CatchPosRight.to_float() + CatchPosLeft.to_float()) / 2 - HomePos.x.to_float(), (CatchPosUp.to_float() + CatchPosDown.to_float()) / 2 - HomePos.y.to_float(), 0);
        } else if (Check == false && red != null && blue != null)
        {
            Destroy(red);
            Destroy(blue);
            red = null;
            blue = null;
        }
    }

    protected bool InFindSpace(Fix_vector2 pos)
    {
        Fixpoint dx = pos.x - f.pos.x; 
        Fixpoint dy = pos.y - f.pos.y;
        if (InCatahSpace(pos) == true && dx > FindPosLeft && dx < FindPosRight && dy > FindPosDown && dy < FindPosUp)
        {
            return true;
        }
        else return false;
    } 

    protected bool InCatahSpace(Fix_vector2 pos)
    {
        if (pos.x > CatchPosLeft && pos.x < CatchPosRight && pos.y > CatchPosDown && pos.y < CatchPosUp)
        {
            return true;
        }
        else return false;
    }
    protected void FindLock()
    {
        for (int i = 0;i < Player_ctrl.plays.Count; ++i)
        {
            if (InFindSpace(Player_ctrl.plays[i].f.pos) == true)
            {
                LockId = i;
                LockPos = Player_ctrl.plays[i].f.pos;
                break;
            }
        }
    }

    protected void CheckCatchQuit()
    {
        if (LockId == -1) return;
        if(InCatahSpace(LockPos) == false)
        {
            LockId = -1;
            LockPos = new Fix_vector2(0, 0);
            return;
        }
    }

    public override void Updatex()
    {

        if(LockId != -1)
        {
            LockPos = Player_ctrl.plays[(int)LockId].f.pos;
        }
        else
        {
            FindLock();
        }
        CheckCatchQuit();
        
        if (status.GetToughness() != 100)
        {
            Debug.Log(status.GetToughness());
        }
        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if(status.death == true && AnimaStatus != 8)
        {
            ChangeStatus(StatusType.Death);
        }
        if(RealStatus != StatusType.Hit && RealStatus != StatusType.Ground && RealStatus != StatusType.Death && RealStatus != StatusType.Defence)
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
                    Main_ctrl.NewAttack2("LightBall",f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), attack2.HpDamage, attack2.ToughnessDamage, id, AnimaToward, CharacterType,attack.hited_fly_type);
                    Main_ctrl.Desobj(attack2.id);
                } else
                {
                    status.GetAttacked(attack.HpDamage/DefenceRate, attack.ToughnessDamage);
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
                status.GetAttacked(HpDamage, ToughnessDamage/2);
            }
            else
            {
                status.GetAttacked(HpDamage/DefenceRate, ToughnessDamage);
            }

            Preform(status.last_damage);
        }
        if (this_hited)
        {
            return CheckToughnessStatus();
        } else
        {
            return 0;
        }
    }

    protected Fixpoint FindDistance = new Fixpoint(10, 0);
    protected int KnightGetNear(ref Fixpoint nearx)
    {
        Fixpoint Min = new Fixpoint(10000000, 0);
        Fixpoint Minx = new Fixpoint(100, 0);
        Fixpoint Miny = new Fixpoint(100, 0);
        foreach (Player i in Player_ctrl.plays)
        {
            Fixpoint Dis = new Fixpoint(0,0);
            if(i.f.pos.x < f.pos.x)
            {
                Dis += f.pos.x - i.f.pos.x;
            } else
            {
                Dis += i.f.pos.x - f.pos.x;
            }
            if (i.f.pos.y < f.pos.y)
            {
                Dis += f.pos.y - i.f.pos.y;
            }
            else
            {
                Dis += i.f.pos.y - f.pos.y;
            }
            if(Dis < Min)
            {
                Min = Dis;
                Minx = i.f.pos.x;
                Miny = i.f.pos.y;
                nearx = i.f.pos.x;
            }
        }
        if (Min > FindDistance) return -1;//寻路距离
        else return Main_ctrl.CalPos(Minx, Miny);
    }

    private void Normal()
    {
        KnightAnimaSpeed = 5f;
        
        if(!f.onground)
        {
            ChangeStatus(StatusType.Fall);
            return;
        }

        int Location = Main_ctrl.CalPos(f.pos.x, f.pos.y);
        if(Location == -1)
        {
            Moves(AnimaToward, status.WalkSpeed);
        }
        
        //Fixpoint Nearx = new Fixpoint(0, 0);
        //int Pos = KnightGetNear(ref Nearx);

        if (LockId == -1) // 如果没有锁定玩家
        {
            if (Main_ctrl.CalPos(f.pos.x, f.pos.y) != HomeLocation)//如果不在家的区域
            {
                SearchX(HomeLocation);
                return;
            }
            else //如果在家的区域，巡逻
            {
                Main_ctrl.node area = Main_ctrl.GetMapNode(f.pos.x, f.pos.y);
                Fixpoint Left =  new Fixpoint(area.left, 0) + new Fixpoint(15, 1);
                Fixpoint Right = new Fixpoint(area.right, 0) - new Fixpoint(15, 1);
                if (Left < CatchPosLeft) Left = CatchPosLeft;
                if (Right > CatchPosRight) Right = CatchPosRight;
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
        }
        else
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
                    else
                    {
                        ChangeStatus(StatusType.Skill);//随机技能
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
    protected void KnightCreateAttack(Fixpoint damage , ref bool created_attack)
    {
        created_attack = true;
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * damage, 45, AnimaToward,3);//最后一个参数是击飞类型
    }
    private void Attack(bool first)
    {
        //Fixpoint Near = GetNearDistance();
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
            if (StatusTime > Attack1BeginToHitTime && KnightCreatedAttack == false) KnightCreateAttack(Attack1Damage,ref KnightCreatedAttack);
        }
        else if (KnightAnimaAttack == 2)
        {
            if (StatusTime > Attack2DuringTime)
            {
                if (Near <= new Fixpoint(15, 1)) AttackToNext();
                else RemoveAttack();
            }
            if (StatusTime > Attack2BeginToHitTime && KnightCreatedAttack == false) KnightCreateAttack(Attack2Damage, ref KnightCreatedAttack);
        }
        else if (KnightAnimaAttack == 3)
        {
            if (StatusTime > Attack3DuringTime)
            {
                RemoveAttack();
            }
            if (StatusTime > Attack3BeginToHitTime && KnightCreatedAttack == false) KnightCreateAttack(Attack3Damage, ref KnightCreatedAttack);
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

    protected void Jump(int type)
    {
        if (type == 1)
        {
            AnimaToward = 1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(20, 0));
        }
        else if (type == -1)
        {
            AnimaToward = -1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(20, 0));
        }
        else
        {
            if (f.onground)
            {
                ChangeStatus(StatusType.Normal);
            }
            if (StatusTime > new Fixpoint(1, 0))
            {
                if (AnimaToward > 0)
                {
                    f.pos.x += status.WalkSpeed * Dt.dt;
                }
                else
                {
                    f.pos.x -= status.WalkSpeed * Dt.dt;
                }
            }
        }
    }
    private static Fixpoint DefenceTime = new Fixpoint(5, 0);//防御时间
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
        if(StatusTime < SkillBeginToHitTime) 
        {
            //Fixpoint Pos = GetNear();
            Fixpoint Pos= LockPos.x;
            if (f.pos.x < Pos) AnimaToward = 1;
            else AnimaToward = -1;
        }
        if (StatusTime > SkillBeginToHitTime + SkillBetweenTime * new Fixpoint(SkillAttackTimes,0))
        {
            if(SkillAttackTimes == 0)
            {
                Vector3 pos = new Vector3(f.pos.x.to_float(), f.pos.y.to_float() + 2.5f, 0);
                if (AnimaToward < 0) pos.x -= 3f;
                else pos.x += 3f;
                Instantiate((GameObject)AB.getobj("knightskill"),pos,Quaternion.identity);
            }
            ++SkillAttackTimes;
            Fix_vector2 tmp_pos = f.pos.Clone();
            if (AnimaToward > 0) tmp_pos.x += new Fixpoint(3, 0);
            else tmp_pos.x -= new Fixpoint(3, 0);
            tmp_pos.y += new Fixpoint(25, 1);
            CreateAttack(tmp_pos, new Fixpoint(5, 0), new Fixpoint(7, 0), status.Damage() * SkillAttackRate, 120, AnimaToward,2);//最后一个参数是击飞类型
        }
        if(StatusTime > SkillDruingTime)
        {
            SkillAttackTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }
    protected void Fall()
    {
        if(f.onground)
        {
            ChangeStatus(StatusType.Normal);
        }
    }

    protected void Death()
    {
        KnightAnimaHited = 0;
        KnightAnimaAttack = 0;
        if(StatusTime > new Fixpoint(2,0))//死亡到消失的时间
        {
            if (type2 == 1)
            {
                Flow_path.zombie_cnt--;
            } 
            Main_ctrl.Desobj(id);
        }
    }

    protected int last_pos = -1;
    protected virtual void Search()
    {
        int x = Main_ctrl.CalPos(f.pos.x, f.pos.y);
        if(x == -1)
        {
            if(AnimaToward > 0)
            {
                f.pos.x += status.WalkSpeed * Dt.dt;
            } else
            {
                f.pos.x -= status.WalkSpeed * Dt.dt;
            }
            ChangeStatus(StatusType.Normal);
            return;
        }
        last_pos = x;

        Fixpoint nearx = new Fixpoint(0,0);
        int y = KnightGetNear(ref nearx);

        if(y ==  -1)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        if(x == y)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        Main_ctrl.TranslateMethod method = Main_ctrl.Guide(x, y);
        if(method.able == false)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        if (f.pos.x < method.pos - new Fixpoint(1,1))
        {
            AnimaToward = 1;
            f.pos.x += status.WalkSpeed * Dt.dt;
        } else if (f.pos.x > method.pos + new Fixpoint(1,1))
        {
            AnimaToward = -1;
            f.pos.x -= status.WalkSpeed * Dt.dt;
        } else
        {
            switch(method.action)
            {
                case Main_ctrl.node.TravelType.LittleJumpLeft:
                    ChangeStatus(StatusType.LittleJump);
                    LittleJump(-1);
                    break;
                case Main_ctrl.node.TravelType.LittleJumpRight:
                    ChangeStatus(StatusType.LittleJump);
                    LittleJump(1);
                    break;
                case Main_ctrl.node.TravelType.Fall:
                    if(AnimaToward > 0)
                    {
                        f.pos.x += status.WalkSpeed * Dt.dt;
                    } else
                    {
                        f.pos.x -= status.WalkSpeed * Dt.dt;
                    }
                    break;
                case Main_ctrl.node.TravelType.JumpLeft:
                    ChangeStatus(StatusType.Jump);
                    Jump(-1);
                    break;
                case Main_ctrl.node.TravelType.JumpRight:
                    ChangeStatus(StatusType.Jump);
                    Jump(1);
                    break;
            }
        }

    }

    protected void LittleJump(int type)
    {
        if (type == 1)
        {
            AnimaToward = 1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(8, 0));
        } else if (type == -1)
        {
            AnimaToward = -1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(8, 0));
        } else
        {
            if(f.onground)
            {
                ChangeStatus(StatusType.Normal);
            }
            if(AnimaToward > 0)
            {
                f.pos.x += status.WalkSpeed * Dt.dt;
            } else
            {
                f.pos.x -= status.WalkSpeed * Dt.dt;
            }
        }
    }

    protected void SearchX(int To)
    {
        if (To == -1) return;
        int x = Main_ctrl.CalPos(f.pos.x, f.pos.y);
        if (x == -1)
        {
            if (AnimaToward > 0)
            {
                f.pos.x += status.WalkSpeed * Dt.dt;
            }
            else
            {
                f.pos.x -= status.WalkSpeed * Dt.dt;
            }
            ChangeStatus(StatusType.Normal);
            return;
        }

        if(x == To)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }

        Main_ctrl.TranslateMethod method = Main_ctrl.Guide(x, To);
        if (method.able == false)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        if (f.pos.x < method.pos - new Fixpoint(1, 1))
        {
            AnimaToward = 1;
            f.pos.x += status.WalkSpeed * Dt.dt;
        }
        else if (f.pos.x > method.pos + new Fixpoint(1, 1))
        {
            AnimaToward = -1;
            f.pos.x -= status.WalkSpeed * Dt.dt;
        }
        else
        {
            switch (method.action)
            {
                case Main_ctrl.node.TravelType.LittleJumpLeft:
                    ChangeStatus(StatusType.LittleJump);
                    LittleJump(-1);
                    break;
                case Main_ctrl.node.TravelType.LittleJumpRight:
                    ChangeStatus(StatusType.LittleJump);
                    LittleJump(1);
                    break;
                case Main_ctrl.node.TravelType.Fall:
                    if (AnimaToward > 0)
                    {
                        f.pos.x += status.WalkSpeed * Dt.dt;
                    }
                    else
                    {
                        f.pos.x -= status.WalkSpeed * Dt.dt;
                    }
                    break;
                case Main_ctrl.node.TravelType.JumpLeft:
                    ChangeStatus(StatusType.Jump);
                    Jump(-1);
                    break;
                case Main_ctrl.node.TravelType.JumpRight:
                    ChangeStatus(StatusType.Jump);
                    Jump(1);
                    break;
            }
        }
    }
}
