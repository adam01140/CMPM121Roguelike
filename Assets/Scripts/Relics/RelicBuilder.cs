using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

public class RelicBuilder
{



    public List<Relic> allRelics;

    private List<int> usedIndexes;
    private int providedIndex1;
    private int providedIndex2;


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

    public List<int> GenRelicIndices()
    {
        List<int> indices = new List<int>();
        if (allRelics.Count < 2) return indices;
        int first, second;
        do
        {
            first = Random.Range(0, allRelics.Count);
            second = Random.Range(0, allRelics.Count);
        } while (second == first && allRelics.Count > 1);
        indices.Add(first);
        indices.Add(second);
        return indices;
    }

    public void RemoveRelic(Relic relic)
    {
        allRelics.RemoveAll(r => r.Name == relic.Name);
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
            var untilToken = relicJson["effect"]?["until"];
            RelicTrigger until = null;
            if (untilToken != null)
            {
                string untilType = untilToken.ToString();
                switch (untilType)
                {
                    case "move":
                        until = new OnMoveTriggerUntil();
                        break;
                    case "cast-spell":
                        until = new OnCastSpellTriggerUntil();
                        break;
                    case "take-damage":
                        until = new OnTakeDamageUntil();
                        break;
                }
            }
            trigger.description = relicJson["trigger"]["description"].ToString();
            effect.description = relicJson["effect"]["description"].ToString();
            Relic relic = new Relic(name, sprite, trigger, effect, until);
            allRelics.Add(relic);
        }
    }
}


public static class RelicEffectFactory
{
    public static RelicEffect Create(JToken effectJson)
    {
        string type = effectJson["type"]?.ToString();
        string amount;
        switch (type)
        {
            case "gain-mana":
                int mana = int.Parse(effectJson["amount"].ToString());
                return new GainManaEffect(mana);
            case "gain-spellpower":
                amount = (effectJson["amount"].ToString());
                return new GainSpellPowerEffect(amount);
            case "gain-speed":
                amount = (effectJson["amount"].ToString());
                return new GainSpeedEffect(amount);
            case "gain-health":
                int health = int.Parse(effectJson["amount"].ToString());
                return new GainHealthEffect(health);
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
        float seconds;
        string type = triggerJson["type"]?.ToString();
        switch (type)
        {
            case "take-damage":
                return new TakeDamageTrigger();
            case "stand-still":
                seconds = triggerJson["amount"] != null ? float.Parse(triggerJson["amount"].ToString()) : 0f;
                return new StandStillTrigger(seconds);
            case "no-cast":
                seconds = triggerJson["amount"] != null ? float.Parse(triggerJson["amount"].ToString()) : 0f;
                return new NoRecentCastTrigger(seconds);
            case "on-kill":
                return new OnKillTrigger();
            case "on-start-wave":
                return new OnStartWaveTrigger();
            default:
                Debug.LogWarning($"Unknown trigger type: {type}");
                return null;
        }
    }
}
