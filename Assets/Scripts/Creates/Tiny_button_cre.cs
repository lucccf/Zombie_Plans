using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Tiny_button_cre : MonoBehaviour
{
    // Start is called before the first frame update
    public static float parent_x = -550;
    public static float parent_y = 340;
    public static float offset_x = 50;
    public static float offset_y = -100;
    public static void Create_tinybutton(Obj_info info , long cnt)
    {
        GameObject playerPanel = GameObject.Find("PlayerPanel");
        GameObject parent = playerPanel.transform.Find("AllFacility/TinyMap").gameObject;
        GameObject tinybutton = (GameObject)Resources.Load("Prefabs/TinyFacButton");
        GameObject Building = Instantiate(tinybutton, parent.transform);
        Building.GetComponent<Tinyfacilitybutton>().facilityid = cnt;
        Building.transform.localPosition = new Vector3(info.pos.x.to_float() *4 + parent_x + offset_x, info.pos.y.to_float() *5 + parent_y + offset_y, 0);
        return ;
    }
}
