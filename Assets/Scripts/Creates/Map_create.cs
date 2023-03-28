using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Map_create : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Wall_create()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/map.xml");
        XmlNodeList Ver_walls = xmlDoc.SelectNodes("/map/floor/floor_info");
        XmlNodeList Hor_walls = xmlDoc.SelectNodes("/map/hole/hole_info");
        XmlNode map_info = xmlDoc.SelectSingleNode("/map/map_info");

        int hf_thick = int.Parse(map_info.SelectSingleNode("floor_half_thick").InnerText);
        int wall_hei = int.Parse(map_info.SelectSingleNode("floor_hei").InnerText);
        int room_cnt = int.Parse(map_info.SelectSingleNode("room_cnt").InnerText);
        int floor_wid = int.Parse(map_info.SelectSingleNode("floor_wid").InnerText);
        int floor_cnt = int.Parse(map_info.SelectSingleNode("floor_cnt").InnerText);
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
