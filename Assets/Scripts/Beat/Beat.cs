using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float AilveTime = 0;
    // Update is called once per frame
    void Update()
    {
        AilveTime += Time.deltaTime;
        if(AilveTime > 0.133)
        {
            Destroy(gameObject);
        }
    }
}
