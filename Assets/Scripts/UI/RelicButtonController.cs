using UnityEngine;
using TMPro;

public class RelicButtonController : MonoBehaviour
{
    public Relic relic;
    public PlayerController player;
    public RewardScreenManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }




    public void SetRelic()
    {
        player.SetPlayerRelic(relic);
        manager.ClearRelicOptions();
        RelicBuilder.Instance.RemoveRelic(relic);
    }
}
