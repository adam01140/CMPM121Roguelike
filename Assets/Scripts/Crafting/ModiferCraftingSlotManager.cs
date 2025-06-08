using UnityEngine;
using TMPro;

public class ModiferCraftingSlotManager : MonoBehaviour
{
    public Mod modifier;
    public CraftingUIManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void Set(Mod modifier)
    { 
        if (this.modifier == null)
        {
        this.modifier = new Mod(modifier)
        }
    }

    public void Release()
    {
        if (this.modifier != null)
        {
        this.modifier = null
        }
    }

}
