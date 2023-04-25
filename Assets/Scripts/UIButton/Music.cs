using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    public static float Battle_Volume = -1;
    public static float Music_Volume = -1;
    public Slider sl_music, sl_Battle;
    public static Dictionary<int, AudioSource> aas = new Dictionary<int, AudioSource>();

    void Start()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/Music_volume.xml");
        XmlNode vol = xmlDoc.SelectSingleNode("/Volume");
        if (Battle_Volume < 0)
        {
            Battle_Volume = float.Parse(vol.SelectSingleNode("battle").InnerText);
        }
        if (Music_Volume < 0)
        {
            Music_Volume = float.Parse(vol.SelectSingleNode("music").InnerText);
        }
        foreach(var aa in aas.Values)
        {
            aa.rolloffMode = AudioRolloffMode.Linear;
            aa.minDistance = 1;
            aa.maxDistance = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Battle_Volume = sl_Battle.value;
        foreach (var aa in aas.Values)
        {
            aa.volume = Battle_Volume;
        }
    }
}
