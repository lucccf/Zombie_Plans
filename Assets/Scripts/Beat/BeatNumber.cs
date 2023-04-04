using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatNumber : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(2, 2, 1);
        transform.Translate(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f));
    }
    private float AilveTime = 0f;
    Sprite GetSprite(int n)
    {
        Texture2D f = (Texture2D)Resources.Load("number/" + n.ToString());
        Sprite g = Sprite.Create(f, new Rect(0, 0, f.width, f.height), Vector2.zero);
        return g;
    }
    public void ChangeNumber(int num)
    {
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
        if(AilveTime > 0.7f)
        {
            Destroy(gameObject);
        }
    }
}
