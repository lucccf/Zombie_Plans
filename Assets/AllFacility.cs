using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllFacility : MonoBehaviour
{
    public Button closebutton;
    public GameObject tinymap;
    public Transform[] tinybuttons;
    // Start is called before the first frame update
    void Start()
    {
        closebutton.onClick.AddListener(CloseUI);
        tinybuttons = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform onebuttons in tinybuttons) {
            Button tinyButton = onebuttons.gameObject.GetComponent<Button>();
            if (tinyButton != null)
            {
                tinyButton.onClick.AddListener(() => HandleTinyButton(onebuttons.gameObject));
            }
        }
    }
    void CloseUI() {
        gameObject.SetActive(false);
    }
    void HandleTinyButton(GameObject tinybutton) {
        foreach ( KeyValuePair<long,Facility> tmp in Flow_path.facilities) {
            Debug.Log("Dicid:" + tmp.Key + "  Fac:" + tmp.Value);
        }
        Facility fa = Flow_path.facilities[tinybutton.GetComponent<Tinyfacilitybutton>().facilityid];
        //Debug.Log(fa.materials);
    }
}
