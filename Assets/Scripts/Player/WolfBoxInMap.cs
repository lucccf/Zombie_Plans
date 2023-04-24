using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WolfBoxInMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BoxUI;
    public GameObject ButtonUI;
    public int BoxId;
    void Start()
    {

    }

    public void InitBoxItem(Dictionary<int,int> items)
    {
        WolfBox box = BoxUI.GetComponent<WolfBox>();
        Player_ctrl.WolfBox.Add(BoxId, box);
        box.Boxid = BoxId;
        box.GetItem(items);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerIn()
    {
        Debug.Log("WolfBoxTriggerIn");
        ButtonUI.SetActive(true);
    }

    public void Triggerout()
    {
        Debug.Log("WolfBoxTriggerOut");
        BoxUI.SetActive(false);
        ButtonUI.SetActive(false);
    }
}
