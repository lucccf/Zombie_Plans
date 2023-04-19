using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debuff : MonoBehaviour
{
    public ParticleSystem debuffparticle;
    public float t;
    void Start()
    {
        debuffparticle = gameObject.GetComponent<ParticleSystem>();

        debuffparticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (false)
        {
            debuffparticle.Play();
            t = Time.time;
        }
        if (t + 5f < Time.time) 
        {
            debuffparticle.Stop();
        }
    }
}
