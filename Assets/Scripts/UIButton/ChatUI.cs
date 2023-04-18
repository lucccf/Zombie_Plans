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
        x.Content = textpanel.GetComponent<Text>().text;   
        Clisocket.Sendmessage(BODYTYPE.PlayerMessage, x);
        gameObject.SetActive(false);
        ClientSend.Send = true;
        commitbuttonable = false;
        t = Time.time;
        Debug.Log("Enable Changed"+ commitbuttonable);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
