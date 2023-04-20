using System.Collections.Generic;
using UnityEngine;
using Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Concurrent;

public class Loading_ctrl : MonoBehaviour
{
    public static ConcurrentQueue<Frame> Frames = new ConcurrentQueue<Frame>();
    public static int room_num = 1;
    public static long roomcnt = 0;

    public Text Cnt;

    // Update is called once per frame
    void Update()
    {
        Cnt.text = "正在匹配" + roomcnt + "/" + room_num;
        while (Frames.Count > 0)
        {
            Frame f;
            if (!Frames.TryDequeue(out f)) break;
            for (int i = 0; i < f.Opts.Count; i++)
            {
                if (f.Opts[i].Opt == PlayerOpt.JoinRoom)
                {
                    Main_ctrl.players.Add(f.Opts[i].Userid);
                    roomcnt++;
                }
                else if (f.Opts[i].Opt == PlayerOpt.ExitRoom)
                {
                    Main_ctrl.players.Remove(f.Opts[i].Userid);
                    roomcnt--;
                }
            }

            if (roomcnt >= room_num)
            {
                roomcnt = 0;
                while(Main_ctrl.Frames.TryPeek(out f))
                {
                    if (f.Index != 1)
                    {
                        Main_ctrl.Frames.TryDequeue(out f);
                    }
                    else
                    {
                        break;
                    }
                }
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
