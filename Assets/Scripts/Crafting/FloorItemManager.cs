using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FloorItemManager : MonoBehaviour
{
    public SpellBuilder builder;
    public RelicManager relicManager;
    public InventoryManager inventory;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.builder = new SpellBuilder();
        this.relicManager = new relicManager(); 
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
        int rand = Random.Range(0, 10);
        if (rand == 0){
            SpawnFloorItem();
        }
    }

    public void SpawnFloorItem(){
        int rand = Random.Range(0, 10);
        if (rand == 0 || rand == 1 || rand == 2){
            inventory.AddToInvBase(this.builder.GetRandomBase(spellCaster));
        } else if (rand == 9){
            inventory.AddToInvRelic(this.relicManager.GenRelic());
        } else{
            inventory.AddtoInvMod(this.builder.GetRandomMod())
        }
    }


}
