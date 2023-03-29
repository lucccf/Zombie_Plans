using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public GameObject bag;
    private Bag BagScript;
    public Item item;
    public int number;
    void Start()
    {
        BagScript = bag.GetComponent<Bag>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            Debug.Log("Item was geted");
            BagScript.GetItem(item.id, number);
            Destroy(gameObject);
        }
    }
}
