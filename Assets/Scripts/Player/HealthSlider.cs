﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthSlider;
    public Transform target;
    public Monster monster;
    private Vector3 offset = new Vector3(0f, 1f, 0f);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        healthSlider.transform.position = screenPos;
        healthSlider.value = monster.CheckHealth();
    }
}
