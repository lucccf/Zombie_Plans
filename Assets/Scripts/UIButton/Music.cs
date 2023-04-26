using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    static int p = 0;
    static XmlDocument xmlDoc;
    static XmlNode vol;
    public static void Startx()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/Configs/Music_volume.xml");
        vol = xmlDoc.SelectSingleNode("/Volume");
        Battle_Volume = float.Parse(vol.SelectSingleNode("battle").InnerText);
        Music_Volume = float.Parse(vol.SelectSingleNode("music").InnerText);
        if (Main_ctrl.camara == null) return;
        ab = Main_ctrl.camara.GetComponent<AudioSource>();
        ab.loop = true;
        ab.spatialBlend = 1;
        ab.clip = (AudioClip)AB.getobj("first_music");
        ab.Play();
    }

    private void Start()
    {
        aas = new Dictionary<int, AudioSource>();
        sl_Battle.value = Battle_Volume;
        sl_music.value = Music_Volume;
    }

    // Update is called once per frame
    void Update()
    {
        Battle_Volume = sl_Battle.value;
        Music_Volume = sl_music.value;
        vol.SelectSingleNode("battle").InnerText = sl_Battle.value.ToString();
        vol.SelectSingleNode("music").InnerText = sl_music.value.ToString();
        xmlDoc.Save(Application.dataPath + "/Configs/Music_volume.xml");
    }
    
    public static void Updatex()
    {
        foreach (var aa in aas.Values)
        {
            aa.volume = Battle_Volume;
        }
        if (ab == null) return;
        ab.volume = Music_Volume;
        if (Flow_path.get_flag() == 1 && p != 1)
        {
            ab.clip = (AudioClip)AB.getobj("second_music");
            ab.Play();
        }
        if (Flow_path.get_flag() == 2 && p != 2)
        {
            ab.clip = (AudioClip)AB.getobj("first");
            ab.Play();
        }
        if (Flow_path.get_flag() == 3 && p != 3)
        {
            ab.clip = (AudioClip)AB.getobj("second_music");
            ab.Play();
        }
        p = Flow_path.get_flag();
    }
}
