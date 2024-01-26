using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField]
    public float timeRemaining;
    public bool timerIsRunning;
    public Text timerText;

    private void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning == true)
        {
            if (timeRemaining > 0)
            {
                //Count down time
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                //Do things when time runs out
                Debug.Log("Time has run out!");
                timerIsRunning = false;
            }
        }

        timerText.text = timeRemaining.ToString("0");
    }
}
