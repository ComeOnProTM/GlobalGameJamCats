using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameUI : MonoBehaviour

    
{
    private const string LEVELONE = "CasinoScene";
    [SerializeField] private GameObject fireOutEndGameUI;
    [SerializeField] private GameObject civSavedEndGameUI;
    [SerializeField] private GameObject timerOutEndGameUI;

    [Header("References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        playButton.onClick.AddListener(() => SceneManager.LoadScene(LEVELONE));
        quitButton.onClick.AddListener(() => Application.Quit());
        playButton.Select();
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
        fireOutEndGameUI.SetActive(true);
    }

    private void ShowCivSaved()
    {
        civSavedEndGameUI.SetActive(true);
    }

    private void ShowTimerOut()
    {
        timerOutEndGameUI.SetActive(true);
    }

    private void Hide()
    {
        fireOutEndGameUI.SetActive(false);
        civSavedEndGameUI.SetActive(false);
        timerOutEndGameUI.SetActive(false);
    }
}
