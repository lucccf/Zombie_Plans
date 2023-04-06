using System.Collections.Generic;
using System.Xml;
using UnityEngine;

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
        floor_hei = wall_hei;
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
                info.col_type = Fix_col2d.col_status.Collider;
                info.pos = new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-5 * id + 1) * wall_hei * 2 , 1));
                info.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
                info.classnames.Add(Object_ctrl.class_name.Moster);
                Main_ctrl.CreateObj(info);

                /*
                Obj_info p = new Obj_info();
                p.name = "Monster1";
                p.hei = size_monster[i].y.Clone();
                p.wid = size_monster[i].x.Clone();
                p.pos = pos_monster[i];
                p.col_type = Fix_col2d.col_status.Collider;
                p.classnames.Add(Object_ctrl.class_name.Fix_rig2d);
                p.classnames.Add(Object_ctrl.class_name.Moster);
                //Debug.Log(p.pos.x.to_float() + " " + p.pos.y.to_float());
                Main_ctrl.CreateObj(p);
                */
                //info.classnames.Add(Object_ctrl.class_name.Wall);
            }
        }

        Obj_info home_info = new Obj_info();
        home_info.name = "home";
        home_info.hei = new Fixpoint(wall_hei + 1 - (hf_thick << 1), 0);
        home_info.wid = new Fixpoint(hf_thick << 1 + 2, 0);
        home_info.col_type = Fix_col2d.col_status.Trigger;
        home_info.pos = new Fix_vector2(new Fixpoint(130, 0), new Fixpoint((-5 * 6 + 1) * wall_hei * 2, 1));
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

        Dictionary<int,int> tmp = new Dictionary<int,int>(); //修復設施所需要材料id和数量
        tmp[1] = 5;
        Building_single_create("facility", new Fixpoint(wall_hei + 1 - (hf_thick << 1), 0), new Fixpoint(hf_thick << 1 + 2, 0), new Fix_vector2(new Fixpoint(170, 0), new Fixpoint((-5* 6 + 1) * wall_hei * 2 , 1)), "building", Bud_cnt++ , tmp);
    }

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

            Debug.Log(Bud_cnt);

            Building_single_create(name, new Fixpoint(floor_hei, 0), new Fixpoint(floor_hei, 0), pos, "building", Bud_cnt++, tmp);

        }
    }



    public static void Building_single_create(string name , Fixpoint hei, Fixpoint wid, Fix_vector2 pos, string type , long id , Dictionary<int,int> material) 
    {
        Obj_info home_info = new Obj_info();
        home_info.name = "facility";
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
                GameObject obj = Instantiate((GameObject)AB.getobj("background/" + pre_name));
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
