using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Button AllFacilityButton;
    public Button WorkTable;
    public GameObject AllFacilityUI;
    void Start()
    {
        AllFacilityButton.onClick.AddListener(ShowAllFacUI);
    }
    void ShowAllFacUI() {
        AllFacilityUI.SetActive(true);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
