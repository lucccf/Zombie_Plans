﻿using Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BasicCharacter
{
    private float AnimaSpeed = 0f;
    private float AnimaAttack = 0f;
    private PlayerBag bag = new PlayerBag();
    private Fixpoint QCD = new Fixpoint(0, 0);
    private Fixpoint ECD = new Fixpoint(0, 0);

    public string words = string.Empty;
    public bool words_ok = false;

    public enum Identity
    {
        Populace,
        Wolf,
        //...
    }

    public Identity identity = Identity.Populace;

    private void Start()
    {
        animator = GetComponent<Animator>();
<<<<<<< HEAD
        SetStatus(10000, 10);//血量。基础攻击力       
=======
        SetStatus(1000, 10);//血量。基础攻击力       
>>>>>>> 041e4b4037ebf4ef8167cc1a76bd26bdc06aa94a
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(29, 2), new Fixpoint(8, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(5, 1), new Fixpoint(5, 1), new Fixpoint(2, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0};//阶段
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

    public void DealMsgs(PlayerMessage msg)
    {
        words = msg.Content;
        words_ok = true;
    }

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
                    if (bag.BagGetItemsNums(m.Key) < 0)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    foreach (var m in fa.materials)
                    {
                        
                        bool value = bag.BagGetItem(m.Key, -1,Player_ctrl.BagUI);
                        if (value) {
                            if (!fa.commited.ContainsKey(m.Key))
                            {
                                fa.commited[m.Key] = 1;
                            }
                            else
                            {
                                fa.commited[m.Key] += 1;
                            }
                        }
                        GameObject.Find("PlayerPanel/Facility/ItemTitle/ItemDetail/ItemImage/Text").gameObject.GetComponent<Text>().text = "还需数量：" + (fa.materials[m.Key] - (fa.commited[m.Key])).ToString();
                        GameObject.Find("PlayerPanel/Facility/progress").gameObject.GetComponent<Image>().fillAmount = ((float)fa.commited[m.Key] / (float)fa.materials[m.Key]);
                        GameObject.Find("PlayerPanel/Facility/progress/progressText").gameObject.GetComponent<Text>().text = (fa.commited[m.Key]*100 / fa.materials[m.Key]).ToString()+"%";
                    }
                }
                Debug.Log(flag);
                break;
            case PlayerOpt.MoveItem:
                Item Makeitem = Main_ctrl.GetItemById(inputs.Itemid);
                bool flag2 = true;
                for(int i=0;i<Makeitem.MakeNeeds.Length;++i)
                {
                    if (bag.BagCheckItemNums(Makeitem.MakeNeeds[i], Makeitem.NeedsNumber[i]) == false) flag2 = false;
                }
                GameObject ui = (GameObject)Resources.Load("Prefabs/UI/提示UI");
                GameObject tmp = Instantiate(ui, Player_ctrl.BagUI.transform);
                if (flag2 == true)
                {
                    bag.BagGetItem(Makeitem.id, 1, Player_ctrl.BagUI);
                    
                    for (int i = 0; i < Makeitem.MakeNeeds.Length; ++i)
                    {
                        bag.BagGetItem(Makeitem.MakeNeeds[i], -Makeitem.NeedsNumber[i], Player_ctrl.BagUI);
                    }
                    tmp.GetComponent<MakeSuccess>().Type = true;
                }
                else
                {
                    //Player_ctrl.MakeFailedUI.SetActive(true);
                }
                break;
            case PlayerOpt.MovePlayer:
                if (identity == Identity.Wolf)
                {
                    Fix_vector2 pos = ((Fix_col2d)Main_ctrl.All_objs[inputs.Itemid].modules[Object_ctrl.class_name.Fix_col2d]).pos;
                    f.pos = pos.Clone();
                }
                break;
        }
    }
    public override void Updatex()
    {
        QCD = QCD - Dt.dt;
        if (QCD < new Fixpoint(0, 0)) QCD = new Fixpoint(0, 0);
        ECD = ECD - Dt.dt;
        if (ECD < new Fixpoint(0, 0)) ECD = new Fixpoint(0, 0);
        GetTrigger();
        StatusTime += Dt.dt;
        status.RecoverToughness(Dt.dt * new Fixpoint(18, 0)); //是每秒恢复韧性值
        if (status.death == true) ChangeStatus(StatusType.Death);
        switch (RealStatus)
        {
            case StatusType.Normal:
                AnimaStatus = 0;
                Normal();
                break;
            case StatusType.Jump:
                AnimaStatus = 1;
                Jump();
                break;
            case StatusType.Roll:
                AnimaStatus = 2;
                //翻滚
                Roll();
                break;
            case StatusType.Attack:
                AnimaStatus = 3;
                Attack();
                break;
            case StatusType.Hit:
                AnimaStatus = 4;
                //受击
                Hited();
                break;
            //case StatusType.CallMagic:
            //    AnimaStatus = 5;
                //击飞
                //HitedFly();
            //    break;
            case StatusType.Fall://下落
                AnimaStatus = 6;
                Fall();
                break;
            case StatusType.Kick:
                AnimaStatus = 7;
                Kick();
                break;
            case StatusType.HeavyAttack:
                AnimaStatus = 8;
                HeavyAttack();
                break;
            case StatusType.Upattack:
                AnimaStatus = 9;
                UpAttack(false);
                break;
            case StatusType.Fire1:
                AnimaStatus = 10;
                Fire1();
                break;
            case StatusType.Fire2:
                AnimaStatus = 11;
                Fire2();
                break;
            case StatusType.Recover:
                AnimaStatus = 12;
                RecoverHp(false);
                break;
            case StatusType.Death:
                AnimaStatus = 13;
                Death();
                break;
            case StatusType.Ground:
                AnimaStatus = 14;
                Ground();
                break;
           // case StatusType.Appear:
           //     AnimaStatus = 15;
            //    Fly2();
            //    break;
            case StatusType.Stay:
                AnimaStatus = 16;
                Stay();
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }

    public bool CheckDeath()
    {
        return status.death;
    }

    private bool checkid()
    {
        if (id == Main_ctrl.Ser_to_cli[Main_ctrl.user_id]) return true;
        else return false;
    }

    public void ThrowItem(int id)
    {
        if(bag.BagCheckItemNums(id,1) == false)
        {
            return;
        }
        bag.BagGetItem(id, -1, Player_ctrl.BagUI);
        Fix_vector2 ItemPos = f.pos.Clone();
        Fix_vector2 ItemSpeed = new Fix_vector2(new Fixpoint(5, 0), new Fixpoint(7, 0));
        if (AnimaToward > 0)
        {
            ItemPos.x += new Fixpoint(1, 0);
        } else
        {
            ItemPos.x -= new Fixpoint(1, 0);
            ItemSpeed.x = new Fixpoint(0, 0) - ItemSpeed.x;
        }
        Main_ctrl.NewItem(ItemPos, Main_ctrl.GetItemById(id).itemname, 1, 1,ItemSpeed);
    }

    private void Normal()
    {
        //站立，走路，跑步
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            AnimaSpeed = 0f;
            ChangeStatus(StatusType.Hit);
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
            ChangeStatus(StatusType.Fall);
            return;

        }

        else if (Press[KeyCode.Space] && Press[KeyCode.J] && f.onground)
        {
            ChangeStatus(StatusType.Upattack);
            UpAttack(true);
            return;
        }

        else if (Press[KeyCode.J])
        {
            if (Press[KeyCode.LeftShift])
            {
                ChangeStatus(StatusType.HeavyAttack);
                return;
            }
            else
            {
                ChangeStatus(StatusType.Attack);
                Attack();
                return;
            }
        }

        else if (Press[KeyCode.L])
        {
            ChangeStatus(StatusType.Roll);
            return;
        }
        else if (Press[KeyCode.K])
        {
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(845, 2));//跳跃起始速度
            ChangeStatus(StatusType.Jump);
            return;
        }
        else if (Press[KeyCode.Space] && Press[KeyCode.LeftShift] && bag.BagCheckItemNums(11,1))
        {
            ChangeStatus(StatusType.Recover);
            RecoverHp(true);
            return;
        }

        else if (Press[KeyCode.Q] && QCD == new Fixpoint(0,0))
        {
            QCD = new Fixpoint(10, 0);//Q的CD
            ChangeStatus(StatusType.Fire1);
            return;
        }
        else if (Press[KeyCode.E] && ECD == new Fixpoint(0,0))
        {
            ECD = new Fixpoint(10, 0);//E的CD
            ChangeStatus(StatusType.Fire2);
            return;
        }
    }

    private int JumpNumber = 0;
    void Jump()
    {
        //跳跃
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            ChangeStatus(StatusType.Hit);
            return;
        }
        if (StatusTime > new Fixpoint(2, 1) && f.onground)
        {
            ChangeStatus(StatusType.Normal);
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
        if (StatusTime > new Fixpoint(2, 1) && !f.onground)//跳的持续时间
        {
            ChangeStatus(StatusType.Fall);
        }

        if (Press[KeyCode.J] == true)
        {
            ChangeStatus(StatusType.Kick);
        }

        return;
    }
    private void Roll()
    {
        RemoveHited();
        if (StatusTime > new Fixpoint(66, 2))//翻滚的总时间
        {
            ChangeStatus(StatusType.Normal);
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

    private void RemoveAttack()
    {
        AnimaAttack = 0f;
        ChangeStatus(StatusType.Normal);
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

    private static Fixpoint Attack1BeginToHitTime = new Fixpoint(67, 3);//命中结算的开始时间
    private static Fixpoint Attack2BeginToHitTime = new Fixpoint(67, 3);
    private static Fixpoint Attack3BeginToHitTime = new Fixpoint(67, 3);
    private static Fixpoint Attack4BeginToHitTime = new Fixpoint(167, 3);
    private static Fixpoint Attack5BeginToHitTime = new Fixpoint(167, 3);

    private static Fixpoint Attack1Damage = new Fixpoint(4, 0);
    private static Fixpoint Attack2Damage = new Fixpoint(4, 0);
    private static Fixpoint Attack3Damage = new Fixpoint(5, 0);
    private static Fixpoint Attack4Damage = new Fixpoint(5, 0);
    private static Fixpoint Attack5Damage = new Fixpoint(6, 0);
    private void Attack()
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            ChangeStatus(StatusType.Hit);
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
            if (StatusTime >= Attack1BeginToHitTime && CreatedAttack == false)
            {
                CreatedAttack = true;
                CreateAttack(NormalFixVector(), new Fixpoint(35, 1), new Fixpoint(2, 0), status.Damage() * Attack1Damage, 30,AnimaToward,1);//最后一个参数是击飞类型
            }

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
            if (StatusTime >= Attack2BeginToHitTime && CreatedAttack == false)
            {
                CreatedAttack = true;
                CreateAttack(NormalFixVector(), new Fixpoint(35, 1), new Fixpoint(2, 0), status.Damage() * Attack2Damage, 30,AnimaToward,1);//最后一个参数是击飞类型
            }
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
            if (StatusTime >= Attack3BeginToHitTime && CreatedAttack == false)
            {
                CreatedAttack = true;
                CreateAttack(NormalFixVector(), new Fixpoint(35, 1), new Fixpoint(2, 0), status.Damage() * Attack3Damage, 30, AnimaToward,1);//最后一个参数是击飞类型
            }
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
            if (StatusTime >= Attack4BeginToHitTime && CreatedAttack == false)
            {
                CreatedAttack = true;
                CreateAttack(NormalFixVector(), new Fixpoint(35, 1), new Fixpoint(2, 0), status.Damage() * Attack4Damage, 30, AnimaToward,2);//最后一个参数是击飞类型
            }
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
            if (StatusTime >= Attack5BeginToHitTime && CreatedAttack == false)
            {
                CreatedAttack = true;
                CreateAttack(NormalFixVector(), new Fixpoint(35, 1), new Fixpoint(2, 0), status.Damage() * Attack5Damage, 30, AnimaToward,3);//最后一个参数是击飞类型
            }
        }

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
    }

    private void Fall()
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            AnimaAttack = 0f;
            JumpNumber = 0;
            ChangeStatus(StatusType.Hit);
            return;
        }
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
            status.toughness = status.max_toughness;
            JumpNumber = 0;
            ChangeStatus(StatusType.Normal);
            return;
        }

        if (Press[KeyCode.J] == true)
        {
            ChangeStatus(StatusType.Kick);
            return;
        }

        if (Press[KeyCode.K] == true && JumpNumber == 0)
        {
            ++JumpNumber;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(16, 0));//二段跳的起始速度
            ChangeStatus(StatusType.Jump);
            return;
        }
    }

    private GameObject Building;
    private GameObject Protal;
    private void GetTrigger()
    {
        GetColider();

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
                    Flow_path.Now_fac = a.opsite.id;
                    Debug.Log("Trigger");
                    Debug.Log(a.opsite.id);
                    GameObject parent = GameObject.Find("PlayerPanel");
                    Building = Instantiate((GameObject)AB.getobj("Building"), parent.transform);
                    Building.GetComponent<BuildingButton>().buildingid = a.opsite.id;
                    Building.name = trigger.name;
                }
                else if (trigger.triggername == "ItemSample")
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
                else if (trigger.triggername == "protal" && checkid() == true) {
                    GameObject parent = GameObject.Find("PlayerPanel");
                    GameObject protalbutton = (GameObject)AB.getobj("ProtalButton");
                    Protal = Instantiate(protalbutton, parent.transform);
                }
            }
            else if (a.type == Fix_col2d_act.col_action.Trigger_out)
            {
                Debug.Log(Main_ctrl.All_objs[a.opsite.id]);
                Trigger trigger = (Trigger)(Main_ctrl.All_objs[a.opsite.id].modules[Object_ctrl.class_name.Trigger]);
                if (trigger.triggertype == "building")
                {
                    Destroy(Building);
                }
                else if (trigger.triggertype == "protal") {
                    Destroy(Protal);
                }
            } else
            {
                Debug.Log("Trigger error");
            }
        }
    }

    private static Fixpoint KickBeginToHit = new Fixpoint(1, 1);//空中飞踢
    private static Fixpoint KickDuring = new Fixpoint(5, 0);
    private static Fixpoint KickShiftx = new Fixpoint(1, 0);//飞踢攻击框
    private static Fixpoint KickShifty = new Fixpoint(-1, 0);

    private static Fixpoint KickDamage = new Fixpoint(6, 0);
    private bool is_kicked = false;
    private void Kick()
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            is_kicked = false;
            ChangeStatus(StatusType.Hit);
            return;
        }
        if(StatusTime > KickBeginToHit && is_kicked == false)
        {
            is_kicked = true;
            CreateAttackWithCharacter(f.pos.Clone(), new Fix_vector2(KickShiftx, KickShifty),
                new Fixpoint(2, 0), new Fixpoint(3, 0), status.Damage() * KickDamage, 90, AnimaToward,3);//最后一个参数是击飞类型
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
            is_kicked = false;
            if(f.onground)
            {
                ChangeStatus(StatusType.Normal);
                return;
            } else
            {
                ChangeStatus(StatusType.Fall);
                return;
            }
        }
    }

    private static Fixpoint HeavyAttackBeginToHit = new Fixpoint(17, 2);//前冲拳
    private static Fixpoint HeavyAttackDuring = new Fixpoint(64, 2);
    private static Fixpoint HeavyAttackShiftx = new Fixpoint(1, 0);
    private static Fixpoint HeavyAttackShifty = new Fixpoint(0, 0);
    private bool HeavyAttackHasHited = false;

    private static Fixpoint HeavyAttackDamage = new Fixpoint(6,0);
    private void HeavyAttack()
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            HeavyAttackHasHited = false;
            ChangeStatus(StatusType.Hit);
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
            CreateAttackWithCharacter(f.pos.Clone(), new Fix_vector2(HeavyAttackShiftx, HeavyAttackShifty),
                new Fixpoint(2, 0), new Fixpoint(3, 0), status.Damage() * HeavyAttackDamage, 105, AnimaToward,0);//最后一个参数是击飞类型
        } 

        if(!f.onground || StatusTime > HeavyAttackDuring)
        {
            HeavyAttackHasHited = false;
            ChangeStatus(StatusType.Normal);
            return;
        }
    }
    private static Fixpoint UpAttackBeginToHit = new Fixpoint(13, 2);//升龙拳
    private static Fixpoint UpAttackDuring = new Fixpoint(5, 0);
    private static Fixpoint UpattackShiftx = new Fixpoint(1, 0);
    private static Fixpoint UpattackShifty = new Fixpoint(1, 0);
    private bool UpAttackHasHited = false;

    private static Fixpoint UpattackDamage = new Fixpoint(2, 0);

    private void UpAttack(bool first)
    {
        if(first == true)
        {
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(10, 0));//升龙拳向上的速度
        }

        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            UpAttackHasHited = false;
            ChangeStatus(StatusType.Hit);
            return;
        }

        if(!f.onground)
        {
            if(AnimaToward > 0)
            {
                f.pos.x += new Fixpoint(1, 0) * Dt.dt;//升龙拳x轴速度
            }
            else
            {
                f.pos.x -= new Fixpoint(1, 0) * Dt.dt;//升龙拳x轴速度
            }
        }

        if (StatusTime > UpAttackBeginToHit && UpAttackHasHited == false)
        {
            UpAttackHasHited = true;
            CreateAttackWithCharacter(f.pos.Clone(), new Fix_vector2(UpattackShiftx, UpattackShifty),
                new Fixpoint(2, 0), new Fixpoint(3, 0), status.Damage() * UpattackDamage, 105, AnimaToward,2);//最后一个参数是击飞类型
        }

        if(f.onground && StatusTime > new Fixpoint(5,1))//最短进入下蹲状态的时间
        {
            UpAttackHasHited = false;
            ChangeStatus(StatusType.Stay);
            return;
        }

        if(StatusTime > UpAttackDuring)
        {
            UpAttackHasHited = false;
            ChangeStatus(StatusType.Normal);
            return;
        }
    }

    private Fix_vector2 NormalFixVector()
    {
        Fix_vector2 tmp = f.pos.Clone();
        if (AnimaToward < 0) tmp.x -= new Fixpoint(125, 2);
        else tmp.x += new Fixpoint(125, 2);
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

        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            ChangeStatus(StatusType.Hit);
            CreatedLighting = false;
            FireTime = 0;
            return;
        }

        if(StatusTime > Fire1BeginToAttackTime && StatusTime < Fire1DuringTime)
        {
            if(CreatedLighting == false)
            {
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
                CreateAttack(tmp, new Fixpoint(12, 0), new Fixpoint(2, 0), status.Damage() * Fire1Damage, 10, AnimaToward,2);//最后一个参数是击飞类型
                return;
            }
        }
        else if (StatusTime > Fire1DuringTime)
        {
            ChangeStatus(StatusType.Normal);
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
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            HasFired1 = 0;
            ChangeStatus(StatusType.Hit);
            return;
        }

        if (StatusTime > Fire2BeginToAttack1Time && StatusTime < Fire2DuringTime && HasFired1 == 0)
        {
            ++HasFired1;
            Main_ctrl.NewAttack2("LightBall",f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack1 , 40, id, AnimaToward, CharacterType,2);//最后一个参数是击飞类型
        } 
        else if(StatusTime > Fire2BeginToAttack2Time && StatusTime < Fire2DuringTime && HasFired1 == 1)
        {
            ++HasFired1;
            Main_ctrl.NewAttack2("LightBall",f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack2, 40, id, AnimaToward, CharacterType,2);//最后一个参数是击飞类型
        }
        else if(StatusTime > Fire2BeginToAttack3Time && StatusTime < Fire2DuringTime && HasFired1 == 2)
        {
            ++HasFired1;
            Main_ctrl.NewAttack2("LightBall",f.pos, new Fixpoint(1, 0), new Fixpoint(1, 0), status.Damage() * Fire2Attack3, 40, id, AnimaToward, CharacterType,2);//最后一个参数是击飞类型
        }
        else if (StatusTime > Fire2DuringTime)
        {
            HasFired1 = 0;
            ChangeStatus(StatusType.Normal);
        }
    }

    private Fixpoint RecoverHpDuringTime = new Fixpoint(2, 0); //喝药的时间
    private void RecoverHp(bool first)
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            ChangeStatus(StatusType.Hit);
            return;
        }
        if (first == true) {
            //Player_ctrl.BagUI.GetItem(11, -1);
            bag.BagGetItem(11, -1, Player_ctrl.BagUI);
        }
        if (StatusTime > RecoverHpDuringTime)
        {
            ChangeStatus(StatusType.Normal);
            status.RecoverHp(100);//恢复的血量
            Preform(-100);
        }
    }

    private void Death()
    {
        AnimaAttack = 0;
        AnimaHited = 0;
        AnimaSpeed = 0;
    }
    private static Fixpoint StayTime = new Fixpoint(5, 1);//下蹲的持续时间
    private void Stay()
    {
        int hit = BasicCharacterGetHited();
        if (hit != 0)
        {
            ChangeStatus(StatusType.Hit);
            return;
        }
        if (StatusTime > StayTime)
        {
            ChangeStatus(StatusType.Normal);
        }
    }
    private void Update()
    {
        if(checkid() == true)
        {
            Player_ctrl.QCD.text = (((int)(QCD.to_float() * 10)) * 1.0 / 10).ToString();
            Player_ctrl.ECD.text = (((int)(ECD.to_float() * 10)) * 1.0 / 10).ToString();
        }
        animator.SetFloat("speed", AnimaSpeed);
        animator.SetFloat("toward", AnimaToward);
        animator.SetFloat("attack", AnimaAttack);
        animator.SetInteger("hited", AnimaHited);
        animator.SetInteger("status", AnimaStatus);
    }
}
