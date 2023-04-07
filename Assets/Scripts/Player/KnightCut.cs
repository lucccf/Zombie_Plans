using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCut : MonoBehaviour
{
    // Start is called before the first frame update
    private float alivetime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alivetime += Time.deltaTime;
        if(alivetime > 1.15f)
        {
            Destroy(gameObject);
        }
    }
}
