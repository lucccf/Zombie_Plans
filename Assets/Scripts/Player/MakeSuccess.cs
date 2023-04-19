using UnityEngine.UI;
using UnityEngine;

public class MakeSuccess : MonoBehaviour
{
    // Start is called before the first frame update
    private float MinSize = 0.7f;
    private float MaxSize = 1;
    private float Speed = 6;
    private float MaxAilveTime = 2;
    private float AilveTime = 0;
    Image image;
    public bool Type = false;
    private Text text;
    void Start()
    {
        image = GetComponent<Image>();
        text = gameObject.GetComponentInChildren<Text>();
        transform.Translate(100, 0, 1);
        if(Type == false)
        {
            Color color = image.color;
            color.r = 1;
            color.g = 0.2f;
            color.b = 0.2f;
            image.color = color;
            text.text = "材料不足";
        }
    }

    void Update()
    {
        AilveTime += Time.deltaTime;
        if(AilveTime > MaxAilveTime)
        {
            Destroy(gameObject);
        }
        transform.localScale = new Vector3(MinSize + AilveTime / MaxAilveTime * (MaxSize - MinSize), MinSize + AilveTime / MaxAilveTime * (MaxSize - MinSize), 0);
        transform.Translate(0, Speed  * Time.deltaTime, 0);
        Color color = image.color;
        color.a = (MaxAilveTime - AilveTime) / MaxAilveTime;
        image.color = color;
        color = text.color;
        color.a = (MaxAilveTime - AilveTime) / MaxAilveTime;
        text.color = color;
    }
}
