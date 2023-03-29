using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToBeMakedItem : MonoBehaviour
{
    public Item item;
    void Start()
    {
        GetComponentInChildren<Text>().text = item.name;
        GetComponent<Image>().sprite = item.image;
    }

    public Item GetItem()
    {
        return item;
    }
    void Update()
    {
        
    }
}