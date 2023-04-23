using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Text_to_play : MonoBehaviour
{
    public Text text;
    Dictionary<int, List<string>> words1 = new Dictionary<int, List<string>>();
    Dictionary<int, List<string>> words2 = new Dictionary<int, List<string>>();
    int word_pos = 0;
    float t = 0;
    float dt = 10f;

    // Start is called before the first frame update
    void Start()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/text.xml");
        XmlNodeList player = xmlDoc.SelectNodes("/Text/player/info");
        XmlNodeList wolf = xmlDoc.SelectNodes("/Text/wolf/info");
        foreach(XmlNode pp in player)
        {
            List<string> s1 = new List<string>();
            foreach(XmlNode xx in pp.SelectNodes("text"))
            {
                s1.Add(xx.InnerText);
            }
            words1[int.Parse(pp.SelectSingleNode("id").InnerText)] = s1;
            Debug.Log(pp.SelectSingleNode("id").InnerText);
        }
        foreach (XmlNode pp in wolf)
        {
            List<string> s1 = new List<string>();
            foreach (XmlNode xx in pp.SelectNodes("text"))
            {
                s1.Add(xx.InnerText);
            }
            words2[int.Parse(pp.SelectSingleNode("id").InnerText)] = s1;
            Debug.Log(pp.SelectSingleNode("id").InnerText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Flow_path.get_flag() < 4 && Flow_path.get_flag() >= 0)
        {
            if (Main_ctrl.main_identity == Player.Identity.Populace)
            {
                int k = words1[Flow_path.get_flag() + 1].Count;
                if (t + dt < Time.time)
                {
                    word_pos = word_pos + 1;
                    t = Time.time;
                }
                word_pos %= k;
                text.text = words1[Flow_path.get_flag() + 1][word_pos];
            }
            else
            {
                int k = words2[Flow_path.get_flag() + 1].Count;
                if (t + dt < Time.time)
                {
                    word_pos = word_pos + 1;
                    t = Time.time;
                }
                word_pos %= k;
                text.text = words2[Flow_path.get_flag() + 1][word_pos];
            }
        }
    }
}
