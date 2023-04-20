using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetClicked : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particle;
    public GameObject particle_instance;
    public GameObject EffectCamera;
    public float destroyDelay = 10f;
    Camera mainCamera;
    void Start()
    {
        mainCamera = GameObject.Find("EffectCamera").GetComponent<Camera>();
        particle = (GameObject)AB.getobj("particle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左键点击
        {
            Vector3 mousePos = Input.mousePosition;


            // 将屏幕坐标转换为世界坐标
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

            particle_instance = Instantiate(particle, EffectCamera.transform);
            particle_instance.transform.position = worldPos;
            particle_instance.transform.localScale = new Vector3(1, 1, 0);
            Destroy(particle_instance, destroyDelay);
        }
    }
}

