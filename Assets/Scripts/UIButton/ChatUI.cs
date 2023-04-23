using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject commitbutton;
    public GameObject textpanel;
    public Button ctbutton;
    public static float t = 0;
    public static bool commitbuttonable = true;

    void Start()
    {
        commitbutton.GetComponent<Button>().onClick.AddListener(CommitMessage);
    }

    void CommitMessage() {
        PlayerMessage x = new PlayerMessage();
        x.Userid = (int)Main_ctrl.user_id;
        Text tmptext = textpanel.GetComponent<Text>();
        x.Content = tmptext.text;
        Clisocket.Sendmessage(BODYTYPE.PlayerMessage, x);
        tmptext.text = " ";
        gameObject.SetActive(false);
        ClientSend.Send = true;
        commitbuttonable = false;
        t = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
