using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    // Start is called before the first frame update
    public float toward = 1f;
    private float Alivetime = 0;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Alivetime += Time.deltaTime;
        animator.SetFloat("toward", toward);
        if(Alivetime > 1f)//波的持续时间，要和player里面的动作总时长-判定命中，相协调
        {
            Destroy(gameObject);
        }

    }
}
