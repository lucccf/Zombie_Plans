using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Item_create : MonoBehaviour
{
    // Start is called before the first frame update
    XmlDocument ItemxmlDoc = new XmlDocument();
    void Start()
    {
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

        foreach (XmlNode p in Crystal_item)
        {
            int id = int.Parse(p.SelectSingleNode("id").InnerText);
            foreach (XmlNode x in p.SelectNodes("crystal_pos"))
            {
                int pos = int.Parse(x.InnerText);
                Obj_info info = new Obj_info();
                info.name = "crystal";
                info.hei = new Fixpoint(wall_hei-3 - (hf_thick << 1), 0);
                info.wid = new Fixpoint(hf_thick << 1, 0);
                info.col_type = Fix_col2d.col_status.Trigger;
                info.pos = new Fix_vector2(new Fixpoint(pos, 0), new Fixpoint((-2 * id + 1) * wall_hei * 5 -15, 1));
                Main_ctrl.CreateObj(info);
                //info.classnames.Add(Object_ctrl.class_name.Wall);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
