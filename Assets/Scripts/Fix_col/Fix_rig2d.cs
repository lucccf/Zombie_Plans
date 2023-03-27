using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fix_rig2d
{
    //
    public long id;
    public Fix_vector2 velocity;
    public Fix_vector2 gravity;

    public Fix_rig2d(long id, Fix_vector2 velocity, Fix_vector2 gravity)
    {
        this.id = id;
        this.velocity = velocity;
        this.gravity = gravity;
    }

    public Fix_rig2d(long id, Fix_vector2 gravity)
    {
        this.id = id;
        this.velocity = new Fix_vector2(0, 0);
        this.gravity = gravity;
    }

    public Fix_rig2d(long id)
    {
        this.id = id;
        this.velocity = new Fix_vector2(0, 0);
        this.gravity = new Fix_vector2(0, 0);
    }
}