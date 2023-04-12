using Net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using Google.Protobuf;


public enum BODYTYPE
{
    LoginData = 0,
    LoginResponse,
    CharacterData,
    LoginOut,
    Frame,
    GameState,
    HashString,
    UserMoney,

    //新加
    PlayerOptData,
    /* server之间通信协议 */
    ServerInfo = 100

};


public class Clisocket : MonoBehaviour
{
    public static Clisocket Instance;
    IPAddress ipAddress;// = IPAddress.Parse("127.0.0.1");
    IPEndPoint remoteEP;// = new IPEndPoint(ipAddress, 8080);
    static int HEAD_SIZE = 4;
    static int offset = 0;
    static byte[] buffer = new byte[1024];
    static byte[] sendbuffer = new byte[1024];
    static Socket clientSocket;
    static Dog timer;
    static Cat rectimer;
    public struct op
    {
        public KeyCode key;
        public int opt;

        public op(KeyCode key, int opt)
        {
            this.key = key;
            this.opt = opt;
        }
    }

    // Start is called before the first frame update
    public static Dictionary<PlayerOpt, op> keys = new Dictionary<PlayerOpt, op>();
    void Awake()
    {
        GetSettings();
        StartSocket();
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        timer = new Dog();
        rectimer = new Cat();
    }

    public static void Headercreate(int len, BODYTYPE type)
    {
        for (int i = 0; i < 3; i++)
        {
            sendbuffer[i] = (byte)(len & 255);
            len = len >> 8;
        }
        sendbuffer[3] = (byte)type;
    }

    void StartSocket()
    {
        DontDestroyOnLoad(gameObject);
        ipAddress = IPAddress.Parse("10.0.150.31");
        remoteEP = new IPEndPoint(ipAddress, 8089);

        // 创建一个 TCP 套接字并连接到远程主机
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(remoteEP);
        clientSocket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, ReceiveCallback, null);
    }

    private void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }

    public static void Sendmessage(BODYTYPE bodyType, IMessage message)
    {
        int len;
        byte[] buf = message.ToByteArray();
        len = buf.Length;
        Headercreate(len, bodyType);
        for (int i = 0; i < len; i++)
        {
            sendbuffer[i + 4] = buf[i];
        }
        clientSocket.Send(sendbuffer, len + 4, 0);
    }

     
    void GetSettings()
    {
        //读取配置文件
        keys[PlayerOpt.ADown] = new op(KeyCode.A, 1);
        keys[PlayerOpt.AUp] = new op(KeyCode.A, 2);
        keys[PlayerOpt.DDown] = new op(KeyCode.D, 1);
        keys[PlayerOpt.DUp] = new op(KeyCode.D, 2);
        keys[PlayerOpt.JDown] = new op(KeyCode.J, 1);
        keys[PlayerOpt.JUp] = new op(KeyCode.J, 2);
        keys[PlayerOpt.KDown] = new op(KeyCode.K, 1);
        keys[PlayerOpt.KUp] = new op(KeyCode.K, 2);
        keys[PlayerOpt.LDown] = new op(KeyCode.L, 1);
        keys[PlayerOpt.LUp] = new op(KeyCode.L, 2);
        keys[PlayerOpt.QDown] = new op(KeyCode.Q, 1);
        keys[PlayerOpt.QUp] = new op(KeyCode.Q, 2);
        keys[PlayerOpt.EDown] = new op(KeyCode.E, 1);
        keys[PlayerOpt.EUp] = new op(KeyCode.E, 2);
        keys[PlayerOpt.ShiftDown] = new op(KeyCode.LeftShift, 1);
        keys[PlayerOpt.ShiftUp] = new op(KeyCode.LeftShift, 2);
        keys[PlayerOpt.SpaceDown] = new op(KeyCode.Space, 1);
        keys[PlayerOpt.SpaceUp] = new op(KeyCode.Space, 2);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static bool MyGetKey(op p)
    {
        switch (p.opt)
        {
            case 1:
                return Input.GetKeyDown(p.key);
            case 2:
                return Input.GetKeyUp(p.key);
        }
        return false;
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        Cat.elapsedTime = 0.0f;
        //Header respHeader = new Header();
        try
        {
            int bytesReceive = clientSocket.EndReceive(ar);
            
            if (bytesReceive > 0)
            {
                if (Dog.ka) {
                    Debug.Log("offset:" + offset);
                    Debug.Log("Cat.ka" + Cat.ka);
                }
                offset += bytesReceive;

                // 处理包体
                while (offset >= HEAD_SIZE)
                {
                    BODYTYPE bodyType = BODYTYPE.Frame;
                    byte[] head = GetResponseHeader(buffer, HEAD_SIZE);
                    byte[] body_len = new byte[3];
                    byte[] body_type = new byte[1];
                    for (int i = 0; i < 3; i++)
                    {
                        body_len[i] = head[i];
                    }
                    for (int i = 3; i < 4; i++)
                    {
                        body_type[i - 3] = head[i];
                    }

                    int bodyLen = (body_len[2] << 16) | (body_len[1] << 8) | body_len[0];
                    bodyType = (BODYTYPE)body_type[0];

                    if (offset < bodyLen + HEAD_SIZE) break;

                    // 当前包的包体
                    byte[] body = GetResponseBody(buffer, bodyLen);

                    if (bodyType == BODYTYPE.Frame)
                    {
                        Dog.elapsedTime = 0.0f;
                        Frame opts = Frame.Parser.ParseFrom(body);
                        if (opts.Index == -1)
                        {
                            Loading_ctrl.Frames.Enqueue(opts);
                        }
                        else {
                            //Debug.Log(opts);
                            Main_ctrl.Frames.Enqueue(opts);
                        }
                        
                        //Main_ctrl.Frame_update(opts);
                        //获取frame
                    }

                    if (bodyType == BODYTYPE.LoginResponse)
                    {
                        LoginResponse loginresponse = LoginResponse.Parser.ParseFrom(body);

                        LoginController.q.Enqueue(loginresponse);
                        //LoginController.Trans(loginresponse);
                        //获取frame

                        Debug.Log(loginresponse.Msg);
                    }

                    int packageLen = bodyLen + HEAD_SIZE;

                    for (int i = 0; i < offset - packageLen; i++)
                    {
                        buffer[i] = buffer[i + packageLen];
                    }

                    offset -= packageLen;
                }
            }
            clientSocket.BeginReceive(buffer, offset, buffer.Length - offset, SocketFlags.None, ReceiveCallback, null);
        }
        catch (SocketException ex)
        {
            Console.WriteLine("接收消息出错，错误信息：{0}", ex.Message);
        }
    }

    public static byte[] GetResponseHeader(byte[] buf, int recvLen)
    {
        if (recvLen >= HEAD_SIZE)
        {
            byte[] head = new byte[HEAD_SIZE];
            for (int i = 0; i < HEAD_SIZE; i++)
            {
                head[i] = buf[i];
            }

            return head;
        }

        return null;
    }

    public static byte[] GetResponseBody(byte[] buf, int recvLen)
    {
        if (recvLen > 0)
        {
            byte[] body = new byte[recvLen];

            for (int i = 0; i < recvLen; i++)
            {
                body[i] = buf[i + HEAD_SIZE];
            }

            return body;
        }
        return null;
    }
}
