using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverPerfprm : MonoBehaviour
{
    // Start is called before the first frame update
    float alivetime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alivetime += Time.deltaTime;
        if(alivetime > 0.66)
        {
            Destroy(gameObject);
        }
    }
}
