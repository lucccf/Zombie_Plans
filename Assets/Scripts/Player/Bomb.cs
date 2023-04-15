using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update
    public float toward = 0;
    float ailvetime = 0;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ailvetime += Time.deltaTime;
        if(ailvetime > 0.58f)
        {
            Destroy(gameObject);
        }
        animator.SetFloat("toward", toward);
    }
}
