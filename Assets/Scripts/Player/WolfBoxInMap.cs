using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBoxInMap : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BoxUI;
    public GameObject ButtonUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerIn()
    {
        ButtonUI.SetActive(true);
    }

    public void Triggerout()
    {
        BoxUI.SetActive(false);
        ButtonUI.SetActive(false);
    }
}
