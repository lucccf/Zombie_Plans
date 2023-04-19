using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debuff : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    private Vector3 offset = new Vector3(0f, 0f, 0f);
    Vector3 screenPos;
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
