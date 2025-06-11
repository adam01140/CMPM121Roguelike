using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModifierCraftingSlotManager : MonoBehaviour
{
    public Mod modifier;
    public CraftingUIManager manager;
    public Image icon;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.modifier != null)
        {
            GameManager.Instance.spellIconManager.PlaceSprite(this.modifier.icon, this.icon);
        }
    }


    public void OnMouseDown()
    {
        if (this.manager.selectedMod != null)
        {
            this.Set(this.manager.selectedMod);
        }

    }



    public void Set(Mod modifier)
    {
        this.modifier = modifier;

    }

    public void Release()
    {
        this.modifier = null;
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
