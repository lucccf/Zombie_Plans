using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public int curprogress = 0;
    public int endprogress = 0;
    void Start()
    {
        curprogress = 0;
        endprogress = 0;
    }

    // Update is called once per frame
    void SetProgress(int cur) {
        endprogress = cur;
    }
    void Update()
    {
       
   
    }
}
