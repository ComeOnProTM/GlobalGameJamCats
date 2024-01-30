using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    private float currentTimer;
    private float timerMax = 0.5f;

    private void Start()
    {
        currentTimer = timerMax;
    }

    private void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0)
        {
            loadMainScene();
        }
    }

    public void loadMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
