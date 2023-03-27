using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject HP_Bar;
    private Image HP_Image;
    public float Cur_HP = 100;
    float HP_Percent;
    float Max_HP = 100;
    // Start is called before the first frame update
    void Start()
    {
        //在游戏开始的时候获取Image组件和最大HP
        HP_Bar = GameObject.Find("Heart_full");
        HP_Image = HP_Bar.GetComponent<Image>();
        Cur_HP = Max_HP;
    }
    void InitHealthBar(float max_hp) 
    {
        Max_HP= max_hp;
        Cur_HP= Max_HP;
    }
    // Update is called once per frame
    void Update()
    {
        HP_Percent = Cur_HP / Max_HP;
        HP_Image.fillAmount = HP_Percent;
    }

    void CurHpChange(float cur) 
    {
        Cur_HP = cur;
    }
}
