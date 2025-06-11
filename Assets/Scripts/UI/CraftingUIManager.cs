using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using NUnit.Framework.Constraints;

public class CraftingUIManager : MonoBehaviour
{
    public GameObject craftingUI;

    public InventoryManager inventory;

    public GameObject description;

    public GameObject baseSpellSlot;
    //Static slots for testing, should probably be a list or smthn
    public GameObject modSlot1;
    public GameObject modSlot2;
    public GameObject modSlot3;
    public GameObject modSlot4;
    public GameObject relicSlot1;
    public GameObject relicSlot2;
    public GameObject relicSlot3;
    public GameObject relicSlot4;


    //

    public ModifierSpell selectedBase;
    public Mod selectedMod;

    public Relic selectedRelic;

    public GameObject energyIcon;
    public TMP_Text energyMax;
    public SpellCaster caster;

    public SpellBuilder builder;

    private bool needToPopulate;

    private bool firstTimeLoad;


    void Start()
    {
        //this.craftingUI.SetActive(true);
        this.builder = new SpellBuilder();


        this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>().manager = this;

        this.modSlot1.GetComponent<ModifierCraftingSlotManager>().manager = this;

        this.modSlot2.GetComponent<ModifierCraftingSlotManager>().manager = this;

        this.modSlot3.GetComponent<ModifierCraftingSlotManager>().manager = this;

        this.modSlot4.GetComponent<ModifierCraftingSlotManager>().manager = this;

        this.relicSlot1.GetComponent<RelicSlotManager>().manager = this;

        this.relicSlot2.GetComponent<RelicSlotManager>().manager = this;

        this.relicSlot3.GetComponent<RelicSlotManager>().manager = this;

        this.relicSlot4.GetComponent<RelicSlotManager>().manager = this;

        this.firstTimeLoad = true;



    }

    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND || GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            this.craftingUI.SetActive(true);
            if (this.needToPopulate == true)
            {
                this.needToPopulate = false;
                this.inventory.PopulateInventoryUI();
            }
            if (this.firstTimeLoad == true)
            {
                this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>().spellBase = builder.GetFirstSpell(this.caster);
                this.firstTimeLoad = false;
            }

        }
        else
        {
            this.craftingUI.SetActive(false);
            this.needToPopulate = true;


        }
    }

    public void SetSelectedBase(ModifierSpell spellBase)
    {
        if (spellBase == null)
        {
            Debug.Log("Invalid Base");
            return;
        }
        if (this.selectedMod != null)
        {
            this.selectedMod = null;
        }
        if (this.selectedRelic != null)
        {
            this.selectedRelic = null;
        }
        this.selectedBase = spellBase;
    }
    public void SetSelectedMod(Mod mod)
    {
        if (mod == null)
        {
            Debug.Log("Invalid Base");
            return;
        }
        if (this.selectedBase != null)
        {
            this.selectedBase = null;
        }
        if (this.selectedRelic != null)
        {
            this.selectedRelic = null;
        }

        this.selectedMod = mod;
    }
    public void SetSelectedRelic(Relic relic)
    {
        if (relic == null)
        {
            Debug.Log("Invalid Base");
            return;
        }
        if (this.selectedBase != null)
        {
            this.selectedBase = null;
        }
        if (this.selectedMod != null)
        {
            this.selectedMod = null;
        }
        this.selectedRelic = relic;
        Debug.Log(this.selectedRelic);
    }


    public void SetCaster(SpellCaster spellCaster)
    {
        this.caster = spellCaster;
        this.builder = new SpellBuilder();


    }

    public void SetDesc(string desc)
    {
        this.description.GetComponent<TextMeshProUGUI>().text = desc;
    }


    public ModifierSpell GetCraftedSpell()
    {
        ModifierSpell spell = this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>().spellBase;
        Mod temp;

        if (this.modSlot1.GetComponent<ModifierCraftingSlotManager>().modifier != null)
        {
            temp = this.modSlot1.GetComponent<ModifierCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        if (this.modSlot2.GetComponent<ModifierCraftingSlotManager>().modifier != null)
        {
            temp = this.modSlot2.GetComponent<ModifierCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        if (this.modSlot3.GetComponent<ModifierCraftingSlotManager>().modifier != null)
        {
            temp = this.modSlot3.GetComponent<ModifierCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        if (this.modSlot4.GetComponent<ModifierCraftingSlotManager>().modifier != null)
        {
            temp = this.modSlot4.GetComponent<ModifierCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        return spell;
    }

    public void SetRelics()
    {
        Relic temp;
        PlayerController player = GameManager.Instance.player.GetComponent<PlayerController>();
        player.ClearRelics();
        if (this.relicSlot1.GetComponent<RelicSlotManager>().relic != null)
        {
            temp = this.relicSlot1.GetComponent<RelicSlotManager>().relic;
            player.SetPlayerRelic(temp);
        }
        if (this.relicSlot2.GetComponent<RelicSlotManager>().relic != null)
        {
            temp = this.relicSlot2.GetComponent<RelicSlotManager>().relic;
            player.SetPlayerRelic(temp);
        }
        if (this.relicSlot3.GetComponent<RelicSlotManager>().relic != null)
        {
            temp = this.relicSlot3.GetComponent<RelicSlotManager>().relic;
            player.SetPlayerRelic(temp);
        }
        if (this.relicSlot4.GetComponent<RelicSlotManager>().relic != null)
        {
            temp = this.relicSlot4.GetComponent<RelicSlotManager>().relic;
            player.SetPlayerRelic(temp);
        }
    }















}
