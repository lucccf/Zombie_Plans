using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BubbleMove : MonoBehaviour
{
    public Transform target;
    private Vector3 offset = new Vector3(0f, 2f, 0f);
    Vector3 screenPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        gameObject.transform.position = screenPos;
    }
}
