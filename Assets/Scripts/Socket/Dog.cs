using UnityEngine;

public class Dog : MonoBehaviour
{
    public float timeLimit = 1.0f; // 超时时间
    public static float elapsedTime = 0.0f; // 已经过去的时间
    public static bool ka = false;
    private bool isRunning = true; // 计时器是否在运行

    // 计时器归零时执行的操作
    public static void ResetTimer()
    {
        elapsedTime = 0.0f;
    }

    // 开始计时
    public void StartTimer()
    {
        isRunning = true;
    }

    // 停止计时
    public void StopTimer()
    {
        isRunning = false;
        elapsedTime = 0.0f;
    }

    // 在每一帧更新计时器
    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeLimit)
            {
                ka = true;
                Debug.Log("计时器超时");
                elapsedTime = 0.0f;
            }
        }
    }
}
