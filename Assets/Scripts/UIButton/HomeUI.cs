using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Button AllFacilityButton;
    public Button WorkTableButton;
    public GameObject AllFacilityUI;
    public GameObject WorkTableUI;
    void Start()
    {
        AllFacilityButton.onClick.AddListener(ShowAllFacUI);
        WorkTableButton.onClick.AddListener(ShowWorkTableUI);
    }
    void ShowAllFacUI() {
        AllFacilityUI.SetActive(true);
        gameObject.SetActive(false);
    }
    void ShowWorkTableUI()
    {
        WorkTableUI.SetActive(true);
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
