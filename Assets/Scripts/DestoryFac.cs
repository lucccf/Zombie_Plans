using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryFac : MonoBehaviour
{

    public void Facchange()
    {
        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.Markfac;
        x.Userid = (int)Main_ctrl.user_id;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
    }
}
