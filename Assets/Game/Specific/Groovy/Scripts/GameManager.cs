using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnStateChanged;

    public enum GameState
    {
        GameStart,
        GamePlaying,
        GameEnd,
    }

    public enum EndCondition
    {
        FireOut,
        CivSaved,
        TimerFail,
    }

    [Header("Attributes")]
    [SerializeField] private GameState gameState;
    private EndCondition endCondition;

    private float timerStart;
    [SerializeField]private float timerStartMax = 3f;

    private void Awake()
    {
        Instance = this;
    }

 [SerializeField]private AudioClip musicClip;
public class AudioPlayer : MonoBehaviour
{
    // Reference to the AudioSource component
    private AudioSource audioSource;

    // The audio clip to be played
    private AudioClip musicClip;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // Set the audio clip to be played
        audioSource.clip = musicClip;
    }

    // Function to play the audio when triggered
    public void PlayAudio()
    {
        // Check if the AudioSource and AudioClip are set
        if (audioSource != null && musicClip != null)
        {
            // Play the audio
            audioSource.Play();
        }
        else
        {
            // Log an error if AudioSource or AudioClip is not set
            Debug.LogError("AudioSource or AudioClip is not set!");
        }
    }
}


private void Start()
    {
        timerStart = timerStartMax;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.GameStart:
                timerStart -= Time.deltaTime;
                if (timerStart < 0)
                {
                    gameState = GameState.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GamePlaying:

                break;
            case GameState.GameEnd:

                break;
        }
    }

    public void FinishGame(EndCondition _endCondition)
    {
        gameState = GameState.GameEnd;
        endCondition = _endCondition;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameStart()
    {
        return gameState == GameState.GameStart;
    }

    public bool IsGamePlaying() 
    { 
        return gameState == GameState.GamePlaying;
    }
    
    public bool IsGameEnd() 
    {  
        return gameState == GameState.GameEnd;
    }

    public float GetStartTimer()
    {
        return timerStart;
    }

    public EndCondition GetEndCondition()
    {
        return endCondition;
    }
}
