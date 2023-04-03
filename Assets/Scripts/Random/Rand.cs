using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rand : MonoBehaviour
{
    public static long seed = 114514;
    private static long nw = 114514;

    public static void Setseed(long s)
    {
        seed = s;
        nw = s;
    }

    public static long rand()
    {
        nw ^= nw << 13;
        nw ^= nw >> 7;
        nw ^= nw << 17;
        return nw;
    }
}
