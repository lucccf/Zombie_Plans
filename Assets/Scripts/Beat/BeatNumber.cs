using System.Collections;
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
    private static float MinSize = 1f;//最小大小
    private static float MaxSize = 3f;//最大大小
    private bool changed = false;
    Sprite GetSpriteGreen(int n)
    {
        Sprite f = (Sprite)AB.getobj(n.ToString());
        return f;
    }
    Sprite GetSpriteYellow(int n)
    {
        Sprite f = (Sprite)AB.getobj(n.ToString() + n.ToString());
        return f;
    }
    Sprite GetSprite(int n)
    {
        if (changed == false) return GetSpriteYellow(n);
        else return GetSpriteGreen(n);
    }
    public void ChangeNumber(int num)
    {
        if(num < 0)
        {
            num = -num;
            changed = true;
        }
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
        if(AilveTime > MaxAliveTime)
        {
            Destroy(gameObject);
        }
    }
}
