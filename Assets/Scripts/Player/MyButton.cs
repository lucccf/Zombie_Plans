using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Net;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button button; // 按钮组件
    public int id;
    void Start()
    {
        // 获取按钮组件
        button = GetComponent<Button>();
    }

    // 鼠标移入按钮
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("VVVMouse Enter" + id);
    }

    // 鼠标移出按钮
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("VVVMouse Exit" + id);
    }

    // 点击按钮
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerOptData x = new PlayerOptData();
            x.Opt = PlayerOpt.Useitem;
            x.Userid = (int)Main_ctrl.user_id;
            x.Itemid = id;
            Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
            //Debug.Log("VVVLeft" + id);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerOptData x = new PlayerOptData();
            x.Opt = PlayerOpt.DeleteItem;
            x.Userid = (int)Main_ctrl.user_id;
            x.Itemid = id;
            Clisocket.Sendmessage(BODYTYPE.PlayerOptData, x);
            // Debug.Log("VVVRight" + id);
            //Player player = Player_ctrl.plays[0];
            //发帧

            //player.ThrowItem(id);  //收到以后
            // 处理鼠标右键点击
        }
    }
}
