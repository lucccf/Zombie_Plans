using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllFacility : MonoBehaviour
{
    public Button closebutton;
    public GameObject tinymap;
    public Transform[] tinybuttons;
    public GameObject factitile;
    public GameObject matimage;
    public GameObject mattext;
    public GameObject facprogress;
    public GameObject progresstext;
    public Sprite init;
    // Start is called before the first frame update
    void Start()
    {
        closebutton.onClick.AddListener(CloseUI);
        tinybuttons = tinymap.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform onebuttons in tinybuttons) {
            Button tinyButton = onebuttons.gameObject.GetComponent<Button>();
            if (tinyButton != null)
            {
                tinyButton.onClick.AddListener(() => HandleTinyButton(onebuttons.gameObject));
            }
        }
        init = matimage.GetComponent<Image>().sprite;
    }
    void CloseUI() {
        factitile.GetComponent<Text>().text = "查看设施详情";
        gameObject.SetActive(false);
    }
    void HandleTinyButton(GameObject tinybutton) {
        foreach ( KeyValuePair<long,Facility> tmp in Flow_path.facilities) {
            Debug.Log("Dicid:" + tmp.Key + "  Fac:" + tmp.Value);
        }
        Facility fa = Flow_path.facilities[tinybutton.GetComponent<Tinyfacilitybutton>().facilityid];
        //Debug.Log(fa.materials);
        factitile.GetComponent<Text>().text = tinybutton.GetComponent<Tinyfacilitybutton>().facilityid+"号设施";
        Dictionary<int, int> curmat = fa.materials;
        foreach (KeyValuePair<int, int> mat in curmat)
        {
            Item x = Main_ctrl.GetItemById(mat.Key);
            matimage.GetComponent<Image>().sprite = x.image;
            mattext.GetComponent<Text>().text = "还需数量：" + (mat.Value - fa.commited[mat.Key]);
        }
    }
}
