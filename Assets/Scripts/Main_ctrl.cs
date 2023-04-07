﻿using Net;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Main_ctrl : MonoBehaviour
{
    public static List<long> players = new List<long>();

    // Start is called before the first frame update
    public static Dictionary<long, Object_ctrl> All_objs = new Dictionary<long, Object_ctrl>();
    public static Dictionary<long, long> Ser_to_cli = new Dictionary<long, long>();

    public static Queue<Frame> Frames = new Queue<Frame>();
    private static Queue<long> Des_objs = new Queue<long>();
    private static Queue<Obj_info> Cre_objs = new Queue<Obj_info>();
    
    private static Dictionary<int,Item>ItemList = new Dictionary<int,Item>();


    float t;
    float dt = 0.033f;
    static long cnt = 0;

    long frame_id = 0;

    public static long user_id = -1;

    public static GameObject camara;
    public static GameObject Tiny_map;
    public static GameObject play;

    public static long frame_index = 0;

    public enum objtype
    {
        Character,
        Article,
        Attack,
        Wall,
        //...
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        Rand.Setseed(114514);
        camara = GameObject.Find("Main Camera");
        Tiny_map = GameObject.Find("Tiny_map");
        Debug.Log("???");
        Map_create.Wall_create();
        Debug.Log("???");
        Map_create.Item_create();
        Debug.Log("???");
        Map_create.Facility_create();
        Debug.Log("???");
        Map_create.Background_create();
        Debug.Log("???");
        Monster_create.Mon_create1();
        Debug.Log("???");
        Player_ctrl.Init_bag();
        Debug.Log("???");

        Item[] Items = Resources.LoadAll<Item>("Prefabs/items/");
        for (int i = 0; i < Items.Length; ++i)
        {
            ItemList.Add(Items[i].id, Items[i]);
        }

        Play_create();
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

    public static void NewAttack(Fix_vector2 pos, Fix_vector2 with_pos, Fixpoint width, Fixpoint high, Fixpoint Hpdamage, int Toughnessdamage, long attacker_id ,float toward,bool with)
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
        p.classnames.Add(Object_ctrl.class_name.Attack);
        if(with == true)
        {
            p.type = "1";
        }
        Creobj(p);
    }
    public static void NewAttack2(Fix_vector2 pos, Fixpoint width, Fixpoint high, Fixpoint Hpdamage, int Toughnessdamage, long attacker_id, float toward)
    {
        Obj_info p = new Obj_info();
        p.name = "LightBall";
        p.hei = high.Clone();
        p.wid = width.Clone();
        p.pos = pos.Clone();
        p.HpDamage = Hpdamage.Clone();
        p.ToughnessDamage = Toughnessdamage;
        p.attacker_id = attacker_id;
        p.col_type = Fix_col2d.col_status.Attack2;
        p.toward = toward;
        p.type = "wave";
        p.classnames.Add(Object_ctrl.class_name.Attack);

        Creobj(p);
    }
    public static void NewItem(Fix_vector2 pos ,string itemname,int num,float size)
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
        p.classnames.Add(Object_ctrl.class_name.Trigger);
        Creobj(p);
    }
    public static GameObject CreateObj(Obj_info info)
    {
        GameObject obj = Instantiate((GameObject)AB.getobj(info.name));
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
                    Debug.Log(m);
                    ctrl.modules[Object_ctrl.class_name.Moster] = m;
                    m.f = f;
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
                    if (info.type == "1") {
                        a.with_attacker = true;
                        a.with_pos = info.with_pos.Clone();
                    } else if(info.type == "wave")
                    {
                        a.type = 1;
                    }
                    a.transform.localScale = new Vector3(info.wid.to_float(), info.hei.to_float(), 0f);
                    
                    if(info.name == "LightBall")
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
                        obj.GetComponent<SpriteRenderer>().sprite = x.image;
                        obj.GetComponent<ItemOnGround>().item = x;
                        obj.transform.localScale = new Vector3(info.toward, info.toward, 1f);
                        t.itemid = x.id;
                        
                    }
                    break;
                case Object_ctrl.class_name.Facility:
                    Facility fa = obj.AddComponent<Facility>();
                    fa.id = cnt;
                    fa.materials = info.materials;
                    Flow_path.facilities[cnt] = fa;
                    break;
                case Object_ctrl.class_name.Tinymap:
                    Tiny_map_cre.Create_tiny(info);
                    break;
                case Object_ctrl.class_name.Tinybutton:
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
            }
        }
        Collider_ctrl.cols.Remove((Fix_col2d)obj.modules[Object_ctrl.class_name.Fix_col2d]);

        Destroy(obj.gameObject);
        All_objs.Remove(id);
    }

    public static Int64 cnt2 = 0;
    public static int count = 0;

    // Update is called once per frame
    void Update()
    {
        while(Frames.Count > 0)
        {
            ++count;
            Frame f = Frames.Dequeue();
            frame_index = f.Index;

            for (int i = 0; i < f.Opts.Count; i++)
            {
                Player p = (Player)(All_objs[Ser_to_cli[f.Opts[i].Userid]].modules[Object_ctrl.class_name.Player]);
                p.DealInputs(f.Opts[i]);
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
            }
            Rigid_ctrl.rig_update();
            Collider_ctrl.Update_collison();
            Flow_path.Updatex();
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
        }

        if (play != null)
        {
            camara.transform.position = play.transform.position;
            camara.transform.position = new Vector3(camara.transform.position.x, camara.transform.position.y, -10);
            Tiny_map.transform.position = play.transform.position;
            Tiny_map.transform.position = new Vector3(Tiny_map.transform.position.x / 3 + Tiny_map_cre.pos_x, Tiny_map.transform.position.y / 3 + Tiny_map_cre.pos_y + 1, -10);
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
    public Fix_vector2 with_pos;
    public Dictionary<int, int> materials;
    public Obj_info()
    {
        classnames = new List<Object_ctrl.class_name>();
    }
}