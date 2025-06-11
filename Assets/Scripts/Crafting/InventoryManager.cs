using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public List<Spell> spellBases;
    public List<Mod> spellMods;
    public List<Relic> relics;

    public GameObject baseInvSlot;
    public GameObject modInvSlot;



    void Start()
    {
       this.spellBases = new List<GameObject>();
       this.spellMods = new List<GameObject>();
        this.baseInvSlot.GetComponent<BaseInventorySlotManager>();
        this.modInvSlot.GetComponent<ModifierInventorySlotManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddToInvBase(Spell spellBase){
        this.spellBases.Add(spellBase);
    }

    void AddToInvMod(Mod mod){
        this.spellMods.Add(mod);
    }
    void AddToInvRelic(Relic relic){
        this.relics.Add(relic);
    }
    void RemoveBase(Spell spellBase){
        if (this.spellBases.Contains(spellBase)){
            this.spellBases.Remove(spellBase);
        }
    }
    void RemoveMod(Mod mod){
        if (this.spellMod.Contains(mod)){
            this.spellMod.Remove(mod);
        }
    }
    void RemoveRelic(Relic relic){
        if (this.relics.Contains(relic)){
            this.relics.Remove(relic);
        }
    }



}
