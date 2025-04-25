using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;



public class SpellBuilder
{
    public Spells spell_bases;


    public Spell Build(SpellCaster owner)
    {
        return new Spell();
    }


    public SpellBuilder()
    {
        var spellText = Resources.Load<TextAsset>("spells");
        JToken jo = JToken.Parse(spellText.text);
        Spells spell_bases = jo.ToObject<Spells>();
        Debug.Log(spell_bases);


        // levels = new Dictionary<string, Level>();
        // var leveltext = Resources.Load<TextAsset>("levels");
        // JToken jol = JToken.Parse(leveltext.text);
        // foreach (var level in jol)
        // {
        //     Level lvl = level.ToObject<Level>();
        //     levels[lvl.name] = lvl;
        // }
    }

}
