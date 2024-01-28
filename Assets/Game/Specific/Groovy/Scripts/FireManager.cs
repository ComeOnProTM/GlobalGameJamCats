using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance;

    private List<GameObject> fireList;
    private float waitTimer;
    private float waitTimerMax = 1f;
    private bool canCheckFire = false;


    private void Awake()
    {
        Instance = this;
        fireList = new List<GameObject>();
        waitTimer = waitTimerMax;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying() && !canCheckFire)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer < 0 ) 
            {
                canCheckFire = true;
            }
        }
        if (GameManager.Instance.IsGamePlaying() && fireList.Count == 0 && canCheckFire)
        {
            GameManager.Instance.FinishGame(GameManager.EndCondition.FireOut);
        }
    }

    public void AddFire(GameObject _fireObject)
    {
        fireList.Add(_fireObject);
    }

    public void RemoveFire(GameObject _fireObject)
    {
        fireList.Remove(_fireObject);
    }
}
