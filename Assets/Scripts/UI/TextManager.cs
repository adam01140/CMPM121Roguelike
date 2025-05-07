using UnityEngine;
using TMPro;

public class PlayerStatTextManager : MonoBehaviour
{
    TextMeshProUGUI temp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        temp = GetComponent<TextMeshProUGUI>();
        temp.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND )
        {
            Debug.Log(GameManager.Instance.projectileManager.projectiles[0]);
            temp.text = "Damage Dealt: " + GameManager.Instance.damageDealt +
                "\nDamage Received: " + GameManager.Instance.damageReceived +
                "\nTime Spent: " + GameManager.Instance.timeSpent +
                "\nNew Spell: " + GameManager.Instance.projectileManager.projectiles[0];
        }
        else if (GameManager.Instance.state == GameManager.GameState.GAMEOVER){
            temp.text = "Damage Dealt: " + GameManager.Instance.damageDealt +
                "\nDamage Received: " + GameManager.Instance.damageReceived +
                "\nTime Spent: " + GameManager.Instance.timeSpent;
        }
        {
            temp.text = "";
        }
    }
}
