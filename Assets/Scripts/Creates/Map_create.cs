﻿using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;
using static UnityEditor.PlayerSettings;

public class Map_create : MonoBehaviour
{
    // Start is called before the first frame update

    static int floor_hei;
    static int room_cnt;
    static int floor_wid;
    static int floor_cnt;

    static List<List<float>> col_list = new List<List<float>>();

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
            col_list.Add(new List<float>());
        }

        foreach (XmlNode p in Ver_walls)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("wall_pos"))
            {
                int pos = int.Parse(x.InnerText);
                walls[id].Add(pos);
                col_list[id].Add(pos);
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
                col_list[id].Add(pos + 1.5f);
                col_list[id + 1].Add(pos + 1.5f);
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
        home_info.name = "home";
        home_info.classnames.Add(Object_ctrl.class_name.Tinymap);
        //home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = new Fixpoint(wall_hei*4, 1);
        home_info.wid = new Fixpoint(hf_thick*5, 0);
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = new Fix_vector2(new Fixpoint(130, 0), new Fixpoint((-5 * 6 + 1) * wall_hei * 2, 1));
        home_info.type = "building";
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
        //创建家
    }

    public static long Bud_cnt = 0;

    public static void Facility_create()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/facility.xml");
        XmlNodeList mill_info = ItemxmlDoc.SelectNodes("/background/mill/mill_info");

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

            Building_single_create(name, new Fixpoint(floor_hei*3, 1), new Fixpoint(floor_hei*4, 1), pos, "building", Bud_cnt++, tmp);

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
            protal_info.hei = new Fixpoint(floor_hei*2, 1);
            protal_info.wid = new Fixpoint(floor_hei*2, 1);
            protal_info.col_type = Fix_col2d.col_status.Trigger;
            protal_info.pos = pos;
            protal_info.type = "protal";
            protal_info.classnames.Add(Object_ctrl.class_name.Trigger);
            protal_info.classnames.Add(Object_ctrl.class_name.Protal);
            GameObject gate = Main_ctrl.CreateObj(protal_info);
            gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 10);

        }
    }


    public static void Building_single_create(string name , Fixpoint hei, Fixpoint wid, Fix_vector2 pos, string type , long id , Dictionary<int,int> material) 
    {
        Obj_info home_info = new Obj_info();
        home_info.name = "facility";
        home_info.classnames.Add(Object_ctrl.class_name.Tinymap);
        home_info.classnames.Add(Object_ctrl.class_name.Tinybutton);
        home_info.hei = hei;
        home_info.wid = wid;
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = pos;
        home_info.type = type;
        home_info.attacker_id = id;
        home_info.materials = material;
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        home_info.classnames.Add(Object_ctrl.class_name.Facility);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
    }

    public static void Background_create() {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/background.xml");
        XmlNodeList Rooms = xmlDoc.SelectNodes("/background/floor/floor_info");
        XmlNode map_info = xmlDoc.SelectSingleNode("/background/map_info");

        floor_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);

        BK_create(Rooms, "boss_pos", "BK_monster");
        BKitem_create(Rooms, "boss_pos", "bg_common");
        BK_create(Rooms, "mill_pos", "BK_castle");
        BKitem_create(Rooms, "mill_pos", "bg_common");
        BK_create(Rooms, "normal_pos", "BK_castle");
        BKitem_create(Rooms, "normal_pos", "bg_common");
        BK_create(Rooms, "battle_pos", "BK_monster");
        BKitem_create(Rooms, "battle_pos", "bg_epic");
        BK_create(Rooms, "home_pos", "BK_home");
        BKitem_create(Rooms, "home_pos", "bg_common");
    }

    static void BK_create(XmlNodeList Rooms, string xml_name, string pre_name)
    {
        foreach (XmlNode p in Rooms)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes(xml_name))
            {
                int pos = int.Parse(x.InnerText);
                GameObject obj = Instantiate((GameObject)AB.getobj("BK_home"));
                obj.transform.position = new Vector3((pos - 0.5f) * floor_wid, - (id - 0.5f) * floor_hei, 20);
                if (xml_name == "normal_pos")
                {
                    Monster_create.pos_monster.Add(new Fix_vector2(new Fixpoint(pos * 10 - 5, 1) * new Fixpoint(floor_wid , 0), new Fixpoint(-id * 10 + 5, 1) * new Fixpoint(floor_hei, 0)));
                    Monster_create.size_monster.Add(new Fix_vector2(new Fixpoint(113, 2), new Fixpoint(225, 2)));
                }
                if (xml_name == "battle_pos")
                {
                    Monster_create.pos_zombies.Add(new Fix_vector2(new Fixpoint(pos * 10 - 5, 1) * new Fixpoint(floor_wid, 0), new Fixpoint(-id * 10 + 5, 1) * new Fixpoint(floor_hei, 0)));
                    Monster_create.size_zombies.Add(new Fix_vector2(new Fixpoint(113, 2), new Fixpoint(225, 2)));
                }
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                spriteRenderer.size = new Vector2(floor_wid, floor_hei);
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
                int q_pi = (int)(Rand.rand() % 3 + 1);
                for(int i = 0; i < q_pi; i++)
                {
                    float sl = (Rand.rand() % 101 - 50) / 10f;
                    float x1 = (pos - 0.5f) * floor_wid + sl + (-q_pi / 2 + i + 0.5f) * (floor_wid / q_pi);
                    float y1 = -(id - 0.5f) * floor_hei;
                    bool flag = true;
                    foreach (var l in col_list[id])
                    {
                        if (x1 < l + 3 && x1 > l - 3 || x1 < 0 || x1 > 280)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (!flag) continue;
                    int k = (int)(Rand.rand() % 3 + 1);
                    Debug.Log(pre_name + "_" + k);
                    GameObject obj = new GameObject("pillar");
                    SpriteRenderer ren = obj.AddComponent<SpriteRenderer>();
                    ren.sprite = (Sprite)AB.getobj(pre_name + "_" + k);
                    obj.transform.position = new Vector3(x1, y1, 19);
                    obj.transform.localScale = new Vector3(1.8f, 1.8f, 1f);
                }
            }
        }
    }
}
