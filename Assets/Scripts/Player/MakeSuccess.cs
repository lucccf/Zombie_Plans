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
    public int Type = 0;
    private Text text;
    void Start()
    {
        image = GetComponent<Image>();
        text = gameObject.GetComponentInChildren<Text>();
        transform.Translate(100, 0, 1);
        if(Type == 0)
        {
            Color color = image.color;
            color.r = 1;
            color.g = 0.2f;
            color.b = 0.2f;
            image.color = color;
            text.text = "材料不足";
        }
        if (Type == 1)
        {
            Color color = image.color;
            color.r = 0.2f;
            color.g = 1;
            color.b = 0.2f;
            image.color = color;
            text.text = "设施已成功修复，不再需要材料";
        }
        if (Type == 2)
        {
            Color color = image.color;
            color.r = 0.2f;
            color.g = 1;
            color.b = 0.2f;
            image.color = color;
            text.text = "设施成功修复";
        }
        if (Type == 3)
        {
            Color color = image.color;
            color.r = 0.2f;
            color.g = 1;
            color.b = 0.2f;
            image.color = color;
            text.text = "成功提交";
        }
        if(Type == 4)
        {
            Color color = image.color;
            color.r = 1;
            color.g = 0.2f;
            color.b = 0.2f;
            image.color = color;
            text.text = "已经拥有";
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
