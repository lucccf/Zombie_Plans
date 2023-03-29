using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moves : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    private float speed = 5;
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0f, speed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0f, -speed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0f, 0f);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0f, 0f);
        }
    }

}
