using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatNumber : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start");
        transform.localScale = new Vector3(MinSize, MinSize, 1);
        transform.Translate(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f));
        Debug.Log(MemoryNum);
    }
    private float AilveTime = 0f;
    private static float MaxAliveTime = 0.7f;
    private static float ChangeColourTime = 0.35f;
    private static float MinSize = 2f;
    private static float MaxSize = 3f;
    private int MemoryNum = 5;
    private bool changed = false;
    Sprite GetSpriteWrite(int n)
    {
        Texture2D f = (Texture2D)Resources.Load("number/" + n.ToString());
        Sprite g = Sprite.Create(f, new Rect(0, 0, f.width, f.height), Vector2.zero);
        return g;
    }
    Sprite GetSpriteYellow(int n)
    {
        Texture2D f = (Texture2D)Resources.Load("number/" + n.ToString() + n.ToString());
        Sprite g = Sprite.Create(f, new Rect(0, 0, f.width, f.height), Vector2.zero);
        return g;
    }
    Sprite GetSprite(int n)
    {
        if (AilveTime > ChangeColourTime) return GetSpriteYellow(n);
        else return GetSpriteWrite(n);
    }
    public void ChangeNumber(int num)
    {
        MemoryNum = num;
        Debug.Log("MN:" + MemoryNum);
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
        Debug.Log(MemoryNum);
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
