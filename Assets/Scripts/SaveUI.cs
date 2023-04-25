using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject savetext;
    public GameObject commitbutton;
    public int talibanid;
    void Start()
    {
        savetext.GetComponent<Text>().text = "30";
        commitbutton.GetComponent<Button>().onClick.AddListener(trysave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void trysave()
    {
        Debug.Log("trysave");
    }
}
