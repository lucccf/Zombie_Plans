using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatbubble : MonoBehaviour
{
    public Text txt;
    public Player player;
    public GameObject bub;

    private float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        bub.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = player.words;
        if (player.words_ok)
        {
            t = Time.time;
            bub.SetActive(true);
            player.words_ok = false;
        }
        if (t + 5f < Time.time)
        {
            bub.SetActive(false);
        }
    }
}
