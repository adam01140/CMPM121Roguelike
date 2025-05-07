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


    public Spell Build()
    {

        // Spell spell = this.spell_bases.arcane_blast;

        // ModifierSpell new_spell = new ModifierSpell(spell);
        // Mod spell_mod = this.spell_bases.doubler;
        // spell_mod.ApplySelf(new_spell);

        return GetRandomSpell();

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
        allSpellsMods = new List<Mod>();
        // allSpellsMods.Add(this.spell_bases.damage_amp);
        // allSpellsMods.Add(this.spell_bases.speed_amp);
        // allSpellsMods.Add(this.spell_bases.doubler);
        // allSpellsMods.Add(this.spell_bases.splitter);
        // allSpellsMods.Add(this.spell_bases.chaos);
        // allSpellsMods.Add(this.spell_bases.homing);
        // allSpellsMods.Add(this.spell_bases.spray);
        allSpellsMods.Add(this.spell_bases.machine);


        //random spell generator

        // public List<Spell> allSpells; 

        // public Spell GetRandomSpell()
        // {


        //     int index = Random.Range(0, allSpells.Count);
        //     return allSpells[index];
        // }


        // levels = new Dictionary<string, Level>();
        // var leveltext = Resources.Load<TextAsset>("levels");
        // JToken jol = JToken.Parse(leveltext.text);
        // foreach (var level in jol)
        // {
        //     Level lvl = level.ToObject<Level>();
        //     levels[lvl.name] = lvl;
        // }
    }

    public Spell GetRandomSpell()
    {


        int index = Random.Range(0, allSpellsBases.Count);
        Spell temp = allSpellsBases[index];
        ModifierSpell spell = new ModifierSpell(temp);
        index = Random.Range(0, allSpellsMods.Count);
        Mod mod = allSpellsMods[index];
        mod.ApplySelf(spell);
        return spell;
    }




}
