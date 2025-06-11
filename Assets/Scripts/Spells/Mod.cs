using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using Unity.Mathematics;

[Serializable]
public abstract class Mod
{
    public string name;
    public string description;
    public string damage_multiplier;
    public string speed_multiplier;
    public string mana_multiplier;
    public string cooldown_multiplier;
    public string mana_adder;
    public float angle;
    public float delay;
    public string projectile_trajectory;
    public string second_projectile_trajectory;
    public string bleed_damage;
    public int slow_amount;
    public float slow_duration;
    public float knockback_amount;
    public RPN localRPN;


    public abstract void ApplySelf(ModifierSpell spell);
}

public class DamageAmp : Mod
{
    public DamageAmp()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        ValueMultiplier damage_mult = new ValueMultiplier(this.localRPN.RPN_to_float(damage_multiplier));
        spell.AddDamageModifier(damage_mult);
        ValueMultiplier mana_mult = new ValueMultiplier(this.localRPN.RPN_to_float(mana_multiplier));
        spell.AddManaModifier(mana_mult);
    }
}

public class SpeedAmp : Mod
{
    public SpeedAmp()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        ValueMultiplier speed_mult = new ValueMultiplier(this.localRPN.RPN_to_float(speed_multiplier));
        spell.AddSpeedModifier(speed_mult);
    }
}
public class Doubler : Mod
{
    public Doubler()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        ValueMultiplier mana_mult = new ValueMultiplier(this.localRPN.RPN_to_float(mana_multiplier));
        spell.AddManaModifier(mana_mult);
        ValueMultiplier cooldown_mult = new ValueMultiplier(this.localRPN.RPN_to_float(cooldown_multiplier));
        spell.AddCooldownModifier(cooldown_mult);
        spell.AddCasts(1);
        spell.AddMultiCastDelay(delay);
    }
}
public class Splitter : Mod
{
    public Splitter()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        spell.AddCasts(1);
        spell.AddMutliCastSpread(angle);
    }
}
public class Chaos : Mod
{
    public Chaos()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        // Dictionary<string, int> tempDict = new Dictionary<string, int>();
        // tempDict["wave"] = GameManager.Instance.wave;
        // RPN tempRPN = new RPN(tempDict);
        ValueMultiplier damage_mult = new ValueMultiplier(this.localRPN.RPN_to_float(damage_multiplier));
        spell.AddDamageModifier(damage_mult);
        spell.ChangeProjectileTrajectory(projectile_trajectory);

    }
}
public class Homing : Mod
{
    public Homing()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        // Dictionary<string, int> tempDict = new Dictionary<string, int>();
        // tempDict["wave"] = GameManager.Instance.wave;
        // RPN tempRPN = new RPN(tempDict);
        ValueMultiplier damage_mult = new ValueMultiplier(this.localRPN.RPN_to_float(damage_multiplier));
        spell.AddDamageModifier(damage_mult);
        ValueAdder mana_add = new ValueAdder(this.localRPN.RPN_to_int(mana_adder));
        spell.AddManaModifier(mana_add);
        spell.ChangeProjectileTrajectory(projectile_trajectory);

    }
}
public class Machine : Mod
{
    public Machine()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        ValueMultiplier damage_mult = new ValueMultiplier(this.localRPN.RPN_to_float(damage_multiplier));
        spell.AddDamageModifier(damage_mult);
        ValueMultiplier mana_mult = new ValueMultiplier(this.localRPN.RPN_to_float(mana_multiplier));
        spell.AddManaModifier(mana_mult);
        ValueMultiplier cooldown_mult = new ValueMultiplier(this.localRPN.RPN_to_float(cooldown_multiplier));
        spell.AddCooldownModifier(cooldown_mult);
        //int count = this.localRPN.RPN_to_int(this.N);
        spell.AddCasts(10);
        spell.AddMultiCastDelay(delay);
    }
}

public class Bleed : Mod
{
    public Bleed()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        ValueMultiplier damage_mult = new ValueMultiplier(this.localRPN.RPN_to_float(damage_multiplier));
        spell.AddDamageModifier(damage_mult);

        ValueAdder cooldown_mult = new ValueAdder(this.localRPN.RPN_to_int(cooldown_multiplier));
        spell.AddCooldownModifier(cooldown_mult);

        spell.AddDamageOverTimeEffect(new DamageTemp(bleed_damage, Damage.Type.PHYSICAL));

    }


}

public class Slow : Mod
{
    public Slow()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        spell.AddSlowEffect(slow_amount, slow_duration);


    }


}

public class Knockback : Mod
{
    public Knockback()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }
    public override void ApplySelf(ModifierSpell spell)
    {
        spell.AddKnockback(knockback_amount);


    }
}

public class Stun : Mod
{
    public Stun()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }

    public override void ApplySelf(ModifierSpell spell)
    {
        float stunDuration = this.localRPN.RPN_to_float(stun_duration);
        spell.AddStunEffect(stunDuration);
    }

    public string stun_duration;
}

public class Vulnerable : Mod
{
    public Vulnerable()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }

    public override void ApplySelf(ModifierSpell spell)
    {
        float multiplier = this.localRPN.RPN_to_float(vulnerability_multiplier);
        float duration = this.localRPN.RPN_to_float(vulnerability_duration);
        spell.AddVulnerabilityEffect(multiplier, duration);
    }

    public string vulnerability_multiplier;
    public string vulnerability_duration;
}

public class Inaccuracy : Mod
{
    public Inaccuracy()
    {
        Dictionary<string, int> tempDict = new Dictionary<string, int>();
        tempDict["wave"] = GameManager.Instance.wave;
        this.localRPN = new RPN(tempDict);
    }

    public override void ApplySelf(ModifierSpell spell)
    {
        float chance = this.localRPN.RPN_to_float(miss_chance);
        float duration = this.localRPN.RPN_to_float(miss_duration);
        spell.AddMissChanceEffect(chance, duration);
    }

    public string miss_chance;
    public string miss_duration;
}





