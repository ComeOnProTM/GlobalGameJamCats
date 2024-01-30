using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    //FIRE OUT IS GENERIC END NOW, GOD HELP US
    [SerializeField] private GameObject fireOutEndGameUI;
    [SerializeField] private GameObject timerOutEndGameUI;
    [SerializeField] private GameObject civSavedEndGameUI;
    [SerializeField] private GameObject slideOut;

    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Text timeText;
    [SerializeField] private float finalTime;

    [SerializeField] private Button mainMenu;

    private void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameEnd())
        {
            switch (GameManager.Instance.GetEndCondition())
            {
                case GameManager.EndCondition.FireOut:
                    ShowFireOut();
                    break;
                case GameManager.EndCondition.CivSaved:
                    ShowCivSaved();
                    break;
                case GameManager.EndCondition.TimerFail:
                    ShowTimerOut();
                    break;
            }
        }
        else
        {
            Hide();
        }
    }

    private void ShowFireOut()
    {
        finalTime = playerUI.maxTime - playerUI.timeRemaining;
        timeText.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);
        mainMenu.onClick.AddListener(() => slideOut.SetActive(true));
        //timeText.text = finalTime.ToString("0");
        float minutes = Mathf.FloorToInt(finalTime / 60);
        float seconds = Mathf.FloorToInt(finalTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        fireOutEndGameUI.SetActive(true);
    }

    private void ShowCivSaved()
    {
        finalTime = playerUI.maxTime - playerUI.timeRemaining;
        timeText.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);
        mainMenu.onClick.AddListener(() => slideOut.SetActive(true));
        //timeText.text = finalTime.ToString("0");
        float minutes = Mathf.FloorToInt(finalTime / 60);
        float seconds = Mathf.FloorToInt(finalTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        civSavedEndGameUI.SetActive(true);
    }

    private void ShowTimerOut()
    {
        timerOutEndGameUI.SetActive(true);
    }

    private void Hide()
    {
        fireOutEndGameUI.SetActive(false);
        timerOutEndGameUI.SetActive(false);
    }
}
