using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading_ctrl : MonoBehaviour
{
    public static Queue<Frame> Frames = new Queue<Frame>();
    public static long roomcnt = 0;

    public Text Cnt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cnt.text = roomcnt + "/2";
        while (Frames.Count > 0)
        {
            Frame f = Frames.Dequeue();
            for(int i = 0; i < f.Opts.Count; i++)
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

            if (roomcnt >= 1)
            {
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
