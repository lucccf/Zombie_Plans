﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Map_create : MonoBehaviour
{
    // Start is called before the first frame update

    static int floor_hei;
    static int room_cnt;
    static int floor_wid;
    static int floor_cnt;

    public static void Wall_create()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/map.xml");
        XmlNodeList Ver_walls = xmlDoc.SelectNodes("/map/floor/floor_info");
        XmlNodeList Hor_walls = xmlDoc.SelectNodes("/map/hole/hole_info");
        XmlNode map_info = xmlDoc.SelectSingleNode("/map/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);
        int hol_len = int.Parse(xmlDoc.SelectSingleNode("/map/hole/hole_basic_info/length").InnerText);

        List<List<int>> holes = new List<List<int>>();
        for(int i = 0; i <= floor_cnt; i++)
        {
            holes.Add(new List<int>());
        }

        foreach (XmlNode p in Ver_walls)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("wall_pos"))
            {
                int pos = int.Parse(x.InnerText);
                Obj_info info = new Obj_info();
                info.name = "wall_b";
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
                holes[id].Add(int.Parse(x.InnerText));
            }
        }
        for (int i = 0; i <= floor_cnt; i++)
        {
            holes[i].Add(0 - hol_len);
            holes[i].Add(room_cnt * floor_wid);
            holes[i].Sort();
            for(int j = 0; j < holes[i].Count - 1; j++)
            {
                Obj_info info = new Obj_info();
                info.name = "wall_b";
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
            info.hei = new Fixpoint(wall_hei * floor_cnt + (hf_thick << 1), 0);
            info.wid = new Fixpoint(hf_thick << 1, 0);
            info.col_type = Fix_col2d.col_status.Wall;
            info.pos = new Fix_vector2(new Fixpoint(-hf_thick, 0), new Fixpoint(-floor_cnt * wall_hei * 5, 1));
            Main_ctrl.CreateObj(info);
        }
        {
            Obj_info info = new Obj_info();
            info.name = "wall_b";
            info.hei = new Fixpoint(wall_hei * floor_cnt + (hf_thick << 1), 0);
            info.wid = new Fixpoint(hf_thick << 1, 0);
            info.col_type = Fix_col2d.col_status.Wall;
            info.pos = new Fix_vector2(new Fixpoint(room_cnt * floor_wid + hf_thick, 0), new Fixpoint(-floor_cnt * wall_hei * 5, 1));
            Main_ctrl.CreateObj(info);
        }
    }

    public static void Item_create()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/crystal.xml");
        XmlNodeList Crystal_item = ItemxmlDoc.SelectNodes("/crystal/crystal_floor/crystal_info");
        XmlNode map_info = ItemxmlDoc.SelectSingleNode("/crystal/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        int room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        int floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        int floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);

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
                info.col_type = Fix_col2d.col_status.Trigger;
                info.pos = new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-2 * id + 1) * wall_hei * 5 - 15, 1));
                info.classnames.Add(Object_ctrl.class_name.Trigger);
                Main_ctrl.CreateObj(info);
                //info.classnames.Add(Object_ctrl.class_name.Wall);
            }
        }

        Obj_info home_info = new Obj_info();
        home_info.name = "home";
        home_info.hei = new Fixpoint(wall_hei + 1 - (hf_thick << 1), 0);
        home_info.wid = new Fixpoint(hf_thick << 1 + 2, 0);
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = new Fix_vector2(new Fixpoint(130, 0), new Fixpoint((-2 * 6 + 1) * wall_hei * 5 - 15, 1));
        home_info.type = "building";
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
        //创建家
    }

    public static long Bud_cnt = 0;

    public static void Building_create()
    {
        XmlDocument ItemxmlDoc = new XmlDocument();
        ItemxmlDoc.Load(Application.dataPath + "/Configs/crystal.xml");
        XmlNodeList Crystal_item = ItemxmlDoc.SelectNodes("/crystal/crystal_floor/crystal_info");
        XmlNode map_info = ItemxmlDoc.SelectSingleNode("/crystal/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);


        Obj_info home_info = new Obj_info();
        home_info.name = "facility";
        home_info.hei = new Fixpoint(wall_hei + 1 - (hf_thick << 1), 0);
        home_info.wid = new Fixpoint(hf_thick << 1 + 2, 0);
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = new Fix_vector2(new Fixpoint(170, 0), new Fixpoint((-2 * 6 + 1) * wall_hei * 5 - 15, 1));
        home_info.type = "building";
        home_info.attacker_id = Bud_cnt++;
        home_info.classnames.Add(Object_ctrl.class_name.Trigger);
        home_info.classnames.Add(Object_ctrl.class_name.Facility);
        GameObject home = Main_ctrl.CreateObj(home_info);
        home.transform.position = new Vector3(home.transform.position.x, home.transform.position.y, 10);
        //创建家
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
        BK_create(Rooms, "mill_pos", "BK_castle");
        BK_create(Rooms, "normal_pos", "BK_castle");
        BK_create(Rooms, "battle_pos", "BK_monster");
        BK_create(Rooms, "home_pos", "BK_home");
    }

    static void BK_create(XmlNodeList Rooms, string xml_name, string pre_name)
    {
        foreach (XmlNode p in Rooms)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes(xml_name))
            {
                int pos = int.Parse(x.InnerText);
                GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs/background/" + pre_name));
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
}
