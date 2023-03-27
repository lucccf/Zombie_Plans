using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixpoint
{
    private long Value;

    const int Frac_len = 32;

    public Fixpoint(long value, int fra_len)
    {
        Value = (value << Frac_len) / Qpow(10, fra_len);
    }

    public Fixpoint(long value)
    {
        Value = value;
    }
    
    public Fixpoint(float value)  //不推荐使用
    {
        Value = (long)(value * ((long)1 << Frac_len));
    }
    
    private long Qpow(long bas, int pow)
    {
        long ans = 1;
        while (pow > 0)
        {
            if ((pow & 1) == 1)
            {
                ans = ans * bas;
            }
            bas = bas * bas;
            pow = pow >> 1;
        }
        return ans;
    }

    public int to_int()
    {
        return (int)(Value >> Frac_len);
    }

    public long val()
    {
        return Value;
    }

    public float to_float()
    {
        return Value / (float)((long)1 << Frac_len);
    }

    public static Fixpoint operator +(Fixpoint f1, Fixpoint f2)
    {
        long new_v = f1.Value + f2.Value;
        return new Fixpoint(new_v);
    }

    public static Fixpoint operator -(Fixpoint f1, Fixpoint f2)
    {
        long new_v = f1.Value - f2.Value;
        return new Fixpoint(new_v);
    }

    public static Fixpoint operator *(Fixpoint f1, Fixpoint f2)
    {
        long new_v = (f1.Value >> (Frac_len >> 1)) * (f2.Value >> (Frac_len >> 1));
        return new Fixpoint(new_v);
    }

    public static Fixpoint operator /(Fixpoint f1, Fixpoint f2)
    {
        long new_v = (f1.Value << (Frac_len >> 1)) / (f2.Value >> (Frac_len >> 1));
        return new Fixpoint(new_v);
    }

    public static bool operator <(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value < f2.Value;
    }

    public static bool operator >(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value > f2.Value;
    }

    public static bool operator <=(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value <= f2.Value;
    }

    public static bool operator >=(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value >= f2.Value;
    }

    public static bool operator ==(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value == f2.Value;
    }

    public static bool operator !=(Fixpoint f1, Fixpoint f2)
    {
        return f1.Value != f2.Value;
    }

    public static Fixpoint operator >>(Fixpoint f1, int shift)
    {
        return new Fixpoint(f1.Value >> shift);
    }

    public static Fixpoint operator <<(Fixpoint f1, int shift)
    {
        return new Fixpoint(f1.Value << shift);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (GetType() != obj.GetType()) return false;
        return ((Fixpoint)obj).Value == Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public Fixpoint Clone()
    {
        return new Fixpoint(Value);
    }
}

public class Fix_vector2
{
    public Fixpoint x;
    public Fixpoint y;

    public Fix_vector2(Fixpoint x, Fixpoint y)
    {
        this.x = x;
        this.y = y;
    }

    public Fix_vector2(long x, long y)
    {
        this.x = new Fixpoint(x);
        this.y = new Fixpoint(y);
    }

    public Fix_vector2(Vector2 v)
    {
        this.x = new Fixpoint(v.x);
        this.y = new Fixpoint(v.y);
    }

    public Fix_vector2(Vector3 v)
    {
        this.x = new Fixpoint(v.x);
        this.y = new Fixpoint(v.y);
    }

    public Fix_vector2 Clone()
    {
        return new Fix_vector2(x.val(), y.val());
    }

    public static Fix_vector2 operator +(Fix_vector2 f1, Fix_vector2 f2)
    {
        return new Fix_vector2(f1.x + f2.x, f1.y + f2.y);
    }

    public static Fix_vector2 operator -(Fix_vector2 f1, Fix_vector2 f2)
    {
        return new Fix_vector2(f1.x - f2.x, f1.y - f2.y);
    }

    public static Fix_vector2 operator *(Fixpoint k, Fix_vector2 f)
    {
        return new Fix_vector2(k * f.x, k * f.y);
    }
}