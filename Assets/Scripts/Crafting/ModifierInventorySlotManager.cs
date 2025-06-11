using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModifierInventorySlotManager : MonoBehaviour
{
    public Mod mod;
    public CraftingUIManager manager;
    public Image icon;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.mod != null)
        {
            GameManager.Instance.spellIconManager.PlaceSprite(this.mod.icon, this.icon);
        }
    }

    public void OnMouseDown()
    {
        this.manager.SetSelectedMod(this.mod);
        this.manager.SetDesc(this.mod.description);
    }


    public void Set(Mod mod)
    {
        this.mod = mod;
    }

    public void Release()
    {
        this.mod = null;
    }
}
