using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBoxInMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BoxUI;
    public GameObject ButtonUI;

    public Dictionary<int, int> TestItem = new Dictionary<int, int>();
    void Start()
    {
        TestItem.Add(1, 2);
        TestItem.Add(2, 4);
        TestItem.Add(3, 8);
        TestItem.Add(4, 1);
        TestItem.Add(5, 2);
        TestItem.Add(6, 3);
        TestItem.Add(7, 4);
        TestItem.Add(8, 1);
        TestItem.Add(9, 9);
        BoxUI.GetComponent<WolfBox>().GetItem(TestItem);
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
