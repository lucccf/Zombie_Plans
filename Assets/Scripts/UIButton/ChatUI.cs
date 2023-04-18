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
    void Start()
    {
        commitbutton.GetComponent<Button>().onClick.AddListener(CommitMessage);
    }

    void CommitMessage() {
        PlayerMessage x = new PlayerMessage();
        x.Userid = (int)Main_ctrl.user_id;
        x.Content = textpanel.GetComponent<Text>().text;   
        Clisocket.Sendmessage(BODYTYPE.PlayerMessage, x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
