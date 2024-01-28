using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        GameStart,
        GamePlaying,
        GameEnd,
    }

    [Header("Attributes")]
    [SerializeField] private GameState gameState;
    private bool isPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.OnPauseAction += InputManager_OnPauseAction;
    }

    private void InputManager_OnPauseAction(object sender, System.EventArgs e)
    {
        TogglePause();
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.GameStart:

                break;
            case GameState.GamePlaying:

                break;
            case GameState.GameEnd:

                break;
        }
    }

    private void TogglePause()
    {
        Debug.Log("Pause");
        isPaused = !isPaused;
        if (isPaused )
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
