using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject op;
    public Button but;
    static bool a = true;
    static XmlDocument xmlDoc;
    static XmlNode fst;

    public static void Startx()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/First_open.xml");
        fst = xmlDoc.SelectSingleNode("/first");
        if (fst.InnerText == "1")
        {
            fst.InnerText = "0";
            xmlDoc.Save(Application.dataPath + "/Configs/First_open.xml");
        }
        else
        {
            a = false;
        }
    }

    void Start()
    {
        if (!a)
        {
            op.SetActive(a);
        }
        but.onClick.AddListener(opt);
    }

    void opt()
    {
        if (op.activeSelf)
        {
            op.SetActive(false);
        }
        else
        {
            op.SetActive(true);
        }
    }
}
