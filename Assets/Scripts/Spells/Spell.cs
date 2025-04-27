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
    public RPN rpn;

    public Spell()
    {


    }

    public void AssignOwner(SpellCaster owner)
    {
        this.owner = owner;
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["power"] = this.owner.spell_power;
        this.rpn = new RPN(tempDict);
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetManaCost()
    {
        if (this.rpn != null)
        {
            return this.rpn.RPN_to_int(this.mana_cost);
        }
        else
        {
            Debug.Log("Spell was cast without a spell owner, use AssignOwner() method on spell creation");
            return 10;
        }
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

    public void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            other.Damage(new Damage("temp"/*GetDamage()*/, Damage.Type.ARCANE));
        }

    }

}
public class ArcaneBolt : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, 15f, this.OnHit);
        yield return new WaitForEndOfFrame();
    }
    ArcaneBolt() : base()
    {
    }
}

public class MagicMissile : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, 15f, this.OnHit);
        yield return new WaitForEndOfFrame();
    }
    MagicMissile() : base()
    {
    }
}

public class ArcaneBlast : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, 15f, this.OnHit);
        yield return new WaitForEndOfFrame();
    }
    ArcaneBlast() : base()
    {
    }
}

public class ArcaneSpray : Spell
{
    override public IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Projectile projectile = this.projectile;
        this.team = team;
        GameManager.Instance.projectileManager.CreateProjectile(0, projectile.trajectory, where, target - where, 15f, this.OnHit);
        yield return new WaitForEndOfFrame();
    }
    ArcaneSpray() : base()
    {
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
