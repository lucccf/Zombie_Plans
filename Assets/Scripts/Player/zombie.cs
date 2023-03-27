using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombie : MonoBehaviour
{
    //怪物状态
    private MonsterStatus status;

    //碰撞体变量
    private Fix_col2d f;
    public SpriteRenderer spriteRenderer;

    //动画变量
    private Animator animator;
    private float speed = 0f;
    private float toward = 1f;
    private bool attack = false;
    private bool death = false;

    private int hp = 50;
    void Start()
    {
        animator = GetComponent<Animator>();
        status = new MonsterStatus(50, 100);

        Vector2 spriteSize = spriteRenderer.bounds.size;

        // 在控制台输出精灵的长和宽
        Debug.Log("Sprite width: " + spriteSize.x);
        Debug.Log("Sprite height: " + spriteSize.y);
        //f = GetComponent<Fix_col2d>();
        //f = new Fix_col2d(new Fix_vector2(transform.position), new Fixpoint(spriteSize.y), new Fixpoint(spriteSize.x), 2, Fix_col2d.col_status.Collider);
        Collider_ctrl.cols.Add(f);
    }

    // Update is called once per frame
    //PlayerStatus tmp = new PlayerStatus(100, 10);
    void Update()
    {
        if (f.actions.Count != 0)
        {
            f.actions.Clear();
            hp -= 10;
            if(hp <= 0)
            {
                death = true;
            }
        }
        animator.SetBool("death", death);
    }
}
