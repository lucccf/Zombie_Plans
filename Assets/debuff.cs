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
        t = Time.time;
        debuffparticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetKeyDown(KeyCode.Space)) {
            debuffparticle.Play(); 
        }*/
    }
}
