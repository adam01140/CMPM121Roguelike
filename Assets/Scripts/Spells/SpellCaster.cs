using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCaster
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public int spell_power;
    public Hittable.Team team;
    public ModifierSpell spell;

    public SpellBuilder builder;

    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, Hittable.Team team, int spell_power)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.spell_power = spell_power;

        this.builder = new SpellBuilder();
        this.spell = builder.Build(this);
        this.spell.AssignOwner(this);
    }

    public void UpdateStats(int mana, int mana_reg, Hittable.Team team, int spell_power)
    {
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.spell_power = spell_power;
    }

    public void NewSpellBase()
    {

        this.spell = null;
        this.spell = builder.BuildSameMod(this);
        this.spell.AssignOwner(this);
    }
    public void NewSpellMod()
    {
        this.spell = null;
        this.spell = builder.BuildSameBase(this);
        this.spell.AssignOwner(this);
    }
    public void AddSpellMod()
    {
        builder.AddSpellMod(this.spell);
    }
    public void updateSpellPower(int spellpower)
    {
        this.spell_power = spellpower;
    }

    public void AddMana(int amount)
    {
        if (this.mana + amount > this.max_mana)
        {
            this.mana = this.max_mana;
        }
        else
        {
            this.mana += amount;
        }
    }

    public IEnumerator Cast(Vector3 where, Vector3 target)
    {
        if (mana >= spell.GetManaCost() && spell.IsReady())
        {
            mana -= spell.GetManaCost();
            EventBus.Instance.DoCastSpell(this.spell);
            yield return spell.Cast(where, target, team);
        }
        yield break;
    }

}
