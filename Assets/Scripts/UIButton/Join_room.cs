using Google.Protobuf.WellKnownTypes;
using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Join_room : MonoBehaviour
{
    public Button but;
    public Button but2;
    public Text txt;

    public GameObject board1;
    public GameObject board2;
    // Start is called before the first frame update
    void Start()
    {
        but.onClick.AddListener(startgame);
        but2.onClick.AddListener(endgame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void endgame()
    {
        PlayerOptData y = new PlayerOptData();
        y.Opt = PlayerOpt.ExitRoom;
        y.Userid = (int)Main_ctrl.user_id;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, y);

        Loading_ctrl.roomcnt = 0;
        Main_ctrl.players.Clear();
        Debug.Log("exit");

        board1.SetActive(false);
        board2.SetActive(true);
    }

    void startgame()
    {
        int x = 10;
        try
        {
            x = int.Parse(txt.text);
        }
        catch
        {
            Debug.Log("not");
        }
        if (x > 0 && x < 9)
        {
            Loading_ctrl.room_num = x;
            PlayerOptData y = new PlayerOptData();
            y.Opt = PlayerOpt.JoinRoom;
            y.Userid = (int)Main_ctrl.user_id;
            y.Itemid = x;

            Clisocket.Sendmessage(BODYTYPE.PlayerOptData, y);

            board2.SetActive(false);
            board1.SetActive(true);

        }
    }
}
