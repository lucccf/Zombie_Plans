using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject HP_Bar;
    public Text txt;
    private Image HP_Image;
    private float Cur_HP = 100;
    float HP_Percent;
    float Max_HP = 100;
    PlayerStatus ps;
    // Start is called before the first frame update
    public void Startx()
    {
        //在游戏开始的时候获取Image组件和最大HP
        HP_Bar = GameObject.Find("Heart_full");
        HP_Image = HP_Bar.GetComponent<Image>();
        ps = ((Player)Main_ctrl.All_objs[Main_ctrl.Ser_to_cli[Main_ctrl.user_id]].modules[Object_ctrl.class_name.Player]).status;
        Max_HP = ps.max_hp;
        Cur_HP = ps.hp;
    }

    // Update is called once per frame
    void Update()
    {
        Cur_HP = ps.hp;
        HP_Percent = Cur_HP / Max_HP;
        HP_Image.fillAmount = HP_Percent;
        txt.text = Cur_HP + "/" + Max_HP;
    }
}
