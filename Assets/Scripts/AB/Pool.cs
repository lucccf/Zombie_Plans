using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    static Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    public static GameObject getobj(string name)
    {
        if (pool.ContainsKey(name))
        {
            if (pool[name].Count > 0)
            {
                GameObject p = pool[name].Dequeue();
                p.SetActive(true);
                return p;
            }
            else
            {
                return Instantiate((GameObject)AB.getobj(name));
            }
        }
        else
        {
            pool[name] = new Queue<GameObject>();
            return Instantiate((GameObject)AB.getobj(name));
        }
    }

    public static void desobj(GameObject obj)
    {
        obj.SetActive(false);
        pool[obj.name].Enqueue(obj);
    }

    public static void crepool(string name, int cnt)
    {
        if (!pool.ContainsKey(name))
        {
            pool[name] = new Queue<GameObject>();
        }
        GameObject p = (GameObject)AB.getobj(name);
        for(int i = 0; i < cnt; i++)
        {
            GameObject q = Instantiate(p);
            q.SetActive(false);
            pool[name].Enqueue(q);
        }
    }
}
