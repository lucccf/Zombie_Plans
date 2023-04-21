using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2 : MonoBehaviour
{
    // Start is called before the first frame update
    float ailvetime = 0;
    protected AudioSource audiosource;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.clip = (AudioClip)AB.getobj("爆炸的声音");
        audiosource.Play();
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
