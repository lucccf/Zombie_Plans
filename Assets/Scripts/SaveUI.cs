using Net;
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
        savetext.GetComponent<Text>().text = "花费:30";
        commitbutton.GetComponent<Button>().onClick.AddListener(trysave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void trysave()
    {
        //Debug.Log("塔利班id:" + talibanid);
        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.SaveTLB;
        x.Userid = (int)Main_ctrl.user_id;
        x.Itemid = talibanid;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);

    }
}
