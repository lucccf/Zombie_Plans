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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
