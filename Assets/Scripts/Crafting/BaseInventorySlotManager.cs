using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseInventorySlotManager : MonoBehaviour
{
    public Spell spellBase;
    public CraftingUIManager manager;
    public Image icon;

    public bool hasSpell;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.spellBase = null;
        this.hasSpell = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.hasSpell);
        if (this.hasSpell == true)
        {
            GameManager.Instance.relicIconManager.PlaceSprite(this.spellBase.icon, this.icon);
        }
    }

    public void OnMouseDown()
    {
        this.manager.SetSelectedBase(this.spellBase);

    }


    public void Set(Spell spellBase)
    {
        if (this.hasSpell == false)
        {
            this.spellBase = spellBase;
            this.hasSpell = true;
        }
    }

    public void Release()
    {
        if (this.hasSpell == true)
        {
            this.hasSpell = false;
            this.spellBase = null;
        }
    }
}
