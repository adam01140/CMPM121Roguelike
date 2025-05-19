using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

public class RelicBuilder
{



    private List<Relic> allRelics;

    private static RelicBuilder theInstance;
    public static RelicBuilder Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new RelicBuilder();
            return theInstance;
        }
    }



    public RelicBuilder()
    {
        allRelics = new List<Relic>();
        LoadRelicsFromJson();
    }

    public Relic GenRelic()
    {
        int index = Random.Range(0, allRelics.Count);
        return new Relic(allRelics[index]);
    }




    public void LoadRelicsFromJson()
    {
        TextAsset relicsText = Resources.Load<TextAsset>("relics");
        JArray relicsArray = JArray.Parse(relicsText.text);

        foreach (var relicJson in relicsArray)
        {
            string name = relicJson["name"].ToString();
            int sprite = (int)relicJson["sprite"];

            // Use your factory methods here
            RelicTrigger trigger = RelicTriggerFactory.Create(relicJson["trigger"]);
            RelicEffect effect = RelicEffectFactory.Create(relicJson["effect"]);
            Relic relic = new Relic(name, sprite, trigger, effect);
            allRelics.Add(relic);
        }
    }
}


public static class RelicEffectFactory
{
    public static RelicEffect Create(JToken effectJson)
    {
        string type = effectJson["type"]?.ToString();
        switch (type)
        {
            case "gain-mana":
                int mana = int.Parse(effectJson["amount"].ToString());
                return new GainManaEffect(mana);
            case "gain-spellpower":
                string amount = (effectJson["amount"].ToString());
                return new GainSpellPowerEffect(amount);
            default:
                Debug.LogWarning($"Unknown effect type: {type}");
                return null;
        }
    }
}


public static class RelicTriggerFactory
{
    public static RelicTrigger Create(JToken triggerJson)
    {
        string type = triggerJson["type"]?.ToString();
        switch (type)
        {
            case "take-damage":
                return new TakeDamageTrigger();
            case "stand-still":
                float seconds = triggerJson["amount"] != null ? float.Parse(triggerJson["amount"].ToString()) : 0f;
                return new StandStillTrigger(seconds);
            case "on-kill":
                return new OnKillTrigger();
            // Add more cases as needed
            default:
                Debug.LogWarning($"Unknown trigger type: {type}");
                return null;
        }
    }
}
