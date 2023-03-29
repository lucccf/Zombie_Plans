using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeFailed : MonoBehaviour
{
    public GameObject CloseButton;
    void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(Click);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Click()
    {
        gameObject.SetActive(false);
    }
}
