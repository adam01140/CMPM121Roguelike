using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RewardScreenManager : MonoBehaviour
{
    //Should probalbly refactor into seperate manager script but this works for now since it's not that many objects
    public GameObject rewardUI;
    public TextMeshProUGUI buttonText;
    public GameObject changeSpellButton;
    public GameObject changeModButton;
    public GameObject addModButton;

    public GameObject relicButton;

    public GameObject relicIcon1;
    public TMP_Text relicText1;
    public GameObject relicIcon2;
    public TMP_Text relicText2;

    public GameObject relicButton1;
    public GameObject relicButton2;

    private bool relicsSet;

    void Start()
    {
        rewardUI.SetActive(false);
        relicsSet = false;
        relicButton1 = Instantiate(relicButton, rewardUI.transform);
        relicButton1.transform.localPosition = new Vector3(60, -90);
        relicButton1.GetComponent<RelicButtonController>().manager = this;

        relicButton2 = Instantiate(relicButton, rewardUI.transform);
        relicButton2.transform.localPosition = new Vector3(220, -90);
        relicButton2.GetComponent<RelicButtonController>().manager = this;
        ClearRelicOptions();
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
                if (GameManager.Instance.wave % 3 == 0 && !relicsSet)
                {
                    RelicManager relicManager = new RelicManager();
                    SetRelicOptions(relicManager.genRelicSelection());
                    relicsSet = true;
                }
            }

        }
        else
        {
            rewardUI.SetActive(false);
            relicsSet = false;

        }
    }

    public void SetRelicOptions(List<Relic> relicSelection)
    {
        if (relicSelection.Count > 0)
        {
            relicIcon1.SetActive(true);
            relicText1.gameObject.SetActive(true);
            relicButton1.SetActive(true);
            relicButton1.GetComponent<RelicButtonController>().player = GameManager.Instance.player.GetComponent<PlayerController>();
            relicText1.text = relicSelection[0].description;
            relicButton1.GetComponent<RelicButtonController>().relic = relicSelection[0];
            GameManager.Instance.relicIconManager.PlaceSprite(relicSelection[0].SpriteId, relicIcon1.GetComponent<Image>());
        }
        if (relicSelection.Count > 1)
        {
            relicIcon2.SetActive(true);
            relicText2.gameObject.SetActive(true);
            relicButton2.SetActive(true);
            relicButton2.GetComponent<RelicButtonController>().player = GameManager.Instance.player.GetComponent<PlayerController>();
            relicText2.text = relicSelection[1].description;
            relicButton2.GetComponent<RelicButtonController>().relic = relicSelection[1];
            GameManager.Instance.relicIconManager.PlaceSprite(relicSelection[1].SpriteId, relicIcon2.GetComponent<Image>());
        }
    }
    public void ClearRelicOptions()
    {
        relicIcon1.SetActive(false);
        relicText1.gameObject.SetActive(false);
        relicButton1.SetActive(false);
        relicIcon2.SetActive(false);
        relicText2.gameObject.SetActive(false);
        relicButton2.SetActive(false);
    }


}
