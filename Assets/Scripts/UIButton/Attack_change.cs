using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack_change : MonoBehaviour
{
    public long sf_id;//唯一
    public long op_id;//唯一
    public int pl_id;//第几个
    public Text usertxt;
    public Text typetxt;
    // Start is called before the first frame update
    private void Start()
    {
        typetxt.text = "友方";

    }

    void Update()
    {
        if (Player_ctrl.checkattack((int)sf_id, (int)op_id))
        {
            typetxt.text = "敌人";
        }
        else
        {
            typetxt.text = "友方";
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
