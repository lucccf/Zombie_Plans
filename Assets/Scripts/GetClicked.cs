using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GetClicked : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particle;
    public GameObject particle_instance;
    public GameObject parents;
    public float destroyDelay = 10f;
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
        particle = (GameObject)AB.getobj("particle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            UnityEngine.Vector3 mousePos = Input.mousePosition;


            // 将屏幕坐标转换为世界坐标
            UnityEngine.Vector3 worldPos = mainCamera.ScreenToWorldPoint(new UnityEngine.Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

            // 打印世界坐标
            Debug.Log("鼠标点击位置的世界坐标为：" + worldPos);
            particle_instance = Instantiate(particle, parents.transform);
            particle_instance.transform.position = new UnityEngine.Vector3(worldPos.x,worldPos.y,20);
            Destroy(particle_instance, destroyDelay);
        }
    }
}

