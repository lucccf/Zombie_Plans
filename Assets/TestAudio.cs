using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAudio : MonoBehaviour
{
    public AudioClip musicClip; // 指定音乐片段
    private AudioSource musicSource; // 音乐源
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Cilck);
        musicSource = GetComponent<AudioSource>(); // 获取AudioSource组件
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cilck()
    {
        musicSource.clip = musicClip; // 指定音乐片段
        musicSource.Play();
    }
}
