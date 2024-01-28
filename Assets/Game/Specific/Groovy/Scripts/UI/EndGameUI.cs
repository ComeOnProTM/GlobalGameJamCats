using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject endGameUI;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameEnd())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        endGameUI.SetActive(true);
    }

    private void Hide()
    {
        endGameUI.SetActive(false);
    }
}
