using Net;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    // Start is called before the first frame update
    public static InputField inputField1;
    public static InputField inputField2;
    public static ConcurrentQueue<LoginResponse> q = new ConcurrentQueue<LoginResponse>();

    void Start()
    {
        inputField1 = GameObject.Find("name").GetComponent<InputField>();
        inputField2 = GameObject.Find("pwd").GetComponent<InputField>();
    }

    public static void LoginClicked()
    {
        string text1;
        string text2;
        text1 = inputField1.text;
        text2 = inputField2.text;
        LoginData loginData = new LoginData();
        loginData.Username = text1;
        loginData.Passwd = text2;
        loginData.Opt = LoginData.Types.Operation.Login;

        Clisocket.Sendmessage(BODYTYPE.LoginData, loginData);
    }

    public static void RegisterClicked()
    {
        string text1;
        string text2;
        text1 = inputField1.text;
        text2 = inputField2.text;
        LoginData loginData = new LoginData();
        loginData.Username = text1;
        loginData.Passwd = text2;
        loginData.Opt = LoginData.Types.Operation.Register;

        Clisocket.Sendmessage(BODYTYPE.LoginData, loginData);
    }

    public static void Trans(LoginResponse l)
    {
        if (l.Result)
        {
            Main_ctrl.user_id = l.Userid;
            SceneManager.LoadScene("Start");
        }
    }

    // Update is called once per frame
    void Update()
    {
        while(q.Count > 0)
        {
            LoginResponse x;
            if (!q.TryDequeue(out x)) break;
            Trans(x);
        }
    }
}
