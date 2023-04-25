using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Devil : Knight
{
    private float DevilAnimaSpeed = 0f;

    private Fixpoint BombCD = new Fixpoint(0, 0);
    private Fixpoint MagicCannonCD = new Fixpoint(0, 0);
    private Fixpoint SuckerPunchCd = new Fixpoint(0, 0);
    private static Fixpoint BombCD_MAX = new Fixpoint(5, 0);
    private static Fixpoint MagicCannonCD_MAX = new Fixpoint(3, 0);
    private static Fixpoint SuckerPunchCD_MAX = new Fixpoint(5, 0);

    public override void InitStatic()
    {
        BombCD_MAX = new Fixpoint(5, 0);
        MagicCannonCD_MAX = new Fixpoint(3, 0);
        SuckerPunchCD_MAX = new Fixpoint(5, 0);
        DevilBombHitTime = new Fixpoint(4, 1);
        DevilBombHitBetween = new Fixpoint(5, 0);
        DevilBombQuitTime = new Fixpoint(139, 2);
        DevilBombAttack = new Fixpoint(5, 0);
        DevilAttack1HitTime = new Fixpoint(3, 1);
        DevilAttack2HitTime = new Fixpoint(6, 1);
        DevilAttack3HitTime = new Fixpoint(9, 1);
        DevilAttackQuitTime = new Fixpoint(1, 0);
        DevilAttack1Damage = new Fixpoint(2, 0);
        DevilAttack2Damage = new Fixpoint(2, 0);
        DevilAttack3Damage = new Fixpoint(2, 0);
        DevilCannonMagicShootTime = new Fixpoint(65, 2);
        DevilCannonMagicQuitTime = new Fixpoint(1166, 3);
        DevilCannonMagicAttack = new Fixpoint(7, 0);
        DevilSuckerPunchAttack = new Fixpoint(3, 0);
        DevilSuckerPunckBeginTime = new Fixpoint(17, 2);
        DevilSuckerPunckQuitTime = new Fixpoint(59, 2);
        DevilSuckerPunckSpeed = new Fixpoint(15, 0);
    }
    public override void InitNormal()
    {
        transform.rotation = Quaternion.identity;
        status.attack = 10;//基础攻击力
        status.WalkSpeed = new Fixpoint(3, 0);//走路速度
        status.max_hp = 1240;//最大血量
        status.hp = 1240;//血量
        status.max_toughness = 100;//最大韧性值
        status.toughness = 100;//韧性值
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(29, 2), new Fixpoint(8, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(11, 1), new Fixpoint(11, 1), new Fixpoint(6, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
    }
    public override void Startx()
    {
        CharacterType = 1 + type2;
        SetStatus(1240, 10);//血量，基础攻击力
        animator = GetComponent<Animator>();
        //status.max_toughness = 200;
        //status.toughness = 200;
        status.WalkSpeed = new Fixpoint(3, 0);
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(29, 2), new Fixpoint(8, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(11, 1), new Fixpoint(11, 1), new Fixpoint(6, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
        audiosource = GetComponent<AudioSource>();
        HitMisuc = "魔王挨打";
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("toward", AnimaToward);
        animator.SetInteger("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
        animator.SetFloat("speed", DevilAnimaSpeed);
    }

    public override void Updatex()
    {
        StatusTime += Dt.dt;
        BombCD -= Dt.dt;
        MagicCannonCD -= Dt.dt;
        SuckerPunchCd -= Dt.dt;
        if (BombCD < new Fixpoint(0, 0)) BombCD = new Fixpoint(0, 0);
        if (MagicCannonCD < new Fixpoint(0, 0)) MagicCannonCD = new Fixpoint(0, 0);
        if (SuckerPunchCd < new Fixpoint(0, 0)) SuckerPunchCd = new Fixpoint(0, 0);
        status.RecoverToughness(Dt.dt * new Fixpoint(10, 0));//自然恢复韧性值
        if (status.death == true && AnimaStatus != 10)
        {
            ChangeStatus(StatusType.Death);
        }
        if (RealStatus != StatusType.Death && RealStatus != StatusType.Ground && RealStatus != StatusType.CallMagic && RealStatus != StatusType.Hit)
        {
            int hited = BasicCharacterGetHited();
            if (hited != 0)
            {
                DevilAttackTimes = 0;
                DevilCannonMagicShooted = false;
                DevilBonmTimes = 0;
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
                DevilAttack();
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
                Hited();
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
                DevilDeath();
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
            if (Dis < new Fixpoint(2, 0) && BombCD == new Fixpoint(0, 0))
            {
                BombCD = BombCD_MAX.Clone();
                ChangeStatus(StatusType.Bomb);
                return;
            }
            else if (Dis <= new Fixpoint(3, 0)) //攻击
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                ChangeStatus(StatusType.Attack);
                return;
            }
            else if (Dis > new Fixpoint(3, 0) && Dis < new Fixpoint(9, 0))//距离判定
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                if (SuckerPunchCd == new Fixpoint(0, 0))
                {
                    SuckerPunchCd = SuckerPunchCD_MAX.Clone();
                    ChangeStatus(StatusType.SuckerPunch);
                }
                else
                {
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            }
            else if (Dis > new Fixpoint(9, 0) && Dis < new Fixpoint(15, 0))
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                if (MagicCannonCD == new Fixpoint(0, 0))
                {
                    MagicCannonCD = MagicCannonCD_MAX.Clone();
                    ChangeStatus(StatusType.CannonMagic);
                }
                else
                {
                    Moves(AnimaToward, status.WalkSpeed);
                }
                return;
            }
            else
            {
                if (f.pos.x < Nearx) AnimaToward = 1;
                else AnimaToward = -1;
                Moves(AnimaToward, status.WalkSpeed);
            }
            return;
        }
        else //否则寻路
        {
            ChangeStatus(StatusType.Search);
        }
    }


    private static Fixpoint DevilAttack1HitTime = new Fixpoint(3, 1);
    private static Fixpoint DevilAttack2HitTime = new Fixpoint(6, 1);
    private static Fixpoint DevilAttack3HitTime = new Fixpoint(9, 1);
    private static Fixpoint DevilAttackQuitTime = new Fixpoint(1, 0);
    private static Fixpoint DevilAttack1Damage = new Fixpoint(4, 0);
    private static Fixpoint DevilAttack2Damage = new Fixpoint(4, 0);
    private static Fixpoint DevilAttack3Damage = new Fixpoint(4, 0);
    private int DevilAttackTimes = 0;

    private void DevilCreateAttack(Fixpoint HPDamage, int ToughnessDamage)
    {
        Fix_vector2 AttackPos = f.pos.Clone();
        if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
        else AttackPos.x -= new Fixpoint(1, 0);
        CreateAttack(AttackPos, new Fixpoint(2, 0), new Fixpoint(2, 0), HPDamage, ToughnessDamage, AnimaToward, 3, "");
    }
    private void DevilAttack()
    {
        if (StatusTime == Dt.dt)
        {
            PlayMusic("普通攻击D");
        }
        if (StatusTime <= DevilAttack1HitTime)
        {
            return;
        }
        else if (StatusTime <= DevilAttack2HitTime && DevilAttackTimes == 0)
        {
            ++DevilAttackTimes;
            Main_ctrl.NewAttack2("skull", new Fix_vector2(f.pos.x + new Fixpoint(5, 1), f.pos.y), new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() *
                DevilAttack1Damage, 40, id, AnimaToward, CharacterType, 3, "大自爆");//最后一个参数是击飞类型
            //DevilCreateAttack(status.Damage() * DevilAttack1Damage,40);
        }
        else if (StatusTime <= DevilAttack3HitTime && DevilAttackTimes == 1)
        {
            ++DevilAttackTimes;
            Main_ctrl.NewAttack2("skull", new Fix_vector2(f.pos.x, f.pos.y + new Fixpoint(5, 1)), new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() *
                DevilAttack2Damage, 40, id, AnimaToward, CharacterType, 3, "大自爆");//最后一个参数是击飞类型
            //DevilCreateAttack(status.Damage() * DevilAttack2Damage, 40);
        }
        else if (StatusTime <= DevilAttackQuitTime && DevilAttackTimes == 2)
        {
            ++DevilAttackTimes;
            Main_ctrl.NewAttack2("skull", new Fix_vector2(f.pos.x, f.pos.y - new Fixpoint(5, 1)), new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() *
                DevilAttack3Damage, 40, id, AnimaToward, CharacterType, 3, "大自爆");//最后一个参数是击飞类型
            //DevilCreateAttack(status.Damage() * DevilAttack3Damage, 40);
        }
        else
        {
            DevilAttackTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    private static Fixpoint DevilCannonMagicShootTime = new Fixpoint(65, 2);
    private static Fixpoint DevilCannonMagicQuitTime = new Fixpoint(1166, 3);
    private static Fixpoint DevilCannonMagicAttack = new Fixpoint(7, 0);
    private bool DevilCannonMagicShooted = false;
    private void CannonMagic()
    {
        if (StatusTime == Dt.dt)
        {
            PlayMusic("魔法袍");
        }
        if (StatusTime > DevilCannonMagicShootTime && DevilCannonMagicShooted == false)
        {
            DevilCannonMagicShooted = true;
            Main_ctrl.NewAttack2("MagicCannon", new Fix_vector2(f.pos.x, f.pos.y + new Fixpoint(5, 1)), new Fixpoint(2, 0), new Fixpoint(2, 0),
                status.Damage() * DevilCannonMagicAttack, 120, id, AnimaToward, CharacterType, 2, "爆炸的声音");//最后一个参数是击飞类型
        }
        if (StatusTime > DevilCannonMagicQuitTime)
        {
            DevilCannonMagicShooted = false;
            ChangeStatus(StatusType.Normal);
            return;
        }
    }

    private static Fixpoint DevilBombHitTime = new Fixpoint(4, 1);
    private static Fixpoint DevilBombHitBetween = new Fixpoint(5, 0);
    private static Fixpoint DevilBombQuitTime = new Fixpoint(139, 2);
    private static Fixpoint DevilBombAttack = new Fixpoint(5, 0);
    private int DevilBonmTimes = 0;
    private void Bomb()
    {
        if (StatusTime == Dt.dt)
        {
            PlayMusic("大自爆2D");
        }
        if (StatusTime > DevilBombHitTime + new Fixpoint(DevilBonmTimes, 0) * DevilBombHitBetween)
        {
            if (DevilBonmTimes == 0)
            {
                GameObject x = (GameObject)AB.getobj("bomb");
                GameObject y = Instantiate(x, new Vector3(f.pos.x.to_float(), f.pos.y.to_float() + 1.8f, 0), Quaternion.identity);
                y.GetComponent<Bomb>().toward = AnimaToward;
            }
            ++DevilBonmTimes;
            Fix_vector2 pos = f.pos.Clone();
            pos.y += new Fixpoint(18, 1);
            CreateAttack(pos, new Fixpoint(45, 1), new Fixpoint(7, 0), status.Damage() * DevilBombAttack, 60, AnimaToward, 3, "大自爆");//最后一个参数是击飞类型
        }
        if (StatusTime > DevilBombQuitTime)
        {
            DevilBonmTimes = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    private void CallMagic()
    {

    }

    private static Fixpoint DevilSuckerPunchAttack = new Fixpoint(5, 0);
    private static Fixpoint DevilSuckerPunckBeginTime = new Fixpoint(17, 2);
    private static Fixpoint DevilSuckerPunckQuitTime = new Fixpoint(59, 2);
    private static Fixpoint DevilSuckerPunckSpeed = new Fixpoint(15, 0);
    private bool DevilSuckerPunckCreatedAttack = false;
    private void SuckerPunch()
    {
        if (StatusTime == Dt.dt)
        {
            PlayMusic("大冲拳D");
        }
        if (StatusTime > DevilSuckerPunckBeginTime)
        {
            Moves(AnimaToward, DevilSuckerPunckSpeed);
            if (DevilSuckerPunckCreatedAttack == false)
            {
                DevilSuckerPunckCreatedAttack = true;
                CreateAttackWithCharacter(f.pos, new Fix_vector2(0, 0), new Fixpoint(3, 0), new Fixpoint(2, 0), status.Damage() * DevilSuckerPunchAttack, 105, AnimaToward, 2, "大冲拳 (1)");//最后一个参数是击飞类型
            }
        }
        if (StatusTime > DevilSuckerPunckQuitTime)
        {
            DevilSuckerPunckCreatedAttack = false;
            ChangeStatus(StatusType.Normal);
        }
    }

    private void DevilDeath()
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
