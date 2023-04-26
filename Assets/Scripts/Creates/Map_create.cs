using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;

public class Map_create : MonoBehaviour
{
    // Start is called before the first frame update

    static int floor_hei;
    static int room_cnt;
    static int floor_wid;
    static int floor_cnt;

    static List<List<Fixpoint>> col_list = new List<List<Fixpoint>>();
    static List<List<bool>> ok_list = new List<List<bool>>();
    public static void init()
    {
        col_list = new List<List<Fixpoint>>();
        ok_list = new List<List<bool>>();
    }

    public static void Wall_create()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/map.xml");
        XmlNodeList Ver_walls = xmlDoc.SelectNodes("/map/floor/floor_info");
        XmlNodeList Hor_walls = xmlDoc.SelectNodes("/map/hole/hole_info");
        XmlNode map_info = xmlDoc.SelectSingleNode("/map/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        floor_hei = wall_hei;
        room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);
        int hol_len = int.Parse(xmlDoc.SelectSingleNode("/map/hole/hole_basic_info/length").InnerText);

        List<List<int>> holes = new List<List<int>>();
        List<List<int>> walls = new List<List<int>>();

        for (int i = 0; i <= floor_cnt; i++)
        {
            holes.Add(new List<int>());
            walls.Add(new List<int>());
            col_list.Add(new List<Fixpoint>());
            ok_list.Add(new List<bool>());
        }
        for(int i = 1; i <= floor_cnt; i++)
        {
            for(int j = 0; j <= room_cnt; j++)
            {
                ok_list[i].Add(false);
            }
        }

        foreach (XmlNode p in Ver_walls)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("wall_pos"))
            {
                int pos = int.Parse(x.InnerText);
                walls[id].Add(pos);
                col_list[id].Add(new Fixpoint(pos, 0));
                Obj_info info = new Obj_info();
                info.name = "wall_b";
                info.classnames.Add(Object_ctrl.class_name.Tinymap);
                info.hei = new Fixpoint(wall_hei - (hf_thick << 1), 0);
                info.wid = new Fixpoint(hf_thick << 1, 0);
                info.col_type = Fix_col2d.col_status.Wall;
                info.pos = new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-2 * id + 1) * wall_hei * 5, 1));
                Main_ctrl.CreateObj(info);
                //info.classnames.Add(Object_ctrl.class_name.Wall);
            }
        }

        foreach (XmlNode p in Hor_walls)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("hole_pos"))
            {
                int pos = int.Parse(x.InnerText);
                col_list[id].Add(new Fixpoint(pos * 10 + 15, 1));
                col_list[id + 1].Add(new Fixpoint(pos * 10 + 15, 1));
                holes[id].Add(pos);
            }
        }
        for (int i = 0; i <= floor_cnt; i++)
        {
            holes[i].Add(0 - hol_len);
            holes[i].Add(room_cnt * floor_wid);
            holes[i].Sort();
            for (int j = 0; j < holes[i].Count - 1; j++)
            {
                Obj_info info = new Obj_info();
                info.name = "wall_b";
                info.classnames.Add(Object_ctrl.class_name.Tinymap);
                info.hei = new Fixpoint(hf_thick << 1, 0);
                info.wid = new Fixpoint(holes[i][j + 1] - holes[i][j] - hol_len, 0);
                info.col_type = Fix_col2d.col_status.Wall;
                info.pos = new Fix_vector2(new Fixpoint((holes[i][j + 1] + holes[i][j] + hol_len) * 5, 1), new Fixpoint(-i * wall_hei, 0));
                Main_ctrl.CreateObj(info);
            }
        }
        {
            Obj_info info = new Obj_info();
            info.name = "wall_b";
            info.classnames.Add(Object_ctrl.class_name.Tinymap);
            info.hei = new Fixpoint(wall_hei * floor_cnt + (hf_thick << 1), 0);
            info.wid = new Fixpoint(hf_thick << 1, 0);
            info.col_type = Fix_col2d.col_status.Wall;
            info.pos = new Fix_vector2(new Fixpoint(-hf_thick, 0), new Fixpoint(-floor_cnt * wall_hei * 5, 1));
            Main_ctrl.CreateObj(info);
        }
        {
            Obj_info info = new Obj_info();
            info.name = "wall_b";
            info.classnames.Add(Object_ctrl.class_name.Tinymap);
            info.hei = new Fixpoint(wall_hei * floor_cnt + (hf_thick << 1), 0);
            info.wid = new Fixpoint(hf_thick << 1, 0);
            info.col_type = Fix_col2d.col_status.Wall;
            info.pos = new Fix_vector2(new Fixpoint(room_cnt * floor_wid + hf_thick, 0), new Fixpoint(-floor_cnt * wall_hei * 5, 1));
            Main_ctrl.CreateObj(info);
        }

        Main_ctrl.walls = walls;
        Main_ctrl.holes = holes;
        Main_ctrl.hole_len = hol_len;
        Main_ctrl.wall_len = hf_thick;
    }

    public static void Item_create()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/crystal.xml");
        XmlNodeList Crystal_item = ItemxmlDoc.SelectNodes("/crystal/crystal_floor/crystal_info");
        XmlNode map_info = ItemxmlDoc.SelectSingleNode("/crystal/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);

        List<List<int>> holes = new List<List<int>>();
        for (int i = 0; i <= floor_cnt; i++)
        {
            holes.Add(new List<int>());
        }

        //创建水晶
        foreach (XmlNode p in Crystal_item)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("crystal_pos"))
            {
                int pos = int.Parse(x.InnerText);
                Obj_info info = new Obj_info();
                info.name = "crystal";
                info.hei = new Fixpoint(wall_hei - 3 - (hf_thick << 1), 0);
                info.wid = new Fixpoint(hf_thick << 1, 0);
                info.col_type = Fix_col2d.col_status.Collider;
                info.pos = new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-5 * id + 1) * wall_hei * 2, 1));
                info.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
                info.classnames.Add(Object_ctrl.class_name.Moster);
                Main_ctrl.CreateObj(info);
            }
        }

        Obj_info home_info = new Obj_info();
        home_info.name = "home1";
        //home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = new Fixpoint(wall_hei*4, 1);
        home_info.wid = new Fixpoint(hf_thick*5, 0);
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = new Fix_vector2(new Fixpoint(130, 0), new Fixpoint((-5 * 6 + 1) * wall_hei * 2, 1));

        Player_ctrl.HomePos = home_info.pos.Clone();

        home_info.type = "building";
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        home_info.classnames.Add(Object_ctrl.class_name.Home);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
        //创建家

        home_info = new Obj_info();
        home_info.name = "home";
        home_info.classnames.Add(Object_ctrl.class_name.Tinymap);
        home_info.classnames.Add(Object_ctrl.class_name.Moster);
        //home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = new Fixpoint(wall_hei * 4, 1);
        home_info.wid = new Fixpoint(hf_thick * 5, 0);
        home_info.col_type = Fix_col2d.col_status.Collider;
        home_info.pos = new Fix_vector2(new Fixpoint(130, 0), new Fixpoint((-5 * 6 + 1) * wall_hei * 2, 1));
        home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
    }

    public static long Bud_cnt = 0;

    static int[] chos;
    static (int, int)[] fac_pos;
    static int[] debuffs;

    public static void Facility_create()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/facility.xml");
        XmlNodeList mill_info = ItemxmlDoc.SelectNodes("/background/mill/mill_info");

        chos = new int[mill_info.Count];
        debuffs = new int[mill_info.Count];
        fac_pos = new (int, int)[mill_info.Count];
        int[] nums = new int[mill_info.Count];

        for(int i = 0; i < 5; i++)
        {
            debuffs[i] = (int)(Rand.rand() % 5);
            int jj = 0;
            while (nums[debuffs[i]] != 0)
            {
                jj++;
                debuffs[i] = (int)(Rand.rand() % 5);
                if (jj > 100) break;
            }
            nums[debuffs[i]] = 1;
        }

        for (int i = 0; i < 3; i++)
        {
            int k = (int)(Rand.rand() % (ulong)mill_info.Count);
            while (chos[k] != 0)
            {
                k = (int)(Rand.rand() % (ulong)mill_info.Count);
            }
            chos[k] = 1;
        }

        for (int i = 0; i < 2; i++)
        {
            int k = (int)(Rand.rand() % (ulong)mill_info.Count);
            while (chos[k] != 0)
            {
                k = (int)(Rand.rand() % (ulong)mill_info.Count);
            }
            chos[k] = 2;
        }

        int ix = 0;
        foreach (XmlNode p in mill_info)
        {
            Dictionary<int, int> tmp = new Dictionary<int, int>(); //修復設施所需要材料id和数量
            int mat_id = int.Parse(p.SelectSingleNode("matrial_id").InnerText);
            int mat_cnt = int.Parse(p.SelectSingleNode("matrial_number").InnerText);
            tmp[mat_id] = mat_cnt;
            int mill_id = int.Parse(p.SelectSingleNode("id").InnerText);
            int mill_pos = int.Parse(p.SelectSingleNode("mill_pos").InnerText);
            fac_pos[ix] = (mill_id, mill_pos / 28);
            string name = p.SelectSingleNode("mill_name").InnerText;
            Fix_vector2 pos = new Fix_vector2(new Fixpoint(mill_pos, 0), new Fixpoint((-5 * mill_id + 1) * floor_hei * 2, 1));

            //Debug.Log(Bud_cnt);

            if (chos[ix] == 1)
            {
                Building_single_create(name, new Fixpoint(floor_hei * 3, 1), new Fixpoint(floor_hei * 4, 1), pos, "building", Bud_cnt++, tmp, debuffs[ix] + 1);
            }
            ix++;
        }
    }

    public static void Facility_create2()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/facility.xml");
        XmlNodeList mill_info = ItemxmlDoc.SelectNodes("/background/mill/mill_info");

        int ix = 0;
        foreach (XmlNode p in mill_info)
        {
            Dictionary<int, int> tmp = new Dictionary<int, int>(); //修復設施所需要材料id和数量
            int mat_id = int.Parse(p.SelectSingleNode("matrial_id").InnerText);
            int mat_cnt = int.Parse(p.SelectSingleNode("matrial_number").InnerText);
            tmp[mat_id] = mat_cnt;
            int mill_id = int.Parse(p.SelectSingleNode("id").InnerText);
            int mill_pos = int.Parse(p.SelectSingleNode("mill_pos").InnerText);
            string name = p.SelectSingleNode("mill_name").InnerText;
            Fix_vector2 pos = new Fix_vector2(new Fixpoint(mill_pos, 0), new Fixpoint((-5 * mill_id + 1) * floor_hei * 2, 1));

            //Debug.Log(Bud_cnt);

            if (chos[ix] == 2)
            {
                Building_single_create(name, new Fixpoint(floor_hei * 3, 1), new Fixpoint(floor_hei * 4, 1), pos, "building", Bud_cnt++, tmp, debuffs[ix] + 1);
            }
            ix++;
        }
    }

    public static void Protal_create()
    {

        XmlDocument ProtalxmlDoc = new XmlDocument();
        ProtalxmlDoc.Load(Application.dataPath + "/Configs/protal.xml");
        XmlNodeList gate_info = ProtalxmlDoc.SelectNodes("/background/gate/gate_info");

        foreach (XmlNode p in gate_info)
        {
            int mill_id = int.Parse(p.SelectSingleNode("id").InnerText);
            int mill_pos = int.Parse(p.SelectSingleNode("gate_pos").InnerText);
            string name = p.SelectSingleNode("gate_name").InnerText;
            Fix_vector2 pos = new Fix_vector2(new Fixpoint(mill_pos, 0), new Fixpoint((-5 * mill_id + 1) * floor_hei * 2, 1));

            Obj_info protal_info = new Obj_info();
            protal_info.name = "protal";
            //protal_info.classnames.Add(Object_ctrl.class_name.Tinymap);
            protal_info.classnames.Add(Object_ctrl.class_name.Protalbutton);
            protal_info.hei = new Fixpoint(floor_hei*4, 1);
            protal_info.wid = new Fixpoint(floor_hei*4, 1);
            protal_info.col_type = Fix_col2d.col_status.Trigger;
            protal_info.pos = pos;
            protal_info.type = "protal";
            protal_info.classnames.Add(Object_ctrl.class_name.Trigger);
            protal_info.classnames.Add(Object_ctrl.class_name.Protal);
            protal_info.classnames.Add(Object_ctrl.class_name.Tinymap);
            GameObject gate = Main_ctrl.CreateObj(protal_info);
            gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 10);

        }
    }

    public static void TaLiBan_create()
    {

        XmlDocument ProtalxmlDoc = new XmlDocument();
        ProtalxmlDoc.Load(Application.dataPath + "/Configs/taliban.xml");
        XmlNodeList gate_info = ProtalxmlDoc.SelectNodes("/taliban/info");

        foreach (XmlNode p in gate_info)
        {
            int mill_id = int.Parse(p.SelectSingleNode("floor").InnerText);
            int mill_pos = int.Parse(p.SelectSingleNode("pos").InnerText);
            Fix_vector2 pos = new Fix_vector2(new Fixpoint(mill_pos, 0), new Fixpoint((-5 * mill_id + 1) * floor_hei * 2+3, 1));

            Obj_info protal_info = new Obj_info();
            protal_info.name = "saveboom";
            //protal_info.classnames.Add(Object_ctrl.class_name.Tinymap);
            //protal_info.classnames.Add(Object_ctrl.class_name.Protalbutton);
            protal_info.hei = new Fixpoint(floor_hei * 4, 1);
            protal_info.wid = new Fixpoint(floor_hei * 4, 1);
            protal_info.col_type = Fix_col2d.col_status.Trigger;
            protal_info.pos = pos;
            protal_info.type = "saveboom";
            protal_info.classnames.Add(Object_ctrl.class_name.Trigger);
            //protal_info.classnames.Add(Object_ctrl.class_name.Protal);
            //protal_info.classnames.Add(Object_ctrl.class_name.Tinymap);
            GameObject gate = Main_ctrl.CreateObj(protal_info);
            gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 10);

        }
    }


    public static void Building_single_create(string name , Fixpoint hei, Fixpoint wid, Fix_vector2 pos, string type , long id , Dictionary<int,int> material, int debuff) 
    {
        Obj_info home_info = new Obj_info();
        home_info.name = "facility";
        home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = hei;
        home_info.wid = wid;
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = pos;
        home_info.type = type;
        home_info.attacker_id = id;
        home_info.materials = material;
        home_info.debuff = debuff;
        home_info.classnames.Add(Object_ctrl.class_name.Tinymap);
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        home_info.classnames.Add(Object_ctrl.class_name.Facility);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);

        home_info = new Obj_info();
        home_info.name = "facility1";
        //home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = hei;
        home_info.wid = wid;
        home_info.col_type = Fix_col2d.col_status.Collider;
        home_info.attacker_id = id;
        home_info.classnames.Add(Object_ctrl.class_name.Only_Facility);
        home_info.classnames.Add(Object_ctrl.class_name.Moster);
        home_info.pos = pos;
        home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);

    }
    static int[] fst = new int[5];
    static int[] sec = new int[5];
    public static void Background_create() {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/background.xml");
        XmlNodeList Rooms = xmlDoc.SelectNodes("/background/floor/floor_info");
        XmlNode map_info = xmlDoc.SelectSingleNode("/background/map_info");

        floor_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);

        XmlDocument xmlDoc2 = new XmlDocument();
        xmlDoc2.Load(Application.dataPath + "/Configs/zombie_config.xml");
        XmlNodeList first = xmlDoc2.SelectNodes("/zombie/first/monster");
        XmlNodeList second = xmlDoc2.SelectNodes("/zombie/second/monster");

        foreach(XmlNode xx in first)
        {
            fst[int.Parse(xx.SelectSingleNode("id").InnerText)] = int.Parse(xx.SelectSingleNode("num").InnerText);
        }
        foreach (XmlNode xx in second)
        {
            sec[int.Parse(xx.SelectSingleNode("id").InnerText)] = int.Parse(xx.SelectSingleNode("num").InnerText);
        }

        BK_create(Rooms, "boss_pos", "BK_monster");
        BKitem_create(Rooms, "boss_pos", "bg_outerlayer");
        BK_create(Rooms, "mill_pos", "BK_castle");
        BKitem_create(Rooms, "mill_pos", "bg_outerlayer");
        BK_create(Rooms, "normal_pos", "BK_castle");
        BKitem_create(Rooms, "normal_pos", "bg_epic");
        BK_create(Rooms, "battle_pos", "BK_monster");
        BKitem_create(Rooms, "battle_pos", "bg_outerlayer");
        BK_create(Rooms, "home_pos", "BK_home");
        BKitem_create(Rooms, "home_pos", "bg_common");
        BKother_create("bg_outerlayer");
        Bkother_create();
    }

    static void Bkother_create()
    {
        for (int f = 1; f <= floor_cnt; f++)
        {
            for (int j = 1; j <= room_cnt; j++)
            {
                if (!ok_list[f][j])
                {
                    int id = f;
                    int pos = j;
                    Fix_vector2 xx = new Fix_vector2(new Fixpoint(pos * 10 - 5, 1) * new Fixpoint(floor_wid, 0), new Fixpoint(-id * 10 + 5, 1) * new Fixpoint(floor_hei, 0));
                    Fix_vector2 yy;
                    int k = (int)(Rand.rand() % 2 + 2);
                    if (k == 2)
                    {
                        yy = new Fix_vector2(new Fixpoint(146, 2), new Fixpoint(235, 2));
                    }
                    else
                    {
                        yy = new Fix_vector2(new Fixpoint(165, 2), new Fixpoint(295, 2));
                    }
                    Monster_create.pos_monster.Add(new Mon_pos(k, getXX(xx, id)));
                    Monster_create.size_monster.Add(yy);
                }
            }
        }
    }

    static Fix_vector2 getXX(Fix_vector2 xx, int id)
    {
        Fix_vector2 xx1= xx + new Fix_vector2(new Fixpoint((long)(Rand.rand() % (ulong)(floor_wid - 3)) - (floor_wid - 3) / 2, 0), new Fixpoint(0));
        for (int ii = 0; ii < 10; ii++)
        {
            xx1 = xx + new Fix_vector2(new Fixpoint((long)(Rand.rand() % (ulong)(floor_wid - 3)) - (floor_wid - 3) / 2, 0), new Fixpoint(0));
            bool flag = true;
            foreach (var l in col_list[id])
            {
                if ((xx1.x < l + new Fixpoint(2, 0) && xx1.x > l - new Fixpoint(2, 0)) || xx1.x < new Fixpoint(0, 0) || xx1.x > new Fixpoint(250, 0))
                {
                    flag = false;
                    break;
                }
            }
            if (flag) break;
        }
        return xx1;
    }
    static void BK_create(XmlNodeList Rooms, string xml_name, string pre_name)
    {
        foreach (XmlNode p in Rooms)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes(xml_name))
            {
                int pos = int.Parse(x.InnerText);
                Fix_vector2 xx = new Fix_vector2(new Fixpoint(pos * 10 - 5, 1) * new Fixpoint(floor_wid, 0), new Fixpoint(-id * 10 + 5, 1) * new Fixpoint(floor_hei, 0));
                Fix_vector2 yy;
                if (xml_name == "normal_pos")
                {
                    for (int ii = 0; ii < 2; ii++)
                    {
                        yy = new Fix_vector2(new Fixpoint(113, 2), new Fixpoint(225, 2));
                        Monster_create.pos_monster.Add(new Mon_pos(1, getXX(xx, id)));
                        Monster_create.size_monster.Add(yy);
                    }
                }

                if (xml_name == "mill_pos" || xml_name == "battle_pos")
                {
                    int k = (int)(Rand.rand() % 2 + 2);
                    if (k == 2)
                    {
                        yy = new Fix_vector2(new Fixpoint(146, 2), new Fixpoint(235, 2));
                    }
                    else
                    {
                        yy = new Fix_vector2(new Fixpoint(165, 2), new Fixpoint(295, 2));
                    }
                    Monster_create.pos_monster.Add(new Mon_pos(k, getXX(xx, id)));
                    Monster_create.size_monster.Add(yy);
                }

                if (xml_name == "boss_pos")
                {
                    yy = new Fix_vector2(new Fixpoint(155, 2), new Fixpoint(295, 2));
                    Monster_create.pos_monster.Add(new Mon_pos(4, getXX(xx, id)));
                    Monster_create.size_monster.Add(yy);
                }

                if (xml_name == "battle_pos")
                {
                    int jj;
                    for (jj = 0; jj < chos.Length; jj++)
                    {
                        if (id <= fac_pos[jj].Item1 + 1 && id >= fac_pos[jj].Item1 - 1 && pos <= fac_pos[jj].Item2 + 1 && pos >= fac_pos[jj].Item2 - 1) break;
                    }
                    if (jj >= chos.Length) continue;
                    int time = 0;
                    if (chos[jj] == 1)
                    {
                        Debug.Log("111");
                        for (int ii = 0; ii < fst[1]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(113, 2), new Fixpoint(225, 2));
                            Monster_create.pos_zombies1.Add(new Mon_pos(1, getXX(xx, id)));
                            Monster_create.size_zombies1.Add(yy);
                            Monster_create.time_zombies1.Add(time);
                            time += 5;
                        }
                        for (int ii = 0; ii < fst[2]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(146, 2), new Fixpoint(235, 2));
                            Monster_create.pos_zombies1.Add(new Mon_pos(2, getXX(xx, id)));
                            Monster_create.size_zombies1.Add(yy);
                            Monster_create.time_zombies1.Add(time);
                            time += 5;
                        }
                        for (int ii = 0; ii < fst[3]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(165, 2), new Fixpoint(295, 2));
                            Monster_create.pos_zombies1.Add(new Mon_pos(3, getXX(xx, id)));
                            Monster_create.size_zombies1.Add(yy);
                            Monster_create.time_zombies1.Add(time);
                            time += 5;
                        }
                    }


                    if (chos[jj] >= 1)
                    {
                        for (int ii = 0; ii < sec[1]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(113, 2), new Fixpoint(225, 2));
                            Monster_create.pos_zombies2.Add(new Mon_pos(1, getXX(xx, id)));
                            Monster_create.size_zombies2.Add(yy);
                            Monster_create.time_zombies2.Add(time);
                            time += 5;
                        }
                        for (int ii = 0; ii < sec[2]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(146, 2), new Fixpoint(235, 2));
                            Monster_create.pos_zombies2.Add(new Mon_pos(2, getXX(xx, id)));
                            Monster_create.size_zombies2.Add(yy);
                            Monster_create.time_zombies2.Add(time);
                            time += 5;
                        }
                        for (int ii = 0; ii < sec[3]; ii++)
                        {
                            yy = new Fix_vector2(new Fixpoint(165, 2), new Fixpoint(295, 2));
                            Monster_create.pos_zombies2.Add(new Mon_pos(3, getXX(xx, id)));
                            Monster_create.size_zombies2.Add(yy);
                            Monster_create.time_zombies2.Add(time);
                            time += 5;
                        }
                    }
                }
            }
        }
    }

    static void BKother_create(string pre_name)
    {
        for(int f = 1; f <= floor_cnt; f++)
        {
            for(int j = 1; j <= room_cnt; j++)
            {
                if (!ok_list[f][j])
                {
                    int id = f;
                    int pos = j;
                    int q_pi = (int)(Rand.rand() % 3 + 1);
                    Load_BL(id, pos, 200, pre_name, q_pi, 19, 1, 3, true, 13);
                    q_pi = (int)(Rand.rand() % 2 + 1);
                    Load_BL(id, pos, 10000, "stalactite", q_pi, 18, 1, 4, false, 6);
                    q_pi = (int)(Rand.rand() % 2 + 1);
                    Load_BL(id, pos, 10000, "rocks", q_pi, 18, 1, 4, true, 4);
                }
            }
        }
    }

    static void BKitem_create(XmlNodeList Rooms, string xml_name, string pre_name)
    {
        foreach (XmlNode p in Rooms)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes(xml_name))
            {
                int pos = int.Parse(x.InnerText);
                ok_list[id][pos] = true;
                int q_pi = (int)(Rand.rand() % 3 + 1);
                Load_BL(id, pos, 200, pre_name, q_pi, 19, 1, 3, true, 13);
                if (pre_name == "bg_outerlayer")
                {
                    q_pi = (int)(Rand.rand() % 2 + 1);
                    Load_BL(id, pos, 10000, "stalactite", q_pi, 18, 1, 4, false, 6);
                    q_pi = (int)(Rand.rand() % 2 + 1);
                    Load_BL(id, pos, 10000, "rocks", q_pi, 18, 1, 4, true, 4);
                }
                else if (pre_name == "bg_epic")
                {
                    q_pi = (int)(Rand.rand() % 2 + 1);
                    Load_BL(id, pos, 10000, "rocks", q_pi, 18, 5, 2, true, 4);
                    q_pi = (int)(Rand.rand() % 3 + 1);
                    Load_BL(id, pos, 10000, "grass", q_pi, 17, 1, 5, true);
                }
                else
                {
                    q_pi = (int)(Rand.rand() % 3 + 0);
                    Load_BL(id, pos, 10000, "flower", q_pi, 17, 1, 3, true);
                    q_pi = (int)(Rand.rand() % 3 + 1);
                    Load_BL(id, pos, 10000, "grass", q_pi, 17, 1, 5, true);
                }
            }
        }
    }

    static void Load_BL(int id, int pos, int y, string name, int q_pi, int hei, int fst, int id_cnt, bool to_ground, int x = 10)
    {
        for (int i = 0; i < q_pi; i++)
        {
            Fixpoint sl = new Fixpoint((long)(Rand.rand() % 101 - 50), 0) / new Fixpoint(x, 0);
            //float sl = (int)(Rand.rand() % 101 - 50) / x;
            Fixpoint sh = new Fixpoint((long)(Rand.rand() % 101 - 50), 0) / new Fixpoint(y, 0);
            //float sh = (int)(Rand.rand() % 101 - 50) / y;
            Fixpoint x1 = new Fixpoint((pos * 10 + 5) * floor_wid, 1) + sl + new Fixpoint((-q_pi * 5 + 5 + i * 10) * floor_wid, 1) / new Fixpoint(q_pi, 0);
            //float x1 = (pos - 0.5f) * floor_wid + sl + (-q_pi / 2 + i + 0.5f) * (floor_wid / q_pi);

            bool flag = true;
            foreach (var l in col_list[id])
            {
                if ((x1 < l + new Fixpoint(2, 0) && x1 > l - new Fixpoint(2, 0)) || x1 < new Fixpoint(0, 0) || x1 > new Fixpoint(250, 0))
                {
                    flag = false;
                    break;
                }
            }
            if (!flag) continue;
            int k = (int)(Rand.rand() % (ulong)id_cnt + (ulong)fst);
            GameObject obj = Instantiate((GameObject)AB.getobj(name + "_" + k));
            SpriteRenderer ren = obj.GetComponent<SpriteRenderer>();
            float y2 = ren.size.y / 2 * obj.transform.localScale.y;
            float y1;
            if (to_ground)
            {
                y1 = -(id - 0.5f) * floor_hei + sh.to_float() - floor_hei / 2f + 1 + y2;
            }
            else
            {
                y1 = -(id - 0.5f) * floor_hei + sh.to_float() + floor_hei / 2f - 1 - y2;
            }
            obj.transform.position = new Vector3(x1.to_float(), y1, hei);
        }
    }

    static Dictionary<int, int> boxitems = new Dictionary<int, int>();

    public static void Box_create()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/box.xml");
        XmlNodeList boxes = xmlDoc.SelectNodes("/box/box_floor/box_info");

        foreach(XmlNode xx in boxes)
        {
            int id = int.Parse(xx.SelectSingleNode("id").InnerText);
            foreach(XmlNode yy in xx.SelectNodes("box_pos"))
            {
                int pos = int.Parse(yy.InnerText);
                Main_ctrl.NewWolfBox(new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-2 * id) * floor_hei * 5 + 20, 1)));
            }
        }
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/boxitem.xml");
        XmlNodeList boxitem = xmlDoc.SelectNodes("/boxitem/boxitem_info");

        for (int i = 0; i < Main_ctrl.wolfboxes.Count; i++)
        {
            boxitems = new Dictionary<int, int>();
            foreach(XmlNode xx in boxitem)
            {
                int k = 0;
                int id = int.Parse(xx.SelectSingleNode("id").InnerText);
                List<int> pro = new List<int>();
                List<int> num = new List<int>();
                foreach (XmlNode yy in xx.SelectNodes("boxitem_probability"))
                {
                    k += int.Parse(yy.InnerText);
                    pro.Add(k);
                }
                foreach (XmlNode yy in xx.SelectNodes("boxitem_num"))
                {
                    num.Add(int.Parse(yy.InnerText));
                }
                int l = (int)(Rand.rand() % (ulong)k);
                for(int j = 0; j < pro.Count; j++)
                {
                    if (l < pro[j])
                    {
                        boxitems[id] = pro[j];
                        break;
                    }
                }
            }
            Main_ctrl.wolfboxes[i].InitBoxItem(boxitems);
        }
    }
}


/*
 * mage
 * 146 235
 * knight
 * 165 295
 * Devil
 * 155 295
 * Terrorist
 * 148 303
 */