using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack_change : MonoBehaviour
{
    public long sf_id;
    public long op_id;
    public int pl_id;
    public Text txt;
    // Start is called before the first frame update
    void Update()
    {
        if (Player_ctrl.checkattack((int)sf_id, (int)op_id))
        {
            txt.text = "User " + (pl_id + 1) + " eveil";
        }
        else
        {
            txt.text = "User " + (pl_id + 1) + " friend";
        }
    }

    public void Attackchange()
    {
        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.MarkUser;
        x.Userid = (int)Main_ctrl.user_id;
        x.Itemid = (int)op_id;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
    }
}
