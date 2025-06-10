using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    public GameObject craftingUI;


    public List<GameObject> spellBases;
    public List<GameObject> spellMod;
    public List<TMP_Text> descriptions;

    public GameObject baseSpellSlot;
    //Static slots for testing, should probably be a list or smthn
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    //
    public GameObject baseInvSlot;
    public GameObject modInvSlot;

    public Spell selectedBase;
    public Mod selectedMod;

    public GameObject energyIcon;
    public TMP_Text energyMax;
    public SpellCaster caster;

    public SpellBuilder builder;



    void Start()
    {
        //this.craftingUI.SetActive(true);
        this.builder = new SpellBuilder();

        this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>().manager = this;

        this.slot1.GetComponent<ModiferCraftingSlotManager>().manager = this;

        this.slot2.GetComponent<ModiferCraftingSlotManager>().manager = this;

        this.slot3.GetComponent<ModiferCraftingSlotManager>().manager = this;

        this.baseInvSlot.GetComponent<BaseInventorySlotManager>().manager = this;



        this.modInvSlot.GetComponent<ModifierInventorySlotManager>().manager = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.INWAVE)
        {
            this.craftingUI.SetActive(false);


        }
        else
        {
            this.craftingUI.SetActive(true);

        }
    }

    public void SetSelectedBase(Spell spellBase)
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
        this.selectedMod = mod;
    }

    public void SetCaster(SpellCaster spellCaster)
    {
        this.caster = spellCaster;
        this.builder = new SpellBuilder();

        this.baseInvSlot.GetComponent<BaseInventorySlotManager>().Set(this.builder.GetRandomBase(spellCaster));
    }


    public void SetCraftedSpell()
    {
        ModifierSpell spell = new ModifierSpell(this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>().spellBase);
        Mod temp;

        if (this.slot1.GetComponent<ModiferCraftingSlotManager>().modifier != null)
        {
            temp = this.slot1.GetComponent<ModiferCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        if (this.slot2.GetComponent<ModiferCraftingSlotManager>().modifier != null)
        {
            temp = this.slot2.GetComponent<ModiferCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }
        if (this.slot3.GetComponent<ModiferCraftingSlotManager>().modifier != null)
        {
            temp = this.slot3.GetComponent<ModiferCraftingSlotManager>().modifier;
            temp.ApplySelf(spell);
        }

    }















    // public void SetRelicOptions(List<Relic> relicSelection)
    // {
    //     if (relicSelection.Count > 0)
    //     {
    //         relicIcon1.SetActive(true);
    //         relicText1.gameObject.SetActive(true);
    //         relicButton1.SetActive(true);
    //         relicButton1.GetComponent<RelicButtonController>().player = GameManager.Instance.player.GetComponent<PlayerController>();
    //         relicText1.text = relicSelection[0].description;
    //         relicButton1.GetComponent<RelicButtonController>().relic = relicSelection[0];
    //         GameManager.Instance.relicIconManager.PlaceSprite(relicSelection[0].SpriteId, relicIcon1.GetComponent<Image>());
    //     }
    //     if (relicSelection.Count > 1)
    //     {
    //         relicIcon2.SetActive(true);
    //         relicText2.gameObject.SetActive(true);
    //         relicButton2.SetActive(true);
    //         relicButton2.GetComponent<RelicButtonController>().player = GameManager.Instance.player.GetComponent<PlayerController>();
    //         relicText2.text = relicSelection[1].description;
    //         relicButton2.GetComponent<RelicButtonController>().relic = relicSelection[1];
    //         GameManager.Instance.relicIconManager.PlaceSprite(relicSelection[1].SpriteId, relicIcon2.GetComponent<Image>());
    //     }
    // }
    // public void ClearRelicOptions()
    // {
    //     relicIcon1.SetActive(false);
    //     relicText1.gameObject.SetActive(false);
    //     relicButton1.SetActive(false);
    //     relicIcon2.SetActive(false);
    //     relicText2.gameObject.SetActive(false);
    //     relicButton2.SetActive(false);
    //}


}
