using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Close : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject board;
    public Button but;

    void Start()
    {
        but.onClick.AddListener(close);
    }

    void close()
    {
        board.SetActive(false);
    }
}
