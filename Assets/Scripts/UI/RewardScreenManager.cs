using UnityEngine;
using TMPro;

public class RewardScreenManager : MonoBehaviour
{
    //Should probalbly refactor into seperate manager script but this works for now since it's not that many objects
    public GameObject rewardUI;
    public TextMeshProUGUI buttonText;
    public GameObject changeSpellButton;
    public GameObject changeModButton;
    public GameObject addModButton;
    void Start()
    {
        rewardUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND || GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            rewardUI.SetActive(true);
            if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
            {
                buttonText.text = "Try Again?";
                changeSpellButton.SetActive(false);
                changeModButton.SetActive(false);
                addModButton.SetActive(false);

            }
            else if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
            {
                changeSpellButton.SetActive(true);
                changeModButton.SetActive(true);
                addModButton.SetActive(true);

            }

        }
        else
        {
            rewardUI.SetActive(false);

        }
    }


}
