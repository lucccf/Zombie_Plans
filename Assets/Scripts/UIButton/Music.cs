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
    static AudioSource ab;
    int p = 0;
    public static void Startx()
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
        ab = Main_ctrl.camara.GetComponent<AudioSource>();
        ab.loop = true;
        ab.spatialBlend = 1;
        ab.clip = (AudioClip)AB.getobj("first_music");
        ab.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Battle_Volume = sl_Battle.value;
        Music_Volume = sl_music.value;
        foreach (var aa in aas.Values)
        {
            aa.volume = Battle_Volume;
        }
        ab.volume = Music_Volume;
        if (Flow_path.get_flag() == 2 && p != 2)
        {
            ab.clip = (AudioClip)AB.getobj("second_music");
        }
        p = Flow_path.get_flag();
    }
}
