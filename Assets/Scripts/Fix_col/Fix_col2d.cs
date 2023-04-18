using System.Collections.Generic;

public class Fix_col2d
{
    /*
    public Fixpoint px;
    public Fixpoint py;
    */
    public Fix_vector2 pos;
    public Fixpoint hei;
    public Fixpoint wid;

    public long id;

    public bool onground = false;

    public enum col_status
    {
        Wall,
        Trigger,
        Trigger2,
        Collider,
        Attack,
        Attack2,
        //...
    }

    public col_status type = col_status.Trigger;

    public Queue<Fix_col2d_act> actions = new Queue<Fix_col2d_act>();

    public Dictionary<long, int> conditions = new Dictionary<long, int>();

    public Fix_col2d(Fixpoint px, Fixpoint py, Fixpoint hei, Fixpoint wid, long id, col_status type)
    {
        this.pos = new Fix_vector2(px.Clone(), py.Clone());
        this.hei = hei.Clone();
        this.wid = wid.Clone();
        this.id = id;
        this.type = type;
    }
    
    public Fix_col2d(Fix_vector2 pos, Fixpoint hei, Fixpoint wid, long id, col_status type)
    {
        this.pos = pos;
        this.hei = hei.Clone();
        this.wid = wid.Clone();
        this.id = id;
        this.type = type;
    }

    public Fix_col2d(Fixpoint px, Fixpoint py, Fixpoint hei, Fixpoint wid, long id)
    {
        this.pos = new Fix_vector2(px.Clone(), py.Clone());
        this.hei = hei.Clone();
        this.wid = wid.Clone();
        this.id = id;
    }
    
    public Fix_col2d(Fix_vector2 pos, Fixpoint hei, Fixpoint wid, long id)
    {
        this.pos = pos;
        this.hei = hei.Clone();
        this.wid = wid.Clone();
        this.id = id;
    }

    public Fix_col2d(long px, long py, long hei, long wid, long id, col_status type)
    {
        this.pos = new Fix_vector2(px, py);
        this.hei = new Fixpoint(hei);
        this.wid = new Fixpoint(wid);
        this.id = id;
        this.type = type;
    }

    public Fix_col2d(long px, long py, long hei, long wid, long id)
    {
        this.pos = new Fix_vector2(px, py);
        this.hei = new Fixpoint(hei);
        this.wid = new Fixpoint(wid);
        this.id = id;
    }

    public void Set_value(Fix_vector2 pos, Fixpoint hei, Fixpoint wid, long id, col_status type)
    {
        this.pos = pos;
        this.hei = hei.Clone();
        this.wid = wid.Clone();
        this.id = id;
        this.type = type;
    }

    public Fixpoint left()
    {
        return pos.x - (wid >> 1);
    }

    public Fixpoint right()
    {
        return pos.x + (wid >> 1);
    }

    public Fixpoint up()
    {
        return pos.y + (hei >> 1);
    }

    public Fixpoint down()
    {
        return pos.y - (hei >> 1);
    }
}

public class Fix_col2d_act
{
    //碰撞事件
    public enum col_action
    {
        Attack,
        Trigger_in,
        Trigger_out,
        On_wall,
        //...
    }

    public col_action type;
    public Fix_col2d opsite;

    public Fix_col2d_act(col_action type, Fix_col2d opsite)
    {
        this.type = type;
        this.opsite = opsite;
    }
}