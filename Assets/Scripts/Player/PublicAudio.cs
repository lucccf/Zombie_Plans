using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicAudio : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audiosource;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string x)
    {
        if (name == "") return;
        audiosource.Stop();
        audiosource.clip = (AudioClip)AB.getobj(x);
        audiosource.Play();
    }
}
