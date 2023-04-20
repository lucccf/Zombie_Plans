using Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_ctrl : MonoBehaviour
{
    public static List<long> players = new List<long>();

    // Start is called before the first frame update
    public static Dictionary<long, Object_ctrl> All_objs = new Dictionary<long, Object_ctrl>();
    public static Dictionary<long, long> Ser_to_cli = new Dictionary<long, long>();

    public static ConcurrentQueue<Frame> Frames = new ConcurrentQueue<Frame>();
    private static Queue<long> Des_objs = new Queue<long>();
    private static Queue<Obj_info> Cre_objs = new Queue<Obj_info>();
    
    private static Dictionary<int,Item>ItemList = new Dictionary<int,Item>();

    public static List<List<int>> holes = new List<List<int>>();
    public static List<List<int>> walls = new List<List<int>>();
    public static int wall_len;
    public static int hole_len;
    private static List<List<node>> MapNode;

    static long cnt = 0;


    public static long user_id = -1;
    public static long main_id = -1;
    public static int wolf_cnt = 1;

    public static GameObject camara;
    public static GameObject Tinymap;
    public static GameObject play;

    public static long frame_index = 0;

    public static bool Exit = false;

    public enum objtype
    {
        Character,
        Article,
        Attack,
        Wall,
        //...
    }

    private void init()
    {
        All_objs = new Dictionary<long, Object_ctrl>();
        Ser_to_cli = new Dictionary<long, long>();
        ItemList = new Dictionary<int, Item>();
        holes = new List<List<int>>();
        walls = new List<List<int>>();
        Exit = false;
        Rigid_ctrl.rigs = new List<Fix_rig2d>();
        Collider_ctrl.cols = new List<Fix_col2d>();
        Player_ctrl.plays = new List<Player>();
        Map_ctrl.Map_items = new Dictionary<long, GameObject>();
        Flow_path.init();
    }

    void Start()
    {
        init();
        Rand.Setseed(114514);
        camara = GameObject.Find("Main Camera");
        Tinymap = GameObject.Find("Tiny_map");
        Map_create.Wall_create();
        Map_create.Item_create();
        Map_create.Facility_create();
        Map_create.Protal_create();
        Map_create.Background_create();
        Monster_create.Mon_create1();
        Player_ctrl.Init_bag();
        CalRoad();
        Item[] Items = Resources.LoadAll<Item>("Prefabs/items/");
        for (int i = 0; i < Items.Length; ++i)
        {
            ItemList.Add(Items[i].id, Items[i]);
        }

        Play_create();
        players.Clear();
        Wolf_create();
        main_id = Ser_to_cli[user_id];
    }

    static void Wolf_create()
    {
        for(int i = 0; i < wolf_cnt; i++)
        {
            int k = (int)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
            while (Player_ctrl.plays[k].identity == Player.Identity.Wolf)
            {
                k = (int)(Rand.rand() % (ulong)Player_ctrl.plays.Count);
            }
            Player_ctrl.plays[k].identity = Player.Identity.Wolf;
        }
    }

    public struct node
    {
        public int id;
        public int idx;
        public int left,right;
        public enum type
        {
            wall,
            hole
        };
        public enum TravelType
        {
            Fall,
            JumpLeft,
            JumpRight,
            LittleJumpLeft,
            LittleJumpRight
        };
        public type LeftType, RightType;
        public List<int> to;
        public List<Fixpoint> pos;
        public List<TravelType> action;
        public node (int id,int posx,int posyl,int posyr,type x,type y)
        {
            this.id = id;
            this.idx = posx;
            this.left = posyl;
            this.right = posyr;
            this.to = new List<int>();
            this.pos = new List<Fixpoint>();
            this.action = new List<TravelType>();
            this.LeftType = x;
            this.RightType = y;
        }
    }

    public struct TranslateMethod
    {
        public TranslateMethod(Fixpoint pos, node.TravelType action)
        {
            this.pos = pos;
            this.action = action;
            able = true;
        }
        public Fixpoint pos;
        public node.TravelType action;
        public bool able;
    }
    public static int PostionToY(Fixpoint num)
    {
        int Num = (new Fixpoint(5,1) + num).to_int();
        Num = -Num;
        return Num / 9 + 1;
    }
    private static TranslateMethod[,] TranslateTo;
    public static TranslateMethod Guide(Fixpoint x, Fixpoint y, Fixpoint tox, Fixpoint toy)
    {
        int from = CalPos(tox, toy);
        int to = CalPos(tox, toy);
        return TranslateTo[from,to];
    }
    public static TranslateMethod Guide(int x,int y)
    {
        return TranslateTo[x, y];
    }
    public static int CalPos(Fixpoint posx , Fixpoint posy)
    {
        int x = (posx + new Fixpoint(5, 1)).to_int();
        int y = PostionToY(posy);
        int l = 0, r = MapNode[y].Count - 1;
        while(l!=r)
        {
            int mid = (l + r) / 2;
            if (MapNode[y][mid].right < x)
            {
                l = mid + 1;
            } else
            {
                r = mid;
            }
        }
        if (MapNode[y][l].left <= x && MapNode[y][l].right >= x) return MapNode[y][l].id;
        else return -1;
    }
    public static node GetMapNode(Fixpoint posx, Fixpoint posy)
    {
        int x = (posx + new Fixpoint(5, 1)).to_int();
        int y = PostionToY(posy);
        int l = 0, r = MapNode[y].Count - 1;
        while (l != r)
        {
            int mid = (l + r) / 2;
            if (MapNode[y][mid].right < x)
            {
                l = mid + 1;
            }
            else
            {
                r = mid;
            }
        }
        if (MapNode[y][l].left <= x && MapNode[y][l].right >= x) return MapNode[y][l];
        else return new node();
    }
    private static void CalRoad()
    {
        List<List<node>> nodes;
        int NodeCount = 0;
        nodes = new List<List<node>>();
        nodes.Add(new List<node>());
        for (int i = 1; i < walls.Count; ++i)
        {
            nodes.Add(new List<node>());
            List<int> wall = walls[i];
            List<int> hole = holes[i];
            node.type last = node.type.wall;
            int lastpos = 0;
            for (int j = 0, k = 1; j < wall.Count || k < hole.Count;)
            {
                if (j >= wall.Count || (wall[j] > hole[k] && k < hole.Count))
                {
                    if (hole[k] <= lastpos)
                    {
                        last = node.type.hole;
                        lastpos = hole[k] + hole_len;
                        ++k;
                        continue;
                    }
                    ++NodeCount;
                    nodes[i].Add(new node(NodeCount,i,lastpos, hole[k],last,node.type.hole));
                    last = node.type.hole;
                    lastpos = hole[k] + hole_len;
                    ++k;
                } else
                {
                    if(wall[j] - wall_len <= lastpos)
                    {
                        last = node.type.wall;
                        lastpos = wall[j] + wall_len;
                        ++j;
                        continue;
                    }
                    ++NodeCount;
                    nodes[i].Add(new node(NodeCount ,i,lastpos, wall[j] - wall_len,last,node.type.wall));
                    last = node.type.wall;
                    lastpos = wall[j] + wall_len;
                    ++j;
                }
            }
        }

        for (int i=1;i< nodes.Count;++i)
        {
            List<node> down;
            if (i != nodes.Count - 1)
            {
                down = nodes[i + 1];
            } else
            {
                down = new List<node>();
            }
            int k = 0;
            for(int j = 0;j< nodes[i].Count;++j)
            {
                if(j!= 0 && nodes[i][j].LeftType == node.type.hole && nodes[i][j].left == nodes[i][j-1].right + hole_len)
                {
                    nodes[i][j].action.Add(node.TravelType.LittleJumpLeft);
                    nodes[i][j].pos.Add(new Fixpoint(nodes[i][j].left, 0));
                    nodes[i][j].to.Add(nodes[i][j - 1].id);
                    nodes[i][j-1].action.Add(node.TravelType.LittleJumpRight);
                    nodes[i][j-1].pos.Add(new Fixpoint(nodes[i][j-1].right, 0));
                    nodes[i][j-1].to.Add(nodes[i][j].id);

                }
                if (nodes[i][j].LeftType == node.type.hole)
                {
                    while (k < down.Count && down[k].right < nodes[i][j].left - (hole_len + 1) / 2)
                    {
                        ++k;
                    }
                    if (k < down.Count && down[k].left < nodes[i][j].left - (hole_len + 1) / 2)
                    {
                        nodes[i][j].action.Add(node.TravelType.Fall);
                        nodes[i][j].pos.Add(new Fixpoint(nodes[i][j].left - (hole_len + 1) / 2, 0));
                        nodes[i][j].to.Add(down[k].id);
                        down[k].action.Add(node.TravelType.JumpRight);
                        down[k].pos.Add(new Fixpoint(nodes[i][j].left - (hole_len + 1) / 2, 0));
                        down[k].to.Add(nodes[i][j].id);
                    }
                }
                if (nodes[i][j].RightType == node.type.hole) 
                { 
                    while (k < down.Count && down[k].right < nodes[i][j].right + (hole_len + 1) / 2)
                    {
                        ++k;
                    }
                    if (k < down.Count && down[k].left < nodes[i][j].right + (hole_len + 1) / 2)
                    {
                        nodes[i][j].action.Add(node.TravelType.Fall);
                        nodes[i][j].pos.Add(new Fixpoint(nodes[i][j].right + (hole_len + 1) / 2, 0));
                        nodes[i][j].to.Add(down[k].id);
                        down[k].action.Add(node.TravelType.JumpLeft);
                        down[k].pos.Add(new Fixpoint(nodes[i][j].right + (hole_len + 1) / 2, 0));
                        down[k].to.Add(nodes[i][j].id);
                    }
                }
            }
        }
        List<node> a = new List<node>();
        a.Add(new node());
        for (int i = 1; i < nodes.Count; ++i)
        {
            for (int j = 0; j < nodes[i].Count; ++j)
            {
                a.Add(nodes[i][j]);
            }
        }
        TranslateTo = new TranslateMethod[a.Count, a.Count];
        for (int i=1 ; i < a.Count ; ++i)
        {
            Queue<int> q = new Queue<int>();
            int[] dis = new int[a.Count];
            TranslateMethod[] beg = new TranslateMethod[a.Count];
            for(int j=0;j<dis.Length;++j)
            {
                dis[j] = 0x3f3f3f3f;
                beg[j].able = false;
            }
            dis[i] = 0;
            q.Enqueue(i);
            while(q.Count > 0)
            {
                int u = q.Peek();
                q.Dequeue();
                for(int j = 0; j < a[u].to.Count; ++j)
                {
                    if (dis[a[u].to[j]] > dis[u] + 1)
                    {
                        if (beg[u].able == true)
                        {
                            beg[a[u].to[j]] = beg[u];
                        }
                        else
                        {
                            beg[a[u].to[j]] = new TranslateMethod(a[u].pos[j], a[u].action[j]);
                        }
                        dis[a[u].to[j]] = dis[u] + 1;
                        q.Enqueue(a[u].to[j]);
                    }
                }
            }
            for(int j = 1;j < beg.Length; ++j)
            {
                TranslateTo[i,j] = beg[j];
            }
        }
        MapNode = nodes;

        Debug.Log("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");

    }

    public static Item GetItemById(int id)
    {
        return ItemList[id];
    }

    void Play_create()
    {
        for (int i = 0; i < players.Count; i++)
        {
            NewPlayer(players[i]);
        }
    }

    public static void Desobj(long id)
    {
        Des_objs.Enqueue(id);
    }

    public static void Creobj(Obj_info p)
    {
        Cre_objs.Enqueue(p);
    }

    public static void NewPlayer(long player_id)
    {

        Obj_info p = new Obj_info();
        p.name = "player";
        p.hei = new Fixpoint(216, 2);
        p.wid = new Fixpoint(111, 2);
        p.pos = new Fix_vector2(new Fixpoint(10 * 28 * 5, 1), new Fixpoint(-11 * 7 * 5, 1));
        p.col_type = Fix_col2d.col_status.Collider;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Player);
        p.classnames.Add(Object_ctrl.class_name.Tinymap);
        p.user_id = player_id;
        if (player_id == user_id)
        {
            play = CreateObj(p);
        }
        else
        {
            CreateObj(p);
        }
    }

    public static void NewMonster()
    {
        Obj_info p = new Obj_info();
        p.name = "Monster1";
        p.hei = new Fixpoint(225, 2);
        p.wid = new Fixpoint(113, 2);
        p.pos = new Fix_vector2(new Fixpoint(1 * 7 * 5, 1), new Fixpoint(-1 * 7 * 5, 1));
        p.col_type = Fix_col2d.col_status.Collider;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Moster);
        CreateObj(p);
    }

    public static void NewAttack(Fix_vector2 pos, Fix_vector2 with_pos, Fixpoint width, Fixpoint high, Fixpoint Hpdamage, int Toughnessdamage, 
        long attacker_id ,float toward,bool with,int attacker_type ,int hit_fly_type)
    {
        Obj_info p = new Obj_info();
        p.name = "yellow";
        p.hei = high.Clone();
        p.wid = width.Clone();
        p.pos = pos.Clone();
        p.HpDamage = Hpdamage.Clone();
        p.ToughnessDamage = Toughnessdamage;
        p.attacker_id = attacker_id;
        p.col_type = Fix_col2d.col_status.Attack;
        p.toward = toward;
        p.with_pos = with_pos;
        p.attacker_type = attacker_type;
        p.classnames.Add(Object_ctrl.class_name.Attack);
        p.hit_fly_type = hit_fly_type;
        if(with == true)
        {
            p.type = "1";
        }
        Creobj(p);
    }
    public static void NewAttack2(string name,Fix_vector2 pos, Fixpoint width, Fixpoint high, Fixpoint Hpdamage, int Toughnessdamage, 
        long attacker_id, float toward,int attacker_type,int hit_fly_type)
    {
        Obj_info p = new Obj_info();
        p.name = name;
        p.hei = high.Clone();
        p.wid = width.Clone();
        p.pos = pos.Clone();
        p.HpDamage = Hpdamage.Clone();
        p.hit_fly_type = hit_fly_type;
        p.ToughnessDamage = Toughnessdamage;
        p.attacker_id = attacker_id;
        p.col_type = Fix_col2d.col_status.Attack2;
        p.toward = toward;
        p.type = "wave";
        p.attacker_type = attacker_type;
        p.classnames.Add(Object_ctrl.class_name.Attack);

        Creobj(p);
    }
    public static void NewItem(Fix_vector2 pos ,string itemname,int num,float size,Fix_vector2 speed)
    {
        Obj_info p = new Obj_info();
        p.name = "ItemSample";
        p.hei = new Fixpoint(1, 0);
        p.wid = new Fixpoint(1, 0);
        p.pos = pos;
        p.col_type = Fix_col2d.col_status.Trigger;
        p.type = itemname;
        p.ToughnessDamage = num;
        p.toward = size;
        p.with_pos = speed;
        p.col_type = Fix_col2d.col_status.Trigger2;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Trigger);
        Creobj(p);
    }


    public static GameObject CreateObj(Obj_info info)
    {
        GameObject obj = Instantiate((GameObject)AB.getobj(info.name));
        //GameObject obj = Instantiate((GameObject)Resources.Load("Prefabs/" + info.name));
        cp = (uint)(cp * 233 + info.pos.x.to_int() * 10 + info.pos.y.to_int()) % 998244353;
        //Debug.Log(cnt + " : " + cp);
        Object_ctrl ctrl = obj.AddComponent<Object_ctrl>();
        SpriteRenderer spriteRenderer= obj.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(info.wid.to_float(), info.hei.to_float());
        obj.transform.position = new Vector3(info.pos.x.to_float(), info.pos.y.to_float(), 0);
        ctrl.id = cnt;
        All_objs[cnt] = ctrl;

        Fix_col2d f = new Fix_col2d(info.pos, info.hei, info.wid, cnt, info.col_type);
        ctrl.modules[Object_ctrl.class_name.Fix_col2d] = f;
        Collider_ctrl.cols.Add(f);

        foreach (Object_ctrl.class_name c in info.classnames)
        {
            switch (c)
            {
                case Object_ctrl.class_name.Player:
                    Player p = obj.GetComponent<Player>();
                    ctrl.modules[Object_ctrl.class_name.Player] = p;
                    p.id = cnt;
                    p.f = f;
                    p.r = (Fix_rig2d)ctrl.modules[Object_ctrl.class_name.Fix_rig2d];
                    Player_ctrl.plays.Add(p);
                    break;
                case Object_ctrl.class_name.Fix_rig2d:
                    Fix_rig2d r = new Fix_rig2d(cnt, new Fix_vector2(new Fixpoint(0, 0), new Fixpoint(-15, 0)));
                    ctrl.modules[Object_ctrl.class_name.Fix_rig2d] = r;
                    Rigid_ctrl.rigs.Add(r);
                    break;
                case Object_ctrl.class_name.Moster:
                    Monster m = obj.GetComponent<Monster>();
                    //Debug.Log(m);
                    ctrl.modules[Object_ctrl.class_name.Moster] = m;
                    m.f = f;
                    m.type2 = info.cre_type;
                    m.r = (Fix_rig2d)ctrl.modules[Object_ctrl.class_name.Fix_rig2d];
                    m.id = cnt;
                    break;
                case Object_ctrl.class_name.Attack:
                    Attack a = obj.GetComponent<Attack>();
                    ctrl.modules[Object_ctrl.class_name.Attack] = a;
                    a.f = f;
                    a.id = cnt;
                    a.HpDamage = info.HpDamage;
                    a.ToughnessDamage = info.ToughnessDamage;
                    a.attakcer_id = info.attacker_id;
                    a.toward = info.toward;
                    a.attacker_type = info.attacker_type;
                    a.hited_fly_type = info.hit_fly_type;
                    if (info.type == "1") {
                        a.with_attacker = true;
                        a.with_pos = info.with_pos.Clone();
                    } else if(info.type == "wave")
                    {
                        a.type = 1;
                        obj.transform.localScale = new Vector3(3, 3, 1);
                    }
                    a.transform.localScale = new Vector3(info.wid.to_float(), info.hei.to_float(), 0f);
                    if(info.name == "MagicCannon")
                    {
                        Attack2 a2 = obj.GetComponent<Attack2>();
                        a2.SetDestroyTime(new Fixpoint(1,0));
                    } else if (info.name == "skull")
                    {
                        Attack2 a2 = obj.GetComponent<Attack2>();
                        a2.SetSpeed(new Fixpoint(15, 0));
                        a2.SetAliveTime(new Fixpoint(9, 1));
                        a2.SetDestroyTime(new Fixpoint(25, 2));
                    }

                    if (info.type == "wave")
                    {
                        obj.transform.localScale = new Vector3(3, 3, 1);
                    }

                    break;
                case Object_ctrl.class_name.Trigger:
                    Trigger t = obj.AddComponent<Trigger>();
                    ctrl.modules[Object_ctrl.class_name.Trigger] = t;
                    t.triggertype = info.type;
                    t.triggername = info.name;
                    if(info.name == "ItemSample")
                    {
                        
                        Item x = (Item)AB.getobj(info.type);
                        //Debug.Log("Resouces:" + x.id);
                        t.itemnum = info.ToughnessDamage;
                        ItemOnGround gg = obj.GetComponent<ItemOnGround>();
                        obj.GetComponent<SpriteRenderer>().sprite = x.image;
                        gg.item = x;
                        obj.transform.localScale = new Vector3(info.toward, info.toward, 1f);
                        gg.r = (Fix_rig2d)ctrl.modules[Object_ctrl.class_name.Fix_rig2d];
                        gg.f = f;
                        gg.r.velocity = info.with_pos;
                        t.r = gg.r;
                        t.f = gg.f;
                        t.itemid = x.id;
                        
                    }
                    break;
                case Object_ctrl.class_name.Facility:
                    Facility fa = obj.AddComponent<Facility>();
                    ctrl.modules[Object_ctrl.class_name.Facility] = fa;
                    fa.id = cnt;
                    fa.materials = info.materials;
                    Dictionary<int, int> newCommited = new Dictionary<int, int>();
                    foreach (KeyValuePair<int, int> mat in info.materials)
                    {
                        newCommited.Add(mat.Key, 0);
                    }
                    fa.commited = newCommited;
                    Flow_path.facilities[cnt] = fa;
                    int k = (int)(Rand.rand() % Flow_path.cons);
                    while (Flow_path.conditions[k] != 0)
                    {
                        k = (int)(Rand.rand() % Flow_path.cons);
                    }
                    fa.cond = k;
                    Flow_path.conditions[k] = 1;

                    break;
                case Object_ctrl.class_name.Tinymap:
                    Tiny_map ti = new Tiny_map();
                    ctrl.modules[Object_ctrl.class_name.Tinymap] = ti;
                    Map_ctrl.Map_items[cnt] = Tiny_map_cre.Create_tiny(info);
                    break;
                case Object_ctrl.class_name.Tinybutton:
                    Tiny_button_cre.Create_tinybutton(info, cnt);
                    break;
                case Object_ctrl.class_name.Protalbutton:
                    Tiny_button_cre.Create_tinyprotalbutton(info, cnt);
                    break;
                case Object_ctrl.class_name.Protal:
                    break;
                case Object_ctrl.class_name.Home:
                    Home h = obj.AddComponent<Home>();
                    ctrl.modules[Object_ctrl.class_name.Home] = h;
                    h.hp = 100;
                    break;
            }
        }

        if (info.classnames.Contains(Object_ctrl.class_name.Player)) {
            Ser_to_cli[info.user_id] = cnt;
        }
        cnt++;
        return obj;
    }

    private static void DestoryObj(long id)
    {
        Object_ctrl obj = All_objs[id];
        foreach (Object_ctrl.class_name m in obj.modules.Keys)
        {
            switch (m)
            {
                case Object_ctrl.class_name.Player:
                    Player_ctrl.plays.Remove((Player)obj.modules[m]);
                    break;
                case Object_ctrl.class_name.Fix_rig2d:
                    Rigid_ctrl.rigs.Remove((Fix_rig2d)obj.modules[m]);
                    break;
                case Object_ctrl.class_name.Moster:
                    //如果是僵尸则在控制流程中-1
                    break;
                case Object_ctrl.class_name.Tinymap:
                    Destroy(Map_ctrl.Map_items[id]);
                    Map_ctrl.Map_items.Remove(id);
                    break;
            }
        }
        Collider_ctrl.cols.Remove((Fix_col2d)obj.modules[Object_ctrl.class_name.Fix_col2d]);

        Destroy(obj.gameObject);
        All_objs.Remove(id);
    }

    public static Int64 cnt2 = 0;
    public static int count = 0;

    // Update is called once per frame

    static uint cp = 0;

    void Debug_1(int index, int x)
    {
        long xx = 0;
        foreach (var yy in All_objs.Values)
        {
            Fix_vector2 p = ((Fix_col2d)yy.modules[Object_ctrl.class_name.Fix_col2d]).pos;
            xx += (int)(p.x.to_float() * 100 + p.y.to_float() * 100);
            xx = xx * 233 % 998244353;
            if (yy.modules.ContainsKey(Object_ctrl.class_name.Fix_rig2d))
            {
                Fix_vector2 q = ((Fix_rig2d)yy.modules[Object_ctrl.class_name.Fix_rig2d]).velocity;
                xx += (int)(q.x.to_float() * 100 + q.y.to_float() * 100);
                xx = xx * 233 % 998244353;
            }
        }
        Debug.Log(index + " : " + x + " : " + xx);
    }

    void Update()
    {
        while(Frames.Count > 0)
        {
            Frame f;
            if (!Frames.TryDequeue(out f)) break;
            ++count;

            frame_index = f.Index;
            Debug_1((int)frame_index, 1);

            for (int i = 0; i < f.Opts.Count; i++)
            {
                if (f.Opts[i].Opt == PlayerOpt.ExitRoom)
                {
                    if (f.Opts[i].Userid == user_id) SceneManager.LoadScene("Start");
                    continue;
                }
                Player p = (Player)(All_objs[Ser_to_cli[f.Opts[i].Userid]].modules[Object_ctrl.class_name.Player]);
                p.DealInputs(f.Opts[i]);
            }

            Debug_1((int)frame_index, 2);



            for (int i = 0; i < f.Msgs.Count; i++)
            {
                Player p = (Player)(All_objs[Ser_to_cli[f.Msgs[i].Userid]].modules[Object_ctrl.class_name.Player]);
                p.DealMsgs(f.Msgs[i]);
            }

            foreach (long i in All_objs.Keys)
            {
                if (All_objs[i].modules.ContainsKey(Object_ctrl.class_name.Player))
                {
                    Player p = (Player)All_objs[i].modules[Object_ctrl.class_name.Player];
                    p.Updatex();
                }
                if (All_objs[i].modules.ContainsKey(Object_ctrl.class_name.Moster))
                {
                    Monster p = (Monster)All_objs[i].modules[Object_ctrl.class_name.Moster];
                    p.Updatex();
                }
                if (All_objs[i].modules.ContainsKey(Object_ctrl.class_name.Attack))
                {
                    Attack p = (Attack)All_objs[i].modules[Object_ctrl.class_name.Attack];
                    p.Updatex();
                }
                if (All_objs[i].modules.ContainsKey(Object_ctrl.class_name.Trigger))
                {
                    Trigger t = (Trigger)All_objs[i].modules[Object_ctrl.class_name.Trigger];
                    t.Updatex();
                }
            }

            Debug_1((int)frame_index, 3);

            Rigid_ctrl.rig_update();
            Collider_ctrl.Update_collison();
            Flow_path.Updatex();
            Map_ctrl.Updatex();

            Debug_1((int)frame_index, 4);

            while (Des_objs.Count > 0)
            {
                long id_des = Des_objs.Dequeue();
                DestoryObj(id_des);
            }
            while (Cre_objs.Count > 0)
            {
                Obj_info q = Cre_objs.Dequeue();
                CreateObj(q);
            }

            Debug_1((int)frame_index, 5);

            if (Exit)
            {
                PlayerOptData y = new PlayerOptData();
                y.Opt = PlayerOpt.ExitRoom;
                y.Userid = (int)user_id;

                Clisocket.Sendmessage(BODYTYPE.PlayerOptData, y);
                Exit = false;
            }
        }
        play = All_objs[main_id].gameObject;
        if (play != null)
        {
            camara.transform.position = play.transform.position;
            camara.transform.position = new Vector3(camara.transform.position.x, camara.transform.position.y + 1.0f, -10);
            Tinymap.transform.position = play.transform.position;
            Tinymap.transform.position = new Vector3(Tinymap.transform.position.x / 3 + Tiny_map_cre.pos_x, Tinymap.transform.position.y / 3 + Tiny_map_cre.pos_y + 1, -10);
        }
    }
}

public class Obj_info
{
    public Fixpoint hei, wid;
    public Fix_vector2 pos;
    public string name;
    public string type;
    public Fix_col2d.col_status col_type;
    public List<Object_ctrl.class_name> classnames;
    public long user_id;
    public Fixpoint HpDamage;
    public int ToughnessDamage;
    public long attacker_id;
    public float toward;
    public int attacker_type;
    public Fix_vector2 with_pos;
    public Dictionary<int, int> materials;
    public int hit_fly_type;
    public int cre_type; 
    public Obj_info()
    {
        classnames = new List<Object_ctrl.class_name>();
    }
}