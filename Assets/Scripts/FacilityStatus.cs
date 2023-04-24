using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityStatus : Monster
{
    // Start is called before the first frame update
    public override void Startx()
    {
        CharacterType = 5;
        SetStatus(10, 10);
        status.max_toughness = 1000000;
        status.toughness = 1000000;
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        HitTime = new Fixpoint[3] { new Fixpoint(100, 0), new Fixpoint(100, 0), new Fixpoint(200, 0) };
        HitSpeed = new Fixpoint[3] { new Fixpoint(5, 1), new Fixpoint(5, 1), new Fixpoint(5, 1) };
        ToughnessStatus = new int[3] { 50, 25, 0 };
    }

    public override void Updatex()
    {
        BasicCharacterGetHited();
        if (status.death == true)
        {
            Debug.Log("建筑坏了");
        }
    }
}
