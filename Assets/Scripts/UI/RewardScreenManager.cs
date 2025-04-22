using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    //Should probalbly refactor into seperate manager script but this works for now since it's not that many objects
    public GameObject rewardUI;
    public GameObject EnemyKillsText;
    public GameObject DamageDealtText;
    public GameObject DamageReceivedText;
    public GameObject TimeSpentText;
    public GameObject GameStateText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND || GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            rewardUI.SetActive(true);
            // EnemyKillsText.SetActive(true);
            // DamageDealtText.SetActive(true);
            // DamageReceivedText.SetActive(true);
            // TimeSpentText.SetActive(true);
            // GameStateText.SetActive(true);
        }
        else
        {
            rewardUI.SetActive(false);
            // EnemyKillsText.SetActive(false);
            // DamageDealtText.SetActive(false);
            // DamageReceivedText.SetActive(false);
            // TimeSpentText.SetActive(false);
            // GameStateText.SetActive(false);
        }
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            // GameStateText.GetComponent<TMPro.TMP_Text>().text = "Game Over";
            // DamageDealtText.GetComponent<TMPro.TMP_Text>().text = "Damage Dealt: " + GameManager.damageDealt;
            // DamageReceivedText.GetComponent<TMPro.TMP_Text>().text = "Damage Received: " + GameManager.damageReceived;
            // TimeSpentText.GetComponent<TMPro.TMP_Text>().text = "Time Spent: " + GameManager.timeSpent;
        }
    }


}
