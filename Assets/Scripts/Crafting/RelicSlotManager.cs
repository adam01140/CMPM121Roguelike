using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RelicSlotManager : MonoBehaviour
{
    public Relic relic;
    public CraftingUIManager manager;
    public Image icon;

    public bool hasRelic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.relic = null;
        this.hasRelic = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.hasRelic);
        if (this.hasRelic == true)
        {
            GameManager.Instance.relicIconManager.PlaceSprite(this.relic.icon, this.icon);
        }

    }

    public void OnMouseDown()
    {
        if (this.manager.selectedRelic != null)
        {
            this.Set(this.manager.selectedRelic);
        }

    }


    public void Set(Relic relic)
    {
        if (this.hasRelic == false)
        {
            this.relic = relic;
            this.hasRelic = true;
        }
    }

    public void Release()
    {
        if (this.hasRelic == true)
        {
            this.hasRelic = false;
            this.relic = null;
        }
    }

    public void OnMouseOver(){
        //Set textbox to active
    }
    public void OnMouseExit(){
        //Set textbox to inactive
    }
}
