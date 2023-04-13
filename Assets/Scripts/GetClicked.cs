using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClicked : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 从屏幕坐标生成一条射线
            if (Physics.Raycast(ray, out RaycastHit hit)) // 判断射线是否碰撞到物体
            {
                Vector3 worldPos = hit.point; // 获取射线碰撞点的世界坐标
                Debug.Log("点击位置的世界坐标为：" + worldPos);
            }
        }
    }
}
