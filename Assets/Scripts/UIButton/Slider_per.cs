using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_per : MonoBehaviour
{
    public Slider s;
    public Text t;
    void Update()
    {
        float x = s.value;
        t.text = (int)(x * 100) + "%";
    }
}
