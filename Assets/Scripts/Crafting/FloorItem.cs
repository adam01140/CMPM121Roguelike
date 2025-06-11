using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FloorItem : MonoBehaviour
{
    public Image icon;
    public int spriteId;




    public bool hasBase;
    public bool hasMod;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.haseBase = false;
        this.hasMod = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hasBase == true)
        {
            GameManager.Instance.relicIconManager.PlaceSprite(this.spriteId, this.icon);
        } else if (this.hasMod == true) {
            GameManager.Instance.relicIconManager.PlaceSprite(this.spriteId, this.icon);
        }

    }



    public void Set(int spriteId)
    {
        this.spriteId = spriteId
    }

}
