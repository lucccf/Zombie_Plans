using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus
{
    public int hp;
    public int attack;
    public int toughness;
    public int max_toughness;
    public Fixpoint move_speed;
    public Fixpoint attack_speed;
    public Fixpoint attack_rate;
    public Fixpoint defence_rate;
    public int monster_id;
    public int tag;
    public int tag2;
    public bool death = false;
    public bool breaked = false;

    public MonsterStatus(int hp, int attack, int max_toughness, float move_speed, float attack_speed,
        float attack_rate, float defence_rate, int tag, int tag2, int monster_id)
    {
        this.hp = hp;
        this.attack = attack;
        this.max_toughness = max_toughness;
        this.toughness = max_toughness;
        this.move_speed = new Fixpoint(move_speed);
        this.attack_speed = new Fixpoint(attack_speed);
        this.attack_rate = new Fixpoint(attack_rate);
        this.defence_rate = new Fixpoint(defence_rate);
        this.tag = tag;
        this.tag2 = tag2;
        this.monster_id = monster_id;
    }
    public MonsterStatus(int hp,int attack)
    {
        this.hp = hp;
        this.attack = attack;
        this.max_toughness = 100;
        this.toughness = max_toughness;
        this.move_speed = new Fixpoint(4f);
        this.attack_speed = new Fixpoint(1f);
        this.attack_rate = new Fixpoint(1f);
        this.defence_rate = new Fixpoint(1f);
        this.tag = 0;
        this.tag2 = 0;
        this.monster_id = 0;
    }
    public Fixpoint Damage()
    {
        Fixpoint tmp = new Fixpoint(attack);
        return tmp * attack_rate;
    }
    public float GetMoveSpeed()
    {
        return move_speed.to_float();
    }
    public float GetAttackSpeed()
    {
        return attack_speed.to_float();
    }
    public float GetAttackRate()
    {
        return attack_rate.to_float();
    }
    public float GetDefenceRate()
    {
        return defence_rate.to_float();
    }
    public void GetAttacked(ref PlayerStatus x, int toughness_damage)
    {
        Fixpoint tmp = x.Damage() * defence_rate;
        int damage = tmp.to_int();
        hp -= damage;
        toughness -= toughness_damage;
        if (hp <= 0)
        {
            death = true;
        }
        if (toughness <= 0)
        {
            toughness = max_toughness;
            breaked = true;
        }
    }
    public void GetAttacked(ref MonsterStatus x, int toughness_damage)
    {
        Fixpoint tmp = x.Damage() * defence_rate;
        int damage = tmp.to_int();
        hp -= damage;
        toughness -= toughness_damage;
        if (hp <= 0)
        {
            death = true;
        }
        if (toughness <= 0)
        {
            toughness = max_toughness;
            breaked = true;
        }
    }
}
