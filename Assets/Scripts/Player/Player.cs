﻿using Net;
using System.Collections.Generic;
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
    public float AnimaToward = 1f;
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private float AnimaHited = 0f;
    private bool AnimaFall = false;
    private bool AnimaJump = false;
    private bool AnimaRoll = false;
    private bool AnimaDeath = false;
    private bool AnimaGround = false;
    private bool Anima623Arrack = false;

    private int AnimaFire = 0;
    private bool AnimaRecover = false;

    private Fixpoint StatusTime = new Fixpoint(0, 0);
    public long id;

    private Queue<Fix_col2d_act> AttackQueue = new Queue<Fix_col2d_act>();
    private Queue<Fix_col2d_act> TriggerQueue = new Queue<Fix_col2d_act>();
    private PlayerBag bag = new PlayerBag();

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
            case PlayerOpt.FixFacility:
                Facility fa = Flow_path.facilities[inputs.Itemid];
                bool flag = true;
                foreach(var m in fa.materials)
                {
                    if (bag.BagGetItemsNums(m.Key) < m.Value)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    GameObject.Find("PlayerPanel").transform.Find("Facility").Find("progress").gameObject.GetComponent<ProgressBar>().endprogress = 100;
                    foreach (var m in fa.materials)
                    {
                        bag.BagGetItem(m.Key, -m.Value);
                    }
                }
                break;
        }
    }
    public void Updatex()
    {
        //Debug.Log(AnimaStatus);
        GetColider();
        GetTrigger();
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(25, 0)); //25的位置是每秒恢复韧性值
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
                Attack();
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
            case 7:
                Kick();
                break;
            case 8:
                HeavyAttack();
                break;
            case 9:
                UpAttack(false);
                break;
            case 10:
                Fire1();
                break;
            case 11:
                Fire2();
                break;
            case 12:
                RecoverHp(false);
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
   private bool checkid()
    {
        if (id == Main_ctrl.Ser_to_cli[Main_ctrl.user_id]) return true;
        else return false;
    }
    void GetColider()
    {
        if (((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Count > 0)
        {
            Fix_col2d_act a = ((Fix_col2d)Main_ctrl.All_objs[id].modules[Object_ctrl.class_name.Fix_col2d]).actions.Dequeue();
            if(a.type == Fix_col2d_act.col_action.Trigger_in || a.type == Fix_col2d_act.col_action.Trigger_out)
            {
                TriggerQueue.Enqueue(a);
            } else if (a.type == Fix_col2d_act.col_action.Attack) {
                AttackQueue.Enqueue(a);
            }
        }
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

        if (Press[KeyCode.Space] && Press[KeyCode.J] && f.onground)
        {
            AnimaStatus = 9;
            StatusTime = new Fixpoint(0, 0);
            Anima623Arrack = true;
            UpAttack(true);
            return;
        }

        if (Press[KeyCode.J] && f.onground)
        {
            if (Press[KeyCode.LeftShift])
            {
                StatusTime = new Fixpoint(0, 0);
                AnimaStatus = 8;
                HeavyAttack();
                return;
            }
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 3;
            Attack();
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
        if (Press[KeyCode.Space] && Press[KeyCode.LeftShift] && bag.BagCheckItemNums(11,1))
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 12;
            AnimaRecover = true;
            RecoverHp(true);
            return;
        }

        if (Press[KeyCode.Q] && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 10;
            return;
        }
        if (Press[KeyCode.E] && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 11;
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

        if (Press[KeyCode.J] == true)
        {
            AnimaJump = false;
            AnimaFall = true;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 7;
        }

        return;
    }
    private void Roll()
    {
        RemoveHited();
        //Debug.Log(StatusTime.to_float());
        if (StatusTime > new Fixpoint(66, 2))//翻滚的总时间
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
    private void AttackToNext()
    {
        CreatedAttack = false;
        AnimaAttack = AnimaAttack + 1;
        StatusTime = new Fixpoint(0, 0);
    }

    private void CreateAttack(Fix_vector2 with_pos,Fixpoint wide, Fixpoint high ,Fixpoint HpDamage,int toughness , bool with)
    {
        CreatedAttack = true;
        if (with == false) 
        { 
<<<<<<< HEAD
            Main_ctrl.NewAttack(with_pos, new Fix_vector2(0, 0), wide, high, status.Damage() * new Fixpoint(25, 2), toughness, id, AnimaToward, with);
        }
        else
        {
            Main_ctrl.NewAttack(NormalFixVector(), with_pos, wide, high, status.Damage() * new Fixpoint(50, 2), toughness, id, AnimaToward, with);
=======
            Main_ctrl.NewAttack(with_pos, new Fix_vector2(0, 0), wide, high, HpDamage, toughness, id, AnimaToward, with);
        }
        else
        {
            Main_ctrl.NewAttack(NormalFixVector(), with_pos, wide, high, HpDamage, toughness, id, AnimaToward, with);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        }
    }
    private void RemoveAttack()
    {
        AnimaAttack = 0f;
        StatusTime = new Fixpoint(0, 0);
        AnimaStatus = 0;
        return;
    }

    private void CheckQuitAttack()
    {
        //if (Press[KeyCode.A] || Press[KeyCode.D] || Press[KeyCode.L] || Press[KeyCode.K])//尝试删除打断的操作
        //{
        //    RemoveAttack();
        //}
    }
    private bool CreatedAttack = false;
    private static Fixpoint Attack1DuringTime = new Fixpoint(32, 2);
    private static Fixpoint Attack1QuitTime = new Fixpoint(44, 2);
    private static Fixpoint Attack2DuringTime = new Fixpoint(32, 2);
    private static Fixpoint Attack2QuitTime = new Fixpoint(44, 2);
    private static Fixpoint Attack3DuringTime = new Fixpoint(32, 2);
    private static Fixpoint Attack3QuitTime = new Fixpoint(44, 2);
    private static Fixpoint Attack4DuringTime = new Fixpoint(43, 2);
    private static Fixpoint Attack4QuitTime = new Fixpoint(49, 2);
    private static Fixpoint Attack5DuringTime = new Fixpoint(43, 2);
    private static Fixpoint Attack5QuitTime = new Fixpoint(44, 2);

<<<<<<< HEAD
    private static Fixpoint Attack1BeginToHitTime = new Fixpoint(67, 3);//命中结算的开始时间
    private static Fixpoint Attack2BeginToHitTime = new Fixpoint(67, 3);
    private static Fixpoint Attack3BeginToHitTime = new Fixpoint(67, 3);
    private static Fixpoint Attack4BeginToHitTime = new Fixpoint(167, 3);
    private static Fixpoint Attack5BeginToHitTime = new Fixpoint(167, 3);
=======
    private static Fixpoint Attack1BeginToHitTime = new Fixpoint(95, 3);
    private static Fixpoint Attack2BeginToHitTime = new Fixpoint(95, 3);
    private static Fixpoint Attack3BeginToHitTime = new Fixpoint(95, 3);
    private static Fixpoint Attack4BeginToHitTime = new Fixpoint(195, 3);
    private static Fixpoint Attack5BeginToHitTime = new Fixpoint(195, 3);

    private static Fixpoint Attack1Damage = new Fixpoint(2, 0);
    private static Fixpoint Attack2Damage = new Fixpoint(2, 0);
    private static Fixpoint Attack3Damage = new Fixpoint(2, 0);
    private static Fixpoint Attack4Damage = new Fixpoint(2, 0);
    private static Fixpoint Attack5Damage = new Fixpoint(2, 0);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
    private void Attack()
    {
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            AnimaStatus = 4;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if (AnimaAttack <= 0.5f) //刚进入，进入一段攻击状态
        {
            AttackToNext();
        } else if(AnimaAttack > 0.5f && AnimaAttack <= 1.5f) //一段攻击
        {
            if(Press[KeyCode.J] && StatusTime > Attack1DuringTime)
            {
                AttackToNext();
            }
            else if(StatusTime > Attack1DuringTime && StatusTime < Attack1QuitTime)
            {
                CheckQuitAttack();
            }
            else if(StatusTime > Attack1QuitTime)
            {
                RemoveAttack();
            }
            if (StatusTime >= Attack1BeginToHitTime && CreatedAttack == false) CreateAttack(NormalFixVector(), new Fixpoint(15, 1), new Fixpoint(2, 0) ,status.Damage() * Attack1Damage, 30 , false);

        } else if(AnimaAttack > 1.5f && AnimaAttack <= 2.5f) //二段攻击
        {
            if (Press[KeyCode.J] && StatusTime > Attack2DuringTime)
            {
                AttackToNext();
            }
            else if (StatusTime > Attack2DuringTime && StatusTime < Attack2QuitTime)
            {
                CheckQuitAttack();
            }
            else if (StatusTime > Attack2QuitTime)
            {
                RemoveAttack();
            }
            if (StatusTime >= Attack2BeginToHitTime && CreatedAttack == false) CreateAttack(NormalFixVector(), new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * Attack2Damage, 30 , false);

        } else if(AnimaAttack > 2.5f && AnimaAttack <= 3.5f) //三段攻击
        {
            if (Press[KeyCode.J] && StatusTime > Attack3DuringTime)
            {
                AttackToNext();
            }
            else if (StatusTime > Attack3DuringTime && StatusTime < Attack3QuitTime)
            {
                CheckQuitAttack();
            }
            else if (StatusTime > Attack3QuitTime)
            {
                RemoveAttack();
            }
            if (StatusTime >= Attack3BeginToHitTime && CreatedAttack == false) CreateAttack(NormalFixVector(), new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * Attack3Damage, 30 , false);

        } else if(AnimaAttack > 3.5f && AnimaAttack <= 4.5f) //四段攻击
        {
            if (Press[KeyCode.J] && StatusTime > Attack4DuringTime)
            {
                AttackToNext();
            }
            else if (StatusTime > Attack4DuringTime && StatusTime < Attack4QuitTime)
            {
                CheckQuitAttack();
            }
            else if (StatusTime > Attack4QuitTime)
            {
                RemoveAttack();
            }
            if (StatusTime >= Attack4BeginToHitTime && CreatedAttack == false) CreateAttack(NormalFixVector(), new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * Attack4Damage, 30 , false);
        }
        else //五段攻击
        {
            if (StatusTime > Attack5DuringTime && StatusTime < Attack5QuitTime)
            {
                CheckQuitAttack();
            }
            else if (StatusTime > Attack5QuitTime)
            {
                RemoveAttack();
            }
            if (StatusTime >= Attack5BeginToHitTime && CreatedAttack == false) CreateAttack(NormalFixVector(), new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage() * Attack5Damage, 30 , false);
        }
        /*
        if (first == true || (Press[KeyCode.J] && StatusTime > new Fixpoint(33, 2) && AnimaAttack < 4.5f) )
        {
            AnimaAttack = AnimaAttack + 1;
            StatusTime = new Fixpoint(0, 0);
            Fix_vector2 AttackPos = f.pos.Clone();
            if (AnimaToward > 0) AttackPos.x += new Fixpoint(1, 0);
            else AttackPos.x -= new Fixpoint(1, 0);
            Main_ctrl.NewAttack(AttackPos, new Fixpoint(15, 1), new Fixpoint(2, 0), status.Damage(), 30 ,id , -AnimaToward);
            return;
        }
        */
        if(StatusTime <= new Fixpoint(2,1))
        {
            if(AnimaToward > 0)
            {
                f.pos.x = f.pos.x + Dt.dt * new Fixpoint(1 , 0);
            } else
            {
                f.pos.x = f.pos.x - Dt.dt * new Fixpoint(1, 0);
            }
        }
        /*
        if (StatusTime > new Fixpoint(8, 1))
        {
            AnimaAttack = 0f;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            return;
        }
        */
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

        if (Press[KeyCode.J] == true)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 7;
        }
    }
    private void RemoveHited()
    {
        if (AttackQueue.Count > 0)
        {
            AttackQueue.Dequeue();
        }
    }
    private GameObject Building;
    private void GetTrigger()
    {
        while(TriggerQueue.Count > 0)
        {
            Fix_col2d_act a = TriggerQueue.Peek();
            TriggerQueue.Dequeue();
            if (a.type == Fix_col2d_act.col_action.Trigger_in)
            {
                //Debug.Log("Trigger in");
                Trigger trigger = (Trigger)(Main_ctrl.All_objs[a.opsite.id].modules[Object_ctrl.class_name.Trigger]);
                if (trigger.triggertype == "building" && checkid() == true)
                {
                    Debug.Log("Trigger");
                    GameObject parent = GameObject.Find("PlayerPanel");
                    Building = Instantiate((GameObject)AB.getobj("building"), parent.transform);
                    Building.name = trigger.name;
                } else if(trigger.triggername == "ItemSample")
                {
                    if (checkid() == true)
                    {
                        bag.BagGetItem(trigger.itemid, trigger.itemnum, Player_ctrl.BagUI);
                    }
                    else
                    {
                        bag.BagGetItem(trigger.itemid, trigger.itemnum);
                    }
                    Main_ctrl.Desobj(a.opsite.id);
                }
            }
            else if (a.type == Fix_col2d_act.col_action.Trigger_out)
            {
                Destroy(Building);
                //Debug.Log("Trigger out");
            } else
            {
                Debug.Log("Trigger error");
            }
        }
    }
    private int GetHited()
    {
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
            if (attack.attakcer_id == id) continue;
            AnimaToward = attack.toward;
            this_hited = true;


            GameObject beat = (GameObject)AB.getobj("beat");
            beat.transform.localScale = new Vector3(3f, 3f, 1f);
            Instantiate(beat, transform.position, transform.rotation);
            GameObject num = (GameObject)AB.getobj("HurtNumber");
            GameObject num2 = Instantiate(num, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            num2.GetComponent<BeatNumber>().ChangeNumber(attack.HpDamage.to_int());

            Fixpoint HpDamage = attack.HpDamage;
            int ToughnessDamage = attack.ToughnessDamage;
            status.GetAttacked(HpDamage, ToughnessDamage);
            if(attack.type == 1)
            {
                Attack2 attack2 = (Attack2)attack;
                attack2.DestroySelf();
            }
        }

        if (status.GetToughness() >= 75)
        {
            return 0;
        }
        else if (status.GetToughness() < 75 && status.GetToughness() >= 50)
        {
            AnimaHited = 1;
            AnimaStatus = 4;
            if (this_hited == true)StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 50 && status.GetToughness() >= 25)
        {
            AnimaHited = 2;
            AnimaStatus = 4;
            if (this_hited == true) StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else if (status.GetToughness() < 25 && status.GetToughness() >= 0)
        {
            AnimaHited = 3;
            AnimaStatus = 4;
            if (this_hited == true) StatusTime = new Fixpoint(0, 0);
            return 1;
        }
        else
        {
            AnimaHited = 4;
            AnimaStatus = 5;
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
        if (StatusTime < new Fixpoint(2, 1))
        {
            if (AnimaToward == 1.0f)
            {
                f.pos.x = f.pos.x - Dt.dt * new Fixpoint(1, 0);
            }
            else
            {
                f.pos.x = f.pos.x + Dt.dt * new Fixpoint(1, 0);
            }
        }

    }

    private void HitedFly()
    {
        if (StatusTime > new Fixpoint(8, 1) && f.onground)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaHited = 0;
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

    private static Fixpoint KickBeginToHit = new Fixpoint(1, 1);
    private static Fixpoint KickDuring = new Fixpoint(20, 1);
    private static Fixpoint KickShiftx = new Fixpoint(1, 0);
    private static Fixpoint KickShifty = new Fixpoint(-1, 0);

    private static Fixpoint KickDamage = new Fixpoint(2, 0);
    private bool is_kicked = false;
    private void Kick()
    {
        AnimaAttack = 1f;
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            AnimaStatus = 4;
            AnimaFall = false;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if(StatusTime > KickBeginToHit && is_kicked == false)
        {
            is_kicked = true;
<<<<<<< HEAD
            CreateAttack(new Fix_vector2(KickShiftx,KickShifty), new Fixpoint(2, 0), new Fixpoint(3, 0), 120 , true);//宽，高，韧性值，攻击框是否跟随人物
=======
            CreateAttack(new Fix_vector2(KickShiftx,KickShifty), new Fixpoint(2, 0), new Fixpoint(3, 0), status.Damage() * KickDamage, 120 , true);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
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

        if (StatusTime > KickDuring || f.onground)
        {
            AnimaAttack = 0f;
            is_kicked = false;
            if(f.onground)
            {
                AnimaFall = false;
                AnimaHited = 0;
                StatusTime = new Fixpoint(0, 0);
                AnimaStatus = 0;
                return;
            } else
            {
                AnimaHited = 0;
                StatusTime = new Fixpoint(0, 0);
                AnimaStatus = 6;
                AnimaFall = true;
                return;
            }
        }
    }

    private static Fixpoint HeavyAttackBeginToHit = new Fixpoint(17, 2);
    private static Fixpoint HeavyAttackDuring = new Fixpoint(64, 2);
    private static Fixpoint HeavyAttackShiftx = new Fixpoint(1, 0);
    private static Fixpoint HeavyAttackShifty = new Fixpoint(0, 0);
    private bool HeavyAttackHasHited = false;

    private static Fixpoint HeavyAttackDamage = new Fixpoint(2,0);
    private void HeavyAttack()
    {
        AnimaAttack = 1f;
        AnimaSpeed = status.RunSpeed.to_float();
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            AnimaSpeed = 0f;
            AnimaStatus = 4;
            AnimaFall = false;
            StatusTime = new Fixpoint(0, 0);
            return;
        }

        if (AnimaToward > 0)
        {
            f.pos.x = f.pos.x + Dt.dt * status.RunSpeed;
        }
        else
        {
            f.pos.x = f.pos.x - Dt.dt * status.RunSpeed;
        }
        if(StatusTime > HeavyAttackBeginToHit && HeavyAttackHasHited == false)
        {
            HeavyAttackHasHited = true;
<<<<<<< HEAD
            CreateAttack(new Fix_vector2(HeavyAttackShiftx, HeavyAttackShifty), new Fixpoint(3, 0), new Fixpoint(2, 0), 120, true);//宽，高，韧性值，攻击框是否跟随人物
=======
            CreateAttack(new Fix_vector2(HeavyAttackShiftx, HeavyAttackShifty), new Fixpoint(3, 0), new Fixpoint(2, 0), status.Damage() * HeavyAttackDamage, 120, true);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        } 

        if(!f.onground || StatusTime > HeavyAttackDuring)
        {
            HeavyAttackHasHited = false;
            AnimaAttack = 0f;
            AnimaSpeed = 0f;
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
    }
    private static Fixpoint UpAttackBeginToHit = new Fixpoint(13, 2);
    private static Fixpoint UpAttackDuring = new Fixpoint(20, 1);
    private static Fixpoint UpattackShiftx = new Fixpoint(1, 0);
    private static Fixpoint UpattackShifty = new Fixpoint(1, 0);
    private bool UpAttackHasHited = false;

    private static Fixpoint UpattackDamage = new Fixpoint(2, 0);

    private void UpAttack(bool first)
    {
        if(first == true)
        {
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(10, 0));
        }

        int hit = GetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            AnimaSpeed = 0f;
            AnimaStatus = 4;
            AnimaFall = false;
            UpAttackHasHited = false;
            StatusTime = new Fixpoint(0, 0);
            return;
        }

        if(!f.onground)
        {
            if(AnimaToward > 0)
            {
                f.pos.x += new Fixpoint(1, 0) * Dt.dt;
            }
            else
            {
                f.pos.x -= new Fixpoint(1, 0) * Dt.dt;
            }
        }

        if (StatusTime > UpAttackBeginToHit && UpAttackHasHited == false)
        {
            UpAttackHasHited = true;
<<<<<<< HEAD
            CreateAttack(new Fix_vector2(UpattackShiftx,UpattackShifty), new Fixpoint(2, 0), new Fixpoint(3, 0), 120, true);//宽，高，韧性值，攻击框是否跟随人物
=======
            CreateAttack(new Fix_vector2(UpattackShiftx,UpattackShifty), new Fixpoint(2, 0), new Fixpoint(3, 0), status.Damage() * UpattackDamage, 120, true);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        }

        if(StatusTime > UpAttackDuring)
        {
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            Anima623Arrack = false;
            UpAttackHasHited = false;
            return;
        }
    }

    private Fix_vector2 NormalFixVector()
    {
        Fix_vector2 tmp = f.pos.Clone();
        if (AnimaToward < 0) tmp.x -= new Fixpoint(15, 1);
        else tmp.x += new Fixpoint(15, 1);
        return tmp;
    }

    private static Fixpoint Fire1DuringTime = new Fixpoint(12, 1);//人物动作的总时间
    private static Fixpoint Fire1BeginToAttackTime = new Fixpoint(2, 1);//发射激光的时间点
    private static Fixpoint FireBetweenDuring = new Fixpoint(1, 1);//激光的攻击间隔
    private int FireTime = 0;
    private bool CreatedLighting = false;

    private static Fixpoint Fire1Damage = new Fixpoint(2, 0);
    private void Fire1()
    {

        int hit = GetHited();
        if (hit != 0)
        {
            AnimaStatus = 4;
            AnimaFire = 0;
            StatusTime = new Fixpoint(0, 0);
            CreatedLighting = false;
            FireTime = 0;
            return;
        }

        AnimaFire = 1;
        if(StatusTime > Fire1BeginToAttackTime && StatusTime < Fire1DuringTime)
        {
            if(CreatedLighting == false)
            {
                //Debug.Log("xxxxxxxx");
                CreatedLighting = true;
                GameObject lighting = Instantiate((GameObject)AB.getobj("Lighting"));
                lighting.transform.position = new Vector3(f.pos.x.to_float() + 6.5f * AnimaToward, f.pos.y.to_float(), 0f);
                lighting.transform.localScale = new Vector3(3f, 3f, 1f);
                lighting.GetComponent<Lighting>().toward = AnimaToward;
            }
            if(StatusTime > Fire1BeginToAttackTime + FireBetweenDuring * new Fixpoint(FireTime,0))
            {
                ++FireTime;
                Fix_vector2 tmp = f.pos.Clone();
                if (AnimaToward > 0) tmp.x += new Fixpoint(65, 1);
                else tmp.x -= new Fixpoint(65, 1);
                CreateAttack(tmp, new Fixpoint(12, 0), new Fixpoint(2, 0), status.Damage() * Fire1Damage, 10, false);
                return;
            }
        }
        else if (StatusTime > Fire1DuringTime)
        {
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaFire = 0;
            CreatedLighting = false;
            FireTime = 0;
        }
    }
    private static Fixpoint Fire2DuringTime = new Fixpoint(85, 2);//三连波动作总时长
    private static Fixpoint Fire2BeginToAttack1Time = new Fixpoint(2, 1);//第1次发射的时间
    private static Fixpoint Fire2BeginToAttack2Time = new Fixpoint(55, 2);//第2次发射的时间
    private static Fixpoint Fire2BeginToAttack3Time = new Fixpoint(8, 1);//第3次发射的时间
    private int HasFired1 = 0;

    private static Fixpoint Fire2Attack1 = new Fixpoint(2, 0);
    private static Fixpoint Fire2Attack2 = new Fixpoint(2, 0);
    private static Fixpoint Fire2Attack3 = new Fixpoint(2, 0);
    private void Fire2()
    {
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaStatus = 4;
            AnimaFire = 0;
            StatusTime = new Fixpoint(0, 0);
            return;
        }

        AnimaFire = 2;
        if (StatusTime > Fire2BeginToAttack1Time && StatusTime < Fire2DuringTime && HasFired1 == 0)
        {
            ++HasFired1;
<<<<<<< HEAD
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * new Fixpoint(2, 1), 40, id, AnimaToward);
=======
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack1 , 40, id, AnimaToward);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        } 
        else if(StatusTime > Fire2BeginToAttack2Time && StatusTime < Fire2DuringTime && HasFired1 == 1)
        {
            ++HasFired1;
<<<<<<< HEAD
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * new Fixpoint(2, 1), 40, id, AnimaToward);
=======
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack2, 40, id, AnimaToward);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        }
        else if(StatusTime > Fire2BeginToAttack3Time && StatusTime < Fire2DuringTime && HasFired1 == 2)
        {
            ++HasFired1;
<<<<<<< HEAD
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * new Fixpoint(2, 1), 40, id, AnimaToward);
=======
            Main_ctrl.NewAttack2(f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack3, 40, id, AnimaToward);
>>>>>>> 6830f2584e4d91deb2fc07050575b1ed75e11ab5
        }
        else if (StatusTime > Fire2DuringTime)
        {
            HasFired1 = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaStatus = 0;
            AnimaFire = 0;
        }
    }

    private Fixpoint RecoverHpDuringTime = new Fixpoint(2, 0); 
    private void RecoverHp(bool first)
    {
        AnimaRecover = true;
        int hit = GetHited();
        if (hit != 0)
        {
            AnimaStatus = 4;
            AnimaRecover = false;
            StatusTime = new Fixpoint(0, 0);
            return;
        }
        if (first == true) {
            Player_ctrl.BagUI.GetItem(11, -1);
            bag.BagGetItem(11, -1);
        }
        if(StatusTime > RecoverHpDuringTime)
        {
            AnimaStatus = 0;
            StatusTime = new Fixpoint(0, 0);
            AnimaRecover = false;
            status.RecoverHp(10);
        }
    }

    private void Update()
    {
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetBool("jump", AnimaJump);
        animator.SetBool("roll", AnimaRoll);
        animator.SetBool("fall", AnimaFall);
        animator.SetFloat("hited", AnimaHited);
        animator.SetBool("onground", AnimaGround);
        animator.SetBool("623", Anima623Arrack);
        animator.SetInteger("firetype",AnimaFire);
        animator.SetBool("recover", AnimaRecover);
    }
}
