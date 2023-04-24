using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Debuff : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem particle;
    void Start()
    {
        particle = gameObject.GetComponent<ParticleSystem>();
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var m in Flow_path.facilities) {
            if (m.Value.repaired) {
                particle.Play();
                Invoke("StopParticleSystem", 3f);
            }
        }
    }

    void StopParticleSystem()
    {
        particle.Stop();
    }
}

