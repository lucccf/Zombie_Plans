using System;
using UnityEngine;
using UnityEngine.UI;

public class CountingDown : MonoBehaviour
{
    //public Fixpoint curtime = new Fixpoint(0); // 目前时间，以秒为单位
    //public Fixpoint alltime = new Fixpoint(420, 0); //总时间

    public Text countingtext;

    void Start()
    {
        //curtime = new Fixpoint(0);

        // 初始化倒计时文本
        countingtext.text = FormatTime(Flow_path.countdown.to_int());
    }

    void Update()
    {
        //curtime = new Fixpoint(Main_ctrl.frame_index, 0) * Dt.dt;

        // 将剩余时间格式化为分钟：秒数，并更新倒计时文本
        countingtext.text = FormatTime(Flow_path.countdown.to_int());
    }

    // 将秒数格式化为分钟：秒数的字符串
    string FormatTime(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
