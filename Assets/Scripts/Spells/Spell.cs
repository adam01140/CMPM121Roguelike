using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

[Serializable]
public class Spell
{
    public string name;
    public string description;
    public int icon;
    public string N;
    public float spray;
    public Damage damage;
    public string mana_cost;
    public float cooldown;
    public Projectile projectile;
    public Projectile secondary_projectile;

    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    public Spell()
    {
        // this.name = other.name;
        // this.description = other.description;
        // this.icon = other.icon;
        // this.N = other.N;
        // this.spray = other.spray;
        // this.damage = other.damage;
        // this.mana_cost = other.mana_cost;
        // this.cooldown = other.cooldown;
        // this.projectile = other.projectile;
        // this.secondary_projectile = other.secondary_projectile;
        // this.owner = other.owner;
        // this.team = other.team;

    }

    public string GetName()
    {
        return this.name;
    }

    public int GetManaCost()
    {
        return 10; //needs string to RPN conversion
    }

    public int GetDamage()
    {
        return 100; //^^
    }

    public float GetCooldown()
    {
        return this.cooldown;
    }

    public virtual int GetIcon()
    {
        return this.icon;
    }

    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return new WaitForEndOfFrame();
    }

    void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage("temp"/*GetDamage()*/, Damage.Type.ARCANE));
        }

    }

}

class ArcaneBolt : Spell
{
    Spell inner;
    // override protected virtual void Cast(ValueModifier modifier)
    // {
    //     inner.Cast(new ValueAdder(10));
    // }
    ArcaneBolt(Spell inner)
    {
        this.name = inner.name;
        this.description = inner.description;
        this.icon = inner.icon;
        this.N = inner.N;
        this.spray = inner.spray;
        this.damage = inner.damage;
        this.mana_cost = inner.mana_cost;
        this.cooldown = inner.cooldown;
        this.projectile = inner.projectile;
        this.secondary_projectile = inner.secondary_projectile;
        this.owner = inner.owner;
        this.team = inner.team;

    }
}


class ValueModifier
{
    public virtual int Apply(int value)
    {
        return value;
    }
}

class ValueAdder : ValueModifier
{
    private int amount;

    public ValueAdder(int amount)
    {
        this.amount = amount;
    }
    public override int Apply(int value)
    {
        return value + amount;
    }
}
