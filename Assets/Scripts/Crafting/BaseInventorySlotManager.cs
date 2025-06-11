using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseInventorySlotManager : MonoBehaviour
{
    public ModifierSpell spellBase;
    public CraftingUIManager manager;
    public Image icon;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        this.manager.SetSelectedBase(this.spellBase);
        this.manager.SetDesc(this.spellBase.description);

    }


    public void SetSpell(ModifierSpell spellBase)
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
