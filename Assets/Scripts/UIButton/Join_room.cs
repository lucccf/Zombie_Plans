using Google.Protobuf.WellKnownTypes;
using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Join_room : MonoBehaviour
{
    public Button but;
    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        but.onClick.AddListener(startgame);
    }

    void startgame()
    {
        int x = int.Parse(txt.text);
        if (x > 0 && x < 9)
        {
            PlayerOptData y = new PlayerOptData();
            y.Opt = PlayerOpt.JoinRoom;
            y.Userid = (int)Main_ctrl.user_id;
            y.Itemid = x;

            Clisocket.Sendmessage(BODYTYPE.PlayerOptData, y);
        }
    }
}
