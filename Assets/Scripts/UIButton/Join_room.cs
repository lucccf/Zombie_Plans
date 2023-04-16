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
    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        but.onClick.AddListener(startgame);
    }

    // Update is called once per frame
    void Update()
    {

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

            SceneManager.LoadScene("Loading");
        }
    }
}
