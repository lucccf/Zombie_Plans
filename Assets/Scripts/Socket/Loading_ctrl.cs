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
        Cnt.text = roomcnt + "/3";
        while (Frames.Count > 0)
        {
            Frame f = Frames.Dequeue();
            Main_ctrl.players.Enqueue(f.Userid);
            roomcnt++;

            if (roomcnt >= 3)
            {
                SceneManager.LoadScene("Battle");
            }
        }
    }
}
