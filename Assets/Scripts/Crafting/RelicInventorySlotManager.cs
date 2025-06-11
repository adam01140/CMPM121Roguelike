using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RelicInventorySlotManager : MonoBehaviour
{
    public Relic relic;
    public CraftingUIManager manager;
    public Image icon;

    public bool hasRelic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.hasRelic = false;
        this.Relic = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hasRelic == true)
        {
            GameManager.Instance.relicIconManager.PlaceSprite(this.relic.icon, this.icon);
        }
    }

    public void OnMouseDown()
    {
        this.manager.SetSelectedRelic(this.Relic);

    }


    public void Set(Relic relic)
    {
        if (this.relic == null)
        {
            this.relic = relic;
        }
    }

    public void Release()
    {
        if (this.relic != null)
        {
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
