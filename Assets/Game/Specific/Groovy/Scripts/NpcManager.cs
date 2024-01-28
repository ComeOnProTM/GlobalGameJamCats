using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager Instance;

    public event EventHandler OnNpcSaved;
    
    private List<GameObject> npcList;
    private float waitTimer;
    private float waitTimerMax = 1f;
    private bool canCheckNPC = false;
    private int npcSaved;
    private int npcTotal;

    private void Awake()
    {
        Instance = this;
        npcList = new List<GameObject>();
        waitTimer = waitTimerMax;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying() && !canCheckNPC)
        {
            npcTotal = npcList.Count;
            waitTimer -= Time.deltaTime;
            if (waitTimer < 0)
            {
                canCheckNPC = true;
            }
        }
        if (GameManager.Instance.IsGamePlaying() && npcList.Count == 0 && canCheckNPC)
        {
            GameManager.Instance.FinishGame(GameManager.EndCondition.CivSaved);
        }
    }

    public void AddNPC(GameObject _NPC)
    {
        npcList.Add(_NPC);
    }

    public void RemoveNPC(GameObject _NPC)
    {
        npcList.Remove(_NPC);
    }

    public void SuccesfulSaveNPC()
    {
        npcSaved += 1;
        OnNpcSaved?.Invoke(this, EventArgs.Empty);
    }

    public float GetTotalNPC()
    {
        return npcList.Count;
    }

    public float GetSavedNPC()
    {
        return npcSaved;
    }
}
