using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public List<ModifierSpell> spellBases;
    public List<Mod> spellMods;
    public List<Relic> relics;

    public CraftingUIManager crafter;

    public List<GameObject> slots;

    public GameObject baseInvSlot;
    public GameObject modInvSlot;
    public GameObject relicInvSlot;

    // Add these public fields to assign in the Unity Editor
    public Transform baseInvParent;
    public Transform modInvParent;
    public Transform relicInvParent;



    void Start()
    {
        this.spellBases = new List<ModifierSpell>();
        this.spellMods = new List<Mod>();
        this.relics = new List<Relic>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToInvBase(ModifierSpell spellBase)
    {
        Debug.Log(spellBase);
        this.spellBases.Add(spellBase);
    }

    public void AddToInvMod(Mod mod)
    {
        this.spellMods.Add(mod);
    }
    public void AddToInvRelic(Relic relic)
    {
        this.relics.Add(relic);
    }
    public void RemoveBase(ModifierSpell spellBase)
    {
        if (this.spellBases.Contains(spellBase))
        {
            this.spellBases.Remove(spellBase);
        }
    }
    void RemoveMod(Mod mod)
    {
        if (this.spellMods.Contains(mod))
        {
            this.spellMods.Remove(mod);
        }
    }
    void RemoveRelic(Relic relic)
    {
        if (this.relics.Contains(relic))
        {
            this.relics.Remove(relic);
        }
    }

    // Call this to refresh the inventory UI
    public void PopulateInventoryUI()
    {
        foreach (Transform child in baseInvParent) Destroy(child.gameObject);
        foreach (Transform child in modInvParent) Destroy(child.gameObject);
        foreach (Transform child in relicInvParent) Destroy(child.gameObject);


        float XOffset = 40f;
        //float relicYOffset = -60f;

        int i = 0;
        foreach (var spellBase in spellBases)
        {
            var slot = Instantiate(baseInvSlot, baseInvParent);
            slot.GetComponent<BaseInventorySlotManager>().manager = crafter;
            slot.transform.localPosition = new Vector3(-250 + i * XOffset, 0, 0);
            slot.GetComponent<BaseInventorySlotManager>().SetSpell(spellBase);
            slots.Add(slot);
            i++;
        }
        foreach (var mod in spellMods)
        {
            var slot = Instantiate(modInvSlot, modInvParent);
            slot.GetComponent<ModifierInventorySlotManager>().manager = crafter;
            slot.transform.localPosition = new Vector3(-250 + i * XOffset, 0, 0);
            slot.GetComponent<ModifierInventorySlotManager>().Set(mod);
            i++;
        }
        foreach (var relic in relics)
        {
            var slot = Instantiate(relicInvSlot, relicInvParent);
            slot.GetComponent<RelicInventorySlotManager>().manager = crafter;
            slot.transform.localPosition = new Vector3(-250 + i * XOffset, 0, 0);
            slot.GetComponent<RelicInventorySlotManager>().Set(relic);
            i++;
        }
    }

}
