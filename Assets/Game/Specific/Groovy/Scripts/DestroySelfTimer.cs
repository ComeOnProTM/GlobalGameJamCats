using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfTimer : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timerMax = 2f;
    private float currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
