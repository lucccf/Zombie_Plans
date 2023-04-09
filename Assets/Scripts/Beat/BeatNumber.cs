﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatNumber : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(MinSize, MinSize, 1);
        transform.Translate(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f));
    }
    private float AilveTime = 0f; 
    private static float MaxAliveTime = 0.4f; //存活时间
    //private static float MaxAliveTime = 100000f; //存活时间
    private static float ChangeColourTime = 0.01f;//改变颜色时间
    private static float MinSize = 1f;//最小大小
    private static float MaxSize = 3f;//最大大小
    //private static float MinSize = 10f;//最小大小
    //private static float MaxSize = 10f;//最大大小
    private int MemoryNum = 5;
    private bool changed = false;
    Sprite GetSpriteWrite(int n)
    {
        //Debug.Log(AB.getobj(n.ToString()).GetType());
        Sprite f = (Sprite)AB.getobj(n.ToString());
        //Sprite g = Sprite.Create(f, new Rect(0, 0, f.width, f.height), Vector2.zero);
        return f;
    }
    Sprite GetSpriteYellow(int n)
    {
        Sprite f = (Sprite)AB.getobj(n.ToString() + n.ToString());
        //Sprite g = Sprite.Create(f, new Rect(0, 0, f.width, f.height), Vector2.zero);
        return f;
    }
    Sprite GetSprite(int n)
    {
        if (AilveTime > ChangeColourTime) return GetSpriteYellow(n);
        else return GetSpriteWrite(n);
    }
    public void ChangeNumber(int num)
    {
        MemoryNum = num;
        SpriteRenderer[] children = gameObject.GetComponentsInChildren<SpriteRenderer>();
        int x = num % 10;
        num /= 10;
        int y = num % 10;
        num /= 10;
        int z = num % 10;
        if(children.Length != 3)
        {
            Debug.Log("error");
        }
        //Debug.Log(x.ToString() + " " + y.ToString() + " " + z.ToString());
        if (z != 0)
        {
            children[0].sprite = GetSprite(z);
        }
        else children[0].sprite = null;
        if (y != 0 || z != 0)
        {
            children[1].sprite = GetSprite(y);
        }
        else children[1].sprite = null;
        
        children[2].sprite = GetSprite(x);
    }
    void Update()
    {
        AilveTime += Time.deltaTime;
        float size = MinSize + (AilveTime / MaxAliveTime) * (MaxSize - MinSize);
        transform.localScale = new Vector3(size, size, 0f);
        if(AilveTime > ChangeColourTime && changed == false)
        {
            changed = true;
            ChangeNumber(MemoryNum);
        }
        if(AilveTime > MaxAliveTime)
        {
            Destroy(gameObject);
        }
    }
}
