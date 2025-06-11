using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class FloorItemManager : MonoBehaviour
{
    public SpellBuilder builder;
    public RelicManager relicManager;
    public InventoryManager inventory;
    public CraftingUIManager crafter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.builder = new SpellBuilder();
        this.relicManager = new RelicManager();
        this.Register();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Register()
    {
        EventBus.Instance.OnEnemyKilled += OnEnemyKill;
    }

    private void OnEnemyKill(Hittable enemy)
    {
        int rand = UnityEngine.Random.Range(0, 5);
        if (rand == 0)
        {
            SpawnFloorItem();
        }
    }

    public void SpawnFloorItem()
    {
        int rand = UnityEngine.Random.Range(0, 10);
        if (rand == 0 || rand == 1 || rand == 2)
        {
            Spell spell = this.builder.GetRandomBase(this.crafter.caster);
            if (spell != null)
            {
                inventory.AddToInvBase(this.builder.GetRandomBase(this.crafter.caster));
            }
            else
            {
                Debug.Log("Save Me");
            }
        }
        else if (rand == 9)
        {
            inventory.AddToInvRelic(this.relicManager.GenRelic());
        }
        else
        {
            inventory.AddToInvMod(this.builder.GetRandomMod());
        }
    }


}
