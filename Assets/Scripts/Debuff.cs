using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Debuff : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem particle;
    public Monster mos;
    bool showbuf = false;
    
    void Start()
    {

        particle = gameObject.GetComponent<ParticleSystem>();
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (mos.CharacterType == 2)
        {
            foreach (var m in Flow_path.facilities)
            {
                if (m.Value.buff)
                {
                    showbuf = true;
                    break;
                }
            }
        }
        if (showbuf) {
            particle.Play();
            showbuf= false;
        }
    }

    void StopParticleSystem()
    {
        particle.Stop();
    }
}

