using UnityEngine;

public class Terrorist : Knight
{
    // Start is called before the first frame update
    private int y;

    public override void InitNormal()
    {
        status.attack = 10;//基础攻击力
        status.WalkSpeed = new Fixpoint(3, 0);//走路速度
        status.max_hp = 100;//最大血量
        status.hp = 100;//血量
        status.max_toughness = 100000000;
        status.toughness = 100000000;
        HitTime = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(2, 1) };
        ToughnessStatus = new int[2] { 20, 0 };//阶段
    }

    public override void Startx()
    {
        transform.rotation = Quaternion.identity;
        CharacterType = 2;
        SetStatus(2000, 10);//血量，基础攻击力
        status.max_toughness = 100000000;
        status.toughness = 100000000;
        animator = GetComponent<Animator>();
        HitTime = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(8, 1) };
        HitSpeed = new Fixpoint[2] { new Fixpoint(0, 0), new Fixpoint(2, 1) };
        ToughnessStatus = new int[2] { 20, 0 };//阶段
        FindDistance = new Fixpoint(1000000, 0);
        y = Main_ctrl.CalPos(Player_ctrl.HomePos.x, Player_ctrl.HomePos.y);
        AnimaToward = 1;
        RealStatus = StatusType.Search;
        audiosource = GetComponent<AudioSource>();
        PlayMusic("恐怖分子奔跑附带的音效");
        HitMisuc = "恐怖分子挨打-只因";
        status.WalkSpeed = new Fixpoint(3, 0);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("toward", AnimaToward);
    }

    public override void Updatex()
    {
        RemoveTrigger();
        BasicCharacterGetHited();
        StatusTime += Dt.dt;
        if (status.death == true)
        {
            ChangeStatus(StatusType.Death);
            Death(true);
            return;
        }
        switch (RealStatus)
        {
            case StatusType.Normal:
                Normal();
                break;
            case StatusType.Jump:
                Jump(0);
                break;
            case StatusType.Fall://下落
                Fall();
                break;
            case StatusType.Death:
                Death(false);
                break;
            case StatusType.Search:
                Search();
                break;
            case StatusType.LittleJump:
                LittleJump(0);
                break;
        }
        transform.position = new Vector3(f.pos.x.to_float(), f.pos.y.to_float(), 0);
    }
    private void Death(bool first)
    {
        if (first == true)
        {
            Player_ctrl.audiox.PlayMusic("恐怖分子死亡-你干嘛哎哟");
            Main_ctrl.NewAttack(f.pos, new Fix_vector2(0, 0), new Fixpoint(6, 0), new Fixpoint(6, 0), new Fixpoint(4000,0), 120, id, AnimaToward, false, CharacterType, 3, "");//最后一个参数是击飞类型
            Main_ctrl.Desobj(id);
            GameObject obj = Instantiate((GameObject)AB.getobj("Bomb2"));
            Instantiate(obj, transform.position, transform.rotation);
        }
    }
    private void Normal()
    {
        ChangeStatus(StatusType.Search);
        return;
    }
    protected override void Search()
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
            return;
        }
        last_pos = x;

        if (y == -1)
        {
            Debug.Log("ERROR");
            return;
        }
        if (x == y)
        {
            if (f.pos.x - Player_ctrl.HomePos.x < new Fixpoint(2, 1) && f.pos.x - Player_ctrl.HomePos.x > new Fixpoint(-2, 1))
            {
                ChangeStatus(StatusType.Death);
                Death(true);
                return;
            }
            if (f.pos.x < Player_ctrl.HomePos.x)
            {
                AnimaToward = 1;
                Moves(AnimaToward, status.WalkSpeed);
            }
            else
            {
                AnimaToward = -1;
                Moves(AnimaToward, status.WalkSpeed);
            }
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
