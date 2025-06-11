using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BaseCraftingSlotManager : MonoBehaviour
{
    public ModifierSpell spellBase;
    public CraftingUIManager manager;
    public Image icon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spellBase = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.spellBase != null)
        {
            GameManager.Instance.spellIconManager.PlaceSprite(this.spellBase.icon, this.icon);
        }


    }

    public void OnMouseDown()
    {
        if (this.manager.selectedBase != null)
        {
            this.Set(this.manager.selectedBase);
        }

    }


    public void Set(ModifierSpell spellBase)
    {
        this.spellBase = spellBase;
    }

    public void Release()
    {

        this.spellBase = null;

    }
    public void OnMouseOver()
    {
        //Set textbox to active
    }
    public void OnMouseExit()
    {
        //Set textbox to inactive
    }
}
