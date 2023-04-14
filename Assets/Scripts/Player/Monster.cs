using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BasicCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected Fixpoint GetNear()
    {
        List<Fixpoint> list = new List<Fixpoint>();
        foreach (Player i in Player_ctrl.plays)
        {
            if (i.f.pos.y - f.pos.y <= new Fixpoint(1, 0) && i.f.pos.y - f.pos.y >= new Fixpoint(-1, 0))
            {
                list.Add(i.f.pos.x);
            }
        }
        if (list.Count > 0)
        {
            Fixpoint min = new Fixpoint(114514, 0);
            Fixpoint ans = new Fixpoint(0, 0);
            foreach (Fixpoint i in list)
            {
                Fixpoint x = f.pos.x, y = i;
                if (x < y)
                {
                    Fixpoint t = x;
                    x = y;
                    y = t;
                }
                if (x - y < min)
                {
                    min = x - y;
                    ans = i;
                }
            }
            return ans;
        }
        else return new Fixpoint(10000, 0);
    }
    protected Fixpoint GetNearDistance()
    {
        Fixpoint x = GetNear();
        if (f.pos.x > x)
        {
            return f.pos.x - x;
        }
        else
        {
            return x - f.pos.x;
        }
    }
}
