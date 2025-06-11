using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;



public class SpellBuilder
{
    public Spells spell_bases;
    List<Spell> allSpellsBases;
    List<Mod> allSpellsMods;
    private int lastBaseIndex; //Should be stored elsewhere if player is able to hold more than 1 spell
    private int lastModIndex; //^^


    public ModifierSpell Build(SpellCaster owner)
    {

        return GetRandomSpell(owner);

    }
    public ModifierSpell BuildSameBase(SpellCaster owner)
    {
        Spell temp = new Spell(allSpellsBases[lastBaseIndex]);
        temp.AssignOwner(owner);
        ModifierSpell spell = new ModifierSpell(temp);
        Mod mod = GetRandomMod();
        mod.ApplySelf(spell);
        return spell;
    }
    public ModifierSpell BuildSameMod(SpellCaster owner)
    {
        ModifierSpell spell = GetRandomBase(owner);
        Mod mod = allSpellsMods[lastModIndex];
        mod.ApplySelf(spell);
        return spell;
    }

    public void AddSpellMod(ModifierSpell spell)
    {
        Mod mod = GetRandomMod();
        mod.ApplySelf(spell);
    }


    public SpellBuilder()
    {
        var spellText = Resources.Load<TextAsset>("spells");
        JToken jo = JToken.Parse(spellText.text);
        spell_bases = jo.ToObject<Spells>();

        allSpellsBases = new List<Spell>();
        allSpellsBases.Add(this.spell_bases.arcane_bolt);
        allSpellsBases.Add(this.spell_bases.magic_missile);
        allSpellsBases.Add(this.spell_bases.arcane_blast);
        allSpellsBases.Add(this.spell_bases.arcane_spray);
        allSpellsBases.Add(this.spell_bases.arcane_bounce);
        allSpellsBases.Add(this.spell_bases.fireball);
        allSpellsBases.Add(this.spell_bases.chaining_lightning);
        allSpellsMods = new List<Mod>();
        allSpellsMods.Add(this.spell_bases.damage_amp);
        allSpellsMods.Add(this.spell_bases.speed_amp);
        allSpellsMods.Add(this.spell_bases.doubler);
        allSpellsMods.Add(this.spell_bases.splitter);
        allSpellsMods.Add(this.spell_bases.chaos);
        allSpellsMods.Add(this.spell_bases.homing);
        allSpellsMods.Add(this.spell_bases.bleed);
        allSpellsMods.Add(this.spell_bases.machine);
        allSpellsMods.Add(this.spell_bases.slow);
        allSpellsMods.Add(this.spell_bases.knockback);
        allSpellsMods.Add(this.spell_bases.stun);        
        allSpellsMods.Add(this.spell_bases.vulnerable);  
        allSpellsMods.Add(this.spell_bases.inaccuracy);   


    }

    public ModifierSpell GetRandomSpell(SpellCaster owner)
    {

        ModifierSpell spell = GetRandomBase(owner);

        Mod mod = GetRandomMod();
        mod.ApplySelf(spell);

        return spell;
    }

    public ModifierSpell GetRandomBase(SpellCaster owner)
    {
        int index = Random.Range(0, allSpellsBases.Count);
        this.lastBaseIndex = index;
        Spell temp = allSpellsBases[index];
        temp.AssignOwner(owner);
        ModifierSpell spell = new ModifierSpell(temp);
        Debug.Log("Setting base " + spell.name);
        return spell;
    }

    public Mod GetRandomMod()
    {
        int index = Random.Range(0, allSpellsMods.Count);
        this.lastModIndex = index;
        Mod mod = allSpellsMods[index];
        Debug.Log("Adding mod " + mod.name);
        return mod;
    }





}
