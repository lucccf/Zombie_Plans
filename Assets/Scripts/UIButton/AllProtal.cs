using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllProtal : MonoBehaviour
{
    public Button closebutton;
    public GameObject tinymap;
    public Transform[] tinybuttons;
    public GameObject protitile;

    // Start is called before the first frame update
    void Start()
    {
        closebutton.onClick.AddListener(CloseUI);
        tinybuttons = tinymap.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform onebuttons in tinybuttons)
        {
            Button tinyButton = onebuttons.gameObject.GetComponent<Button>();
            if (tinyButton != null)
            {
                tinyButton.onClick.AddListener(() => HandleTinyButton(onebuttons.gameObject));
            }
        }
    }
    void CloseUI()
    {
        protitile.GetComponent<Text>().text = "传送";
        gameObject.SetActive(false);
    }
    void HandleTinyButton(GameObject tinybutton)
    {
        Debug.Log(tinybutton.GetComponent<TinyProtalButton>().tinyprotalid);
        PlayerOptData x = new PlayerOptData();
        x.Opt = PlayerOpt.MovePlayer;
        x.Userid = (int)Main_ctrl.user_id;
        x.Itemid = (int)tinybutton.GetComponent<TinyProtalButton>().tinyprotalid;

        Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
    }
}
