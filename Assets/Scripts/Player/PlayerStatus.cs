using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus
{
    public int max_hp;
    public int hp;
    public int attack;
    public int toughness;
    public int max_toughness;
    public Fixpoint WalkSpeed;
    public Fixpoint RunSpeed;
    public Fixpoint attack_rate;
    public Fixpoint defence_rate;
    public int tag;
    public int player_id;
    public int player_id2;
    public bool death = false;
    public bool breaked = false;

    public Fixpoint TmpToughness;

    public int last_damage;
    
    public PlayerStatus(int max_hp,int attack,int max_toughness,int walk_speed,int run_speed,
        Fixpoint attack_rate,Fixpoint defence_rate)
    {
        this.max_hp = max_hp;
        this.hp = max_hp;
        this.attack = attack;
        this.max_toughness = max_toughness;
        this.toughness = max_toughness;
        this.WalkSpeed = new Fixpoint(walk_speed);
        this.RunSpeed = new Fixpoint(run_speed);
        this.attack_rate = attack_rate;
        this.defence_rate = new Fixpoint(1,0) - defence_rate;
        this.TmpToughness = new Fixpoint(0, 0);
    }
    public PlayerStatus(int max_hp,int attack)
    {
        this.hp = max_hp;
        this.max_hp = max_hp;
        this.attack = attack;
        this.max_toughness = 100;
        this.toughness = 100;
        this.WalkSpeed = new Fixpoint(5,0);
        this.RunSpeed = new Fixpoint(10,0);
        this.attack_rate = new Fixpoint(1,0);
        this.defence_rate = new Fixpoint(1,0);
        this.TmpToughness = new Fixpoint(0, 0);
    }
    public Fixpoint Damage()
    {
        return new Fixpoint(attack,0) * attack_rate;
    }
    public void RecoverHp(int x)
    {
        hp += x;
        if(hp > max_hp)
        {
            hp = max_hp;
        } 
    }
    public void RecoverToughness(Fixpoint x)
    {
        TmpToughness += x;
        
        if(TmpToughness.to_int() > 0)
        {
            toughness += TmpToughness.to_int();
            TmpToughness -= new Fixpoint(TmpToughness.to_int(),0);
            if (toughness > max_toughness)
            {
                toughness = max_toughness;
            }
        }
    }
    public void GetAttacked(Fixpoint damage, int toughness_damage)
    {
        Fixpoint real_damage = damage * defence_rate + new Fixpoint(1,3);
        hp -= real_damage.to_int();
        toughness -= toughness_damage;
        last_damage = real_damage.to_int();
        if(hp <= 0)
        {
            death = true;
        }
    }
    public int GetToughness()
    {
        return toughness;
    } 
}
