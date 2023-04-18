using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        Debug.Log("VVVMouse Enter" + id);
    }

    // 鼠标移出按钮
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("VVVMouse Exit" + id);
    }

    // 点击按钮
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("VVVLeft" + id);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("VVVRight" + id);
            Player player = Player_ctrl.plays[0];
            player.ThrowItem(id);
            // 处理鼠标右键点击
        }
    }
}
