using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour
{
    public int buttonCount;

    // Button的Prefab
    GameObject buttonPrefab;

    // 控制Button的GameObject
    public GameObject controlButton;

    // 创建Button的父对象
    public GameObject buttonParent;

    private void Start()
    {
        // 找到控制Button的组件
        Button controlButtonComponent = controlButton.GetComponent<Button>();
        buttonPrefab = (GameObject)AB.getobj("PlayerButton");
        // 给控制Button添加OnClick事件
        controlButtonComponent.onClick.AddListener(ToggleUI);
    }

    // 根据参数创建Button
    public void CreateButtons()
    {
        buttonCount = Player_ctrl.plays.Count;
        // 先清空所有已经存在的Button
        ClearButtons();
        // 创建Button
        for (int i = 0; i < buttonCount; i++)
        {
            // 创建Button对象
            GameObject buttonObject = Instantiate(buttonPrefab, buttonParent.transform);

            // 设置Button的名称
            buttonObject.name = "User " + (i + 1);

            Attack_change att = buttonObject.GetComponent<Attack_change>();
            att.sf_id = Main_ctrl.Ser_to_cli[Main_ctrl.user_id];
            att.pl_id = i;
            att.op_id = (int)Player_ctrl.plays[i].id;

            buttonObject.transform.localPosition = new Vector3(150 * i + 140, 0, 0);
        }
    }

    // 清空已经存在的Button
    private void ClearButtons()
    {
        for (int i = buttonParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonParent.transform.GetChild(i).gameObject);
        }
    }

    // 控制UI和Button的显示和隐藏
    private void ToggleUI()
    {
        /*
        bool showUI = !buttonParent.activeSelf;
        buttonParent.SetActive(showUI);
        Debug.Log("Button changed");
        if (showUI)
        {
            // 创建Button
            CreateButtons();
        }
        else
        {
            // 隐藏所有Button
            ClearButtons();
        }
        */
    }
}
