using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject fireOutEndGameUI;
    [SerializeField] private GameObject civSavedEndGameUI;
    [SerializeField] private GameObject timerOutEndGameUI;

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
