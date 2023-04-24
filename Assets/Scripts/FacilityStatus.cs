using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityStatus : Monster
{
    // Start is called before the first frame update
    public override void Startx()
    {
        //Debug.Log("FFFF");
        CharacterType = 5;
        SetStatus(0, 10);
        status.max_toughness = 100000000;
        status.toughness = 1000000000;
        status.max_hp = 100;
        status.hp = 100;
        HitTime = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(29, 2), new Fixpoint(29, 2), new Fixpoint(8, 1) };//击退时间，第一个为占位，其余为1段，2段，3段
        HitSpeed = new Fixpoint[4] { new Fixpoint(0, 0), new Fixpoint(9, 1), new Fixpoint(9, 1), new Fixpoint(4, 1) };//击退速度，第一个为占位
        ToughnessStatus = new int[4] { 75, 50, 25, 0 };//阶段
        audiosource = GetComponent<AudioSource>();
        //status.GetAttacked(new Fixpoint(-10, 0), 0);
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
        //Debug.Log("F" + status.toughness);
        
        if (status.hp <= 0)
        {
            Debug.Log("建筑坏了");
        }
        else {
            BasicCharacterGetHited();
        }
    }
}
