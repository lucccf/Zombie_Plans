using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Assets : MonoBehaviour
{
    static int q = 0;

    // Start is called before the first frame update
    void Start()
    {
        AB.LoadALL();
    }

    // Update is called once per frame
    void Update()
    {
        switch(q)
        {
            case 0:
                if (AB.LoadAllbundles())
                {
                    q = 1;
                }
                break;
            case 1:
                AB.LoadAllobjs();
                q = 2;
                break;
            case 2:
                if (AB.GetAllobjs())
                {
                    q = 3;
                }
                break;
            case 3:
                AB.FetchAllobjs();
                SceneManager.LoadScene("Login");
                q = 4;
                break;
        }
    }
}
