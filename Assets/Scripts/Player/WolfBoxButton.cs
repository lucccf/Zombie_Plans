using UnityEngine;
using UnityEngine.UI;

public class WolfBoxButton : MonoBehaviour
{
    private Button button;
    public GameObject WolfBoxUI;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Onclick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Onclick()
    {
        WolfBoxUI.SetActive(true);
    }
}
