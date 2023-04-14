using UnityEngine;
using UnityEngine.UI;

public class Identity : MonoBehaviour
{
    // Start is called before the first frame update
    public Text txt;

    // Update is called once per frame
    void Update()
    {
        if (((Player)Main_ctrl.All_objs[Main_ctrl.Ser_to_cli[Main_ctrl.user_id]].modules[Object_ctrl.class_name.Player]).identity == Player.Identity.Populace)
        {
            txt.text = "身份：英雄";
        }
        else
        {
            txt.text = "身份：狼人";
        }
    }
}
