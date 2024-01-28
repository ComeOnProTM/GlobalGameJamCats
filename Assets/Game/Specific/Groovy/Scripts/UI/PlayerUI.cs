using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject baseUI;
    [SerializeField] private Text CivCountText;
    [SerializeField] private Text TimerText;
    private float npcTotal;
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    
    private void Start()
    {
        DisplayTime(timeRemaining - 1f);
        npcTotal = NpcManager.Instance.GetTotalNPC();
        string _tempString = "0/" + npcTotal.ToString();
        Debug.Log(_tempString);
        CivCountText.text = _tempString;
        NpcManager.Instance.OnNpcSaved += NpcManager_OnNpcSaved;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        // Starts the timer automatically
        timerIsRunning = true;
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        npcTotal = NpcManager.Instance.GetTotalNPC();
        CivCountText.text = "0/" + npcTotal.ToString(); ;
    }

    private void NpcManager_OnNpcSaved(object sender, System.EventArgs e)
    {
        Debug.Log(NpcManager.Instance.GetSavedNPC().ToString());
        CivCountText.text = NpcManager.Instance.GetSavedNPC().ToString() + "/" + npcTotal.ToString();
    }

    void Update()
    {
        if (timerIsRunning && GameManager.Instance.IsGamePlaying())
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                GameManager.Instance.FinishGame(GameManager.EndCondition.TimerFail);
            }
        }
    }
    
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Show()
    {
        baseUI.SetActive(true);
    }

    private void Hide()
    {
        baseUI.SetActive(false);
    }
}
