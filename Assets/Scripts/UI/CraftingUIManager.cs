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

    

    public GameObject energyIcon;
    public TMP_Text energyMax;

    

    void Start()
    {
        craftingUI.SetActive(false);
        relicsSet = false;
        baseSpellSlot = Instantiate(baseSlot, craftingUI.transform);
        baseSpellSlot.transform.localPosition = new Vector3(60, -90);
        baseSpellSlot.GetComponent<BaseCraftingSlotManager>().manager = this;


        slot1 = Instantiate(modSlot, craftingUI.transform);
        slot1.transform.localPosition = new Vector3(70, -90);
        slot1.GetComponent<ModiferCraftingSlotManager>().manager = this;

        slot2 = Instantiate(modSlot, craftingUI.transform);
        slot2.transform.localPosition = new Vector3(80, -90);
        slot2.GetComponent<ModiferCraftingSlotManager>().manager = this;

        slot3 = Instantiate(modSlot, craftingUI.transform);
        slot3.transform.localPosition = new Vector3(90, -90);
        slot3.GetComponent<ModiferCraftingSlotManager>().manager = this;


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND || GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            rewardUI.SetActive(true);
            if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
            {
                buttonText.text = "Try Again?";
                changeSpellButton.SetActive(false);
                changeModButton.SetActive(false);
                addModButton.SetActive(false);

            }
            else if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
            {
                changeSpellButton.SetActive(true);
                changeModButton.SetActive(true);
                addModButton.SetActive(true);
                if (GameManager.Instance.wave % 3 == 0 && !relicsSet)
                {
                    RelicManager relicManager = new RelicManager();
                    SetRelicOptions(relicManager.genRelicSelection());
                    relicsSet = true;
                }
            }

        }
        else
        {
            rewardUI.SetActive(false);
            relicsSet = false;

        }
    }



    public void SetCraftedSpell(){
        ModifierSpell spell = new ModifierSpell(this.baseSpellSlot.GetComponent<BaseCraftingSlotManager>.spellBase);
        Mod temp;

        if (this.slot1.GetComponent<ModiferCraftingSlotManager>.modifier != null){
            temp = this.slot1.GetComponent<ModiferCraftingSlotManager>.modifier;
            temp.ApplySelf(spell)
        }
        if (this.slot2.GetComponent<ModiferCraftingSlotManager>.modifier != null){
            temp = this.slot2.GetComponent<ModiferCraftingSlotManager>.modifier;
            temp.ApplySelf(spell)
        }
        if (this.slot3.GetComponent<ModiferCraftingSlotManager>.modifier != null){
            temp = this.slot3.GetComponent<ModiferCraftingSlotManager>.modifier;
            temp.ApplySelf(spell)
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
