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

    }

    public void OnMouseDown()
    {
        this.manager.SetSelectedMod(this.mod);

    }


    public void Set(Mod mod)
    {
        if (this.mod == null)
        {
            this.mod = mod;
        }
    }

    public void Release()
    {
        if (this.mod != null)
        {
            this.mod = null;
        }
    }
}
