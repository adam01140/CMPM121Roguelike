using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RelicSlotManager : MonoBehaviour
{
    public Relic relic;
    public CraftingUIManager manager;
    public Image icon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (this.relic != null)
        {
            GameManager.Instance.relicIconManager.PlaceSprite(this.relic.SpriteId, this.icon);
        }

    }

    public void OnMouseDown()
    {
        Debug.Log(this.manager.selectedRelic);
        if (this.manager.selectedRelic != null)
        {
            this.Set(this.manager.selectedRelic);
        }

    }


    public void Set(Relic relic)
    {
        this.relic = relic;
    }

    public void Release()
    {
        this.relic = null;
    }


}
