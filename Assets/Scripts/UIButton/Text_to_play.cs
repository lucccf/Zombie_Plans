using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Text_to_play : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/text.xml");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
