using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2 : MonoBehaviour
{
    // Start is called before the first frame update
    float ailvetime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ailvetime += Time.deltaTime;
        if(ailvetime > 0.92f)
        {
            Destroy(gameObject);
        }
    }
}
