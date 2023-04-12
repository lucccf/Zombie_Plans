using System.Collections.Generic;
using UnityEngine;
using Net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Concurrent;

public class Loading_ctrl : MonoBehaviour
{
    public static ConcurrentQueue<Frame> Frames = new ConcurrentQueue<Frame>();
    public static long roomcnt = 0;

    public Text Cnt;

    // Update is called once per frame
    void Update()
    {
        Cnt.text = roomcnt + "/4";
        while (Frames.Count > 0)
        {
            Frame f;
            if (!Frames.TryDequeue(out f)) break;
            for (int i = 0; i < f.Opts.Count; i++)
            {
                if (f.Opts[0].Opt == PlayerOpt.UserLogin)
                {
                    Main_ctrl.players.Add(f.Opts[0].Userid);
                    roomcnt++;
                }
                else if (f.Opts[0].Opt == PlayerOpt.UserLogout)
                {
                    Main_ctrl.players.Remove(f.Opts[0].Userid);
                    roomcnt--;
                }
            }

            if (roomcnt >= 4)
            {
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
