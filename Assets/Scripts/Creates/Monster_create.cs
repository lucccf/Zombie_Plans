using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public class Mon_pos
{
    public int type;
    public Fix_vector2 pos;

    public Mon_pos(int type, Fix_vector2 pos)
    {
        this.type = type;
        this.pos = pos;
    }
}

public class Monster_create : MonoBehaviour
{
    public static List<Mon_pos> pos_monster = new List<Mon_pos>();
    public static List<Fix_vector2> size_monster = new List<Fix_vector2>();
    public static List<Mon_pos> pos_zombies1 = new List<Mon_pos>();
    public static List<Fix_vector2> size_zombies1 = new List<Fix_vector2>();
    public static List<Mon_pos> pos_zombies2 = new List<Mon_pos>();
    public static List<Fix_vector2> size_zombies2 = new List<Fix_vector2>();
    public static float cnt1 = 0;

    public static void init()
    {
        pos_monster = new List<Mon_pos>();
        size_monster = new List<Fix_vector2>();
        pos_zombies1 = new List<Mon_pos>();
        size_zombies1 = new List<Fix_vector2>();
        pos_zombies2 = new List<Mon_pos>();
        size_zombies2 = new List<Fix_vector2>();
    }

    public class cre_items
    {
        public List<string> su_name = new List<string>();
        public List<int> su_num = new List<int>();
        public int ran_cnt = 0;
        public List<string> ra_name = new List<string>();
        public List<int> ra_num = new List<int>();
        public Dictionary<int, (string, int)> ro_info = new Dictionary<int, (string, int)>();
    }

    static Dictionary<string, cre_items> All_m_items = new Dictionary<string, cre_items>();

    public static void Get_mon_items()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/monster_item.xml");
        XmlNodeList mos = xmlDoc.SelectNodes("/monster_item/monster");
        foreach(XmlNode xx in mos)
        {
            string name = xx.SelectSingleNode("id").InnerText;
            cre_items c1 = new cre_items();
            XmlNode x1 = xx.SelectSingleNode("surely");
            if (x1 != null)
            {
                foreach(XmlNode yy in x1.SelectNodes("id"))
                {
                    c1.su_name.Add(Main_ctrl.GetItemById(int.Parse(yy.InnerText)).name);
                }
                foreach (XmlNode yy in x1.SelectNodes("num"))
                {
                    c1.su_num.Add(int.Parse(yy.InnerText));
                }
            }
            XmlNode x2 = xx.SelectSingleNode("random");
            if (x2 != null)
            {
                c1.ran_cnt = int.Parse(x2.SelectSingleNode("type").InnerText);
                foreach (XmlNode yy in x2.SelectNodes("id"))
                {
                    c1.ra_name.Add(Main_ctrl.GetItemById(int.Parse(yy.InnerText)).name);
                }
                foreach (XmlNode yy in x2.SelectNodes("num"))
                {
                    c1.ra_num.Add(int.Parse(yy.InnerText));
                }
            }
            XmlNode x3 = xx.SelectSingleNode("room");
            if (x3 != null)
            {
                foreach (XmlNode yy in x3.SelectNodes("area"))
                {
                    int p1 = int.Parse(yy.SelectSingleNode("areaid").InnerText);
                    string p2 = Main_ctrl.GetItemById(int.Parse(yy.SelectSingleNode("id").InnerText)).name;
                    int p3 = int.Parse(yy.SelectSingleNode("num").InnerText);
                    c1.ro_info[p1] = (p2, p3);
                }
            }
            All_m_items[name] = c1;
        }
    }

    public static void Mon_create1()  //生成资源怪
    {
        for(int i = 0; i < pos_monster.Count; i++)
        {
            create(pos_monster[i], size_monster[i], 0);
        }
    }

    static Dictionary<string, int> get_item(cre_items c, int type)
    {
        Dictionary<string, int> it1 = new Dictionary<string, int>();
        if (c == null) return it1;
        for(int i = 0; i < c.su_num.Count; i++)
        {
            it1[c.su_name[i]] = c.su_num[i];
        }
        int[] cn = new int[c.ra_num.Count];
        for (int i = 0; i < c.ra_num.Count; i++) cn[i] = 0;
        for(int i = 0; i < c.ran_cnt; i++)
        {
            int k = (int)(Rand.rand() % (ulong)c.ra_num.Count);
            while (cn[k] != 0)
            {
                k = (int)(Rand.rand() % (ulong)c.ra_num.Count);
            }
            cn[k] = 1;
            it1[c.ra_name[k]] = c.ra_num[k];
        }
        if (c.ro_info.Count > 0)
        {
            it1[c.ro_info[type].Item1] = c.ro_info[type].Item2;
        }
        return it1;
    }

    private static void create(Mon_pos p1, Fix_vector2 p2, int type2)
    {
        Obj_info p = new Obj_info();
        switch (p1.type)
        {
            case 1:
                p.name = "Monster1";
                break;
            case 2:
                p.name = "Mage";
                break;
            case 3:
                p.name = "knight";
                break;
            case 4:
                p.name = "Devil";
                break;
            case 5:
                p.name = "Terrorist";
                break;
        }
        if (All_m_items.ContainsKey(p.name))
        {
            p.falls = get_item(All_m_items[p.name], 1);
        }
        else
        {
            p.falls = get_item(null, -1);
        }
        p.cre_type = type2;
        p.hei = p2.y.Clone();
        p.wid = p2.x.Clone();
        p.pos = p1.pos;
        p.col_type = Fix_col2d.col_status.Collider;
        p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
        p.classnames.Add(Object_ctrl.class_name.Moster);
        p.classnames.Add(Object_ctrl.class_name.Tinymap);
        //Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
        Main_ctrl.CreateObj(p);
        Flow_path.zombie_cnt++;
    }

    public static void Zom_create1()  //生成僵尸，后期替换模型和ai
    {
        for (int i = 0; i < pos_zombies1.Count; i++)
        {
            create(pos_zombies1[i], size_zombies1[i], 1);
        }
    }

    public static void Zom_create2()  //生成僵尸，后期替换模型和ai
    {
        for (int i = 0; i < pos_zombies2.Count; i++)
        {
            create(pos_zombies2[i], size_zombies2[i], 1);
        }
    }
}
