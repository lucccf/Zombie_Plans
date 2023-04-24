using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacEffect : MonoBehaviour
{
    public GameObject repairedeffect;
    public GameObject buffeffect;
    ParticleSystem repairedpac;
    ParticleSystem buffpac;
    public Facility fac;
    // Start is called before the first frame update
    void Start()
    {
        repairedpac = repairedeffect.GetComponent<ParticleSystem>();
        buffpac = buffeffect.GetComponent<ParticleSystem>();
        repairedpac.Stop();
        buffpac.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (fac.buff)
        {
            if (!buffpac.isPlaying) buffpac.Play();
            if (!repairedpac.isPlaying) repairedpac.Play();
        }
        else 
        {
            buffpac.Stop();
            repairedpac.Stop();
        }
    }
}
