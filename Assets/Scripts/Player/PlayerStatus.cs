public class PlayerStatus
{
    public int hp;
    public int attack;
    public int toughness;
    public int max_toughness;
    public Fixpoint walk_speed;
    public Fixpoint run_speed;
    public Fixpoint attack_speed;
    public Fixpoint attack_rate;
    public Fixpoint defence_rate;
    public int tag;
    public int player_id;
    public int player_id2;
    public bool death = false;
    public bool breaked = false;

    
    public PlayerStatus(int hp,int attack,int max_toughness,float walk_speed,float run_speed,float attack_speed,
        float attack_rate,float defence_rate,int tag,int player_id,int player_id2)
    {
        this.hp = hp;
        this.attack = attack;
        this.max_toughness = max_toughness;
        this.toughness = max_toughness;
        this.walk_speed = new Fixpoint(walk_speed);
        this.run_speed = new Fixpoint(run_speed);
        this.attack_speed = new Fixpoint(attack_speed);
        this.attack_rate = new Fixpoint(attack_rate);
        this.defence_rate = new Fixpoint(defence_rate);
        this.tag = tag;
        this.player_id = player_id;
        this.player_id2 = player_id2;
    }
    public PlayerStatus(int hp,int attack)
    {
        this.hp = hp;
        this.attack = attack;
        this.max_toughness = 100;
        this.toughness = 100;
        this.walk_speed = new Fixpoint(5f);
        this.run_speed = new Fixpoint(10f);
        this.attack_rate = new Fixpoint(1f);
        this.defence_rate = new Fixpoint(1f);
        this.attack_speed = new Fixpoint(1f);
        this.tag = 0;
        this.player_id = 0;
        this.player_id2 = 0;
    }
    public Fixpoint Damage()
    {
        Fixpoint tmp = new Fixpoint(attack);
        return tmp * attack_rate;
    }
    public float GetWalkSpeed()
    {
        return walk_speed.to_float();
    }
    public float GetRunSpeed()
    {
        return run_speed.to_float();
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
    public void GetAttacked(ref PlayerStatus x , int toughness_damage)
    {
        Fixpoint tmp = x.Damage() * defence_rate;
        int damage = tmp.to_int();
        hp -= damage;
        toughness -= toughness_damage;
        if(hp <= 0)
        {
            death = true;
        }
        if(toughness <= 0)
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
