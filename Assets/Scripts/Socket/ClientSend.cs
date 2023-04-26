using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    public static bool Send = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Send)
        {
            foreach (PlayerOpt p in Clisocket.keys.Keys)
            {
                if (Clisocket.MyGetKey(Clisocket.keys[p]))
                {
                    PlayerOptData x = new PlayerOptData();
                    x.Opt = p;
                    x.Userid = (int)Main_ctrl.user_id;
                    Debug.Log("send" + x);
                    Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
                }
            }
        }
    }
}
