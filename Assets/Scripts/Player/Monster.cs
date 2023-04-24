using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BasicCharacter
{
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

    protected GameObject red = null;
    protected GameObject blue = null;
    protected GameObject Follow = null;

    public bool Check;
    public bool ToHomeFlag = false;
    protected bool AngryFlag = false;
    protected bool HasToHome = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void NormalUpdate()
    {
        if (LockId != -1)
        {
            LockPos = Player_ctrl.plays[(int)LockId].f.pos;
        }
        else
        {
            FindLock();
        }
        CheckCatchQuit();
    }

    protected void SetFindStatus()
    {
        FindPosUp = new Fixpoint(7, 0);
        FindPosDown = new Fixpoint(-3, 0);
        FindPosLeft = new Fixpoint(-15, 0);
        FindPosRight = new Fixpoint(15, 0);

        CatchPosUp = f.pos.y.Clone() + new Fixpoint(22, 0);
        CatchPosDown = f.pos.y.Clone() - new Fixpoint(22, 0);
        CatchPosLeft = f.pos.x.Clone() - new Fixpoint(40, 0);
        CatchPosRight = f.pos.x.Clone() + new Fixpoint(40, 0);


        HomePos = f.pos.Clone();
        HomeLocation = Main_ctrl.CalPos(f.pos.x.Clone(), f.pos.y.Clone());
        if (HomeLocation == -1)
        {
            Debug.LogError("Find Home Error" + f.pos.x.to_float() + " " + f.pos.y.to_float());
        }
    }
    public virtual void ToHome()
    {
        if (HasToHome == true) return;
        HasToHome = true;
        HomePos = Player_ctrl.HomePos.Clone();
        HomeLocation = Main_ctrl.CalPos(Player_ctrl.HomePos.x, Player_ctrl.HomePos.y);

        FindPosLeft = new Fixpoint(-3, 0);
        FindPosRight = new Fixpoint(3, 0);
        FindPosUp = new Fixpoint(7, 0);
        FindPosDown = new Fixpoint(-3, 0);

        CatchPosUp = HomePos.x.Clone() + FindPosUp;
        CatchPosDown = HomePos.x.Clone() + FindPosDown;
        CatchPosLeft = HomePos.x.Clone() + FindPosLeft;
        CatchPosRight = HomePos.x.Clone() + FindPosRight;
    }

    public void BeAngry()
    {
        if (AngryFlag == true) return;
        AngryFlag = true;
        //CatchPosUp = HomePos.x.Clone();
        //CatchPosDown = HomePos.x.Clone();
        //CatchPosLeft = HomePos.x.Clone();
        //CatchPosRight = HomePos.x.Clone();
        FindPosLeft = new Fixpoint(0, 0);
        FindPosRight = new Fixpoint(0, 0);
        FindPosUp = new Fixpoint(0, 0);
        FindPosDown = new Fixpoint(0, 0);

        status.max_toughness = 100000000;
        status.toughness = 100000000;
        status.WalkSpeed = new Fixpoint(8,0);
    }

    protected void SetBlueAndRed()
    {
        if (Follow != null)
        {
            Follow.transform.position = new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), 0);
        }
        if (Follow == null && LockId != -1 && Check == true)
        {
            Follow = Instantiate((GameObject)AB.getobj("yellow"), new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), 0f), Quaternion.identity);
            Follow.transform.position = new Vector3(LockPos.x.to_float(), LockPos.y.to_float(), -1);
        }
        if (Follow != null && LockId == -1 || Check == false)
        {
            Destroy(Follow);
            Follow = null;
        }
        if (Check == true && red == null && blue == null)
        {
            red = Instantiate((GameObject)AB.getobj("red"), transform);
            blue = Instantiate((GameObject)AB.getobj("blue"), new Vector3(HomePos.x.to_float(), HomePos.y.to_float(), 0f), Quaternion.identity);
            red.transform.localScale = new Vector3((FindPosRight.to_float() - FindPosLeft.to_float()) / 3, (FindPosUp.to_float() - FindPosDown.to_float()) / 3, 1);
            red.transform.Translate((FindPosRight.to_float() + FindPosLeft.to_float()) / 2, (FindPosUp.to_float() + FindPosDown.to_float()) / 2, 0);
            blue.transform.localScale = new Vector3(CatchPosRight.to_float() - CatchPosLeft.to_float(), CatchPosUp.to_float() - CatchPosDown.to_float(), 1);
            blue.transform.Translate((CatchPosRight.to_float() + CatchPosLeft.to_float()) / 2 - HomePos.x.to_float(), (CatchPosUp.to_float() + CatchPosDown.to_float()) / 2 - HomePos.y.to_float(), 0);
        }
        else if (Check == false && red != null && blue != null)
        {
            Destroy(red);
            Destroy(blue);
            red = null;
            blue = null;
        }
    }


    protected int NormalFind(ref int Location)
    {
        Location = Main_ctrl.CalPos(f.pos.x, f.pos.y);
        if (Location == -1)
        {
            Moves(AnimaToward, status.WalkSpeed);
            return 0;
        }

        if (LockId == -1) // 如果没有锁定玩家
        {
            if (Main_ctrl.CalPos(f.pos.x, f.pos.y) != HomeLocation)//如果不在家的区域
            {
                SearchX(HomeLocation);
                return 0;
            }
            else //如果在家的区域，巡逻
            {
                if(HasToHome == true)
                {
                    if(f.pos.x + new Fixpoint(1,0) < HomePos.x)
                    {
                        AnimaToward = 1;
                        Moves(AnimaToward, status.WalkSpeed);
                        return 0;
                    } else if(f.pos.x - new Fixpoint(1,0) > HomePos.x)
                    {
                        AnimaToward = -1;
                        Moves(AnimaToward, status.WalkSpeed);
                        return 0;
                    } else
                    {
                        LockPos = HomePos;
                        return 2;
                    }
                }
                Main_ctrl.node area = Main_ctrl.GetMapNode(f.pos.x, f.pos.y);
                Fixpoint Left = new Fixpoint(area.left, 0) + new Fixpoint(15, 1);
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
                return 0;
            }
        }
        return 1;
    }

    protected void Fall()
    {
        if (f.onground)
        {
            ChangeStatus(StatusType.Normal);
        }
    }

    protected bool InFindSpace(Fix_vector2 pos)
    {
        Fixpoint dx = pos.x - f.pos.x;
        Fixpoint dy = pos.y - f.pos.y;
        bool InCatch = true;
        if (HasToHome == false)
        {
            InCatch = InCatahSpace(pos);
        }
        if (InCatch && dx > FindPosLeft && dx < FindPosRight && dy > FindPosDown && dy < FindPosUp)
        {
            return true;
        }
        else return false;
    }

    protected bool InCatahSpace(Fix_vector2 pos)
    {
        if (HasToHome)
        {
            return InFindSpace(pos);
        }
        if (pos.x > CatchPosLeft && pos.x < CatchPosRight && pos.y > CatchPosDown && pos.y < CatchPosUp)
        {
            return true;
        }
        else return false;
    }
    protected void FindLock()
    {
        for (int i = 0; i < Player_ctrl.plays.Count; ++i)
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
        if (InCatahSpace(LockPos) == false)
        {
            LockId = -1;
            LockPos = new Fix_vector2(0, 0);
            return;
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

    protected void LittleJump(int type)
    {
        if (type == 1)
        {
            AnimaToward = 1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(8, 0));
        }
        else if (type == -1)
        {
            AnimaToward = -1;
            r.velocity = new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(8, 0));
        }
        else
        {
            if (f.onground)
            {
                ChangeStatus(StatusType.Normal);
            }
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

        if (x == To)
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

    protected Fixpoint GetNear()
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
        else return new Fixpoint(10000, 0);
    }
    protected Fixpoint GetNearDistance()
    {
        Fixpoint x = GetNear();
        if (f.pos.x > x)
        {
            return f.pos.x - x;
        }
        else
        {
            return x - f.pos.x;
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
            Fixpoint Dis = new Fixpoint(0, 0);
            if (i.f.pos.x < f.pos.x)
            {
                Dis += f.pos.x - i.f.pos.x;
            }
            else
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
            if (Dis < Min)
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


    protected int last_pos = -1;
    protected virtual void Search()
    {
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
        last_pos = x;

        Fixpoint nearx = new Fixpoint(0, 0);
        int y = KnightGetNear(ref nearx);

        if (y == -1)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        if (x == y)
        {
            ChangeStatus(StatusType.Normal);
            return;
        }
        Main_ctrl.TranslateMethod method = Main_ctrl.Guide(x, y);
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
