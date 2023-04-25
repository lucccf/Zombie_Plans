using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FacilityStatus : Monster
{
    // Start is called before the first frame update
    public Facility fac;
    public GameObject hp;
    public int curhp;
    public int curnum;
    public override void Startx()
    {
        //Debug.Log("FFFF");
        CharacterType = 5;
        SetStatus(0, 10);
        status.max_toughness = 100000000;
        status.toughness = 1000000000;
        foreach (var m in fac.materials)
        {
            status.max_hp = 100 * m.Value;
            status.hp = 100 * m.Value;
            curhp = status.hp;
            curnum = fac.commited[m.Key];
            Debug.Log("FacBar:" + 100 * m.Value);
        }
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(29, 2), new Fixpoint(8, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(9, 1), new Fixpoint(9, 1), new Fixpoint(4, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
        audiosource = GetComponent<AudioSource>();
        hp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //HitTime = new Fixpoint[3] { new Fixpoint(100, 0), new Fixpoint(100, 0), new Fixpoint(200, 0) };
        //HitSpeed = new Fixpoint[3] { new Fixpoint(5, 1), new Fixpoint(5, 1), new Fixpoint(5, 1) };
        //ToughnessStatus = new int[3] { 50, 25, 0 };
    }

    public override void Updatex()
    {
        if (!hp.activeSelf && fac.repaired)
        {
            hp.SetActive(true);
        }

        if (status.hp <= 0)
        {
            status.hp = 0;
            Debug.Log("建筑坏了");
        }
        if (fac.repaired)
        {
            BasicCharacterGetHited();
            for (int i = 0; i < fac.commited.Count; i++)
            {
                var key = fac.commited.Keys.ElementAt(i);
                if ((curhp - status.hp) / 100 >= 1)
                {
                    fac.commited[key] -= 1;
                    curhp = fac.commited[key] * 100;
                    curnum = fac.commited[key];
                }
                if (curnum != fac.commited[key])
                {
                    status.hp = fac.commited[key] * 100;
                    curhp = status.hp;
                    curnum = fac.commited[key];
                }
                //fac.commited[key] = status.hp / 100;    
                if ((fac.commited[key] * 100 / fac.materials[key]) < 70)
                {
                    fac.buff = false;
                }
                else
                {
                    fac.buff = true;
                }
            }
        }
    }
}
