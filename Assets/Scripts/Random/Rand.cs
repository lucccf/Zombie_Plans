using UnityEngine;

public class Rand : MonoBehaviour
{
    public static ulong seed = 114514;
    private static ulong nw = 114514;

    public static void Setseed(ulong s)
    {
        Debug.Log("setseed" + s);
        seed = s;
        nw = s;
    }

    public static ulong rand()
    {
        nw ^= nw << 13;
        nw ^= nw >> 7;
        nw ^= nw << 17;
        return nw;
    }
}
