using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowBreakable : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        // Check if the collider belongs to a specific GameObject or has a specific tag
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Player.DashState dashingState = other.gameObject.GetComponent<Player>().GetDashState();
            if(dashingState == Player.DashState.Dashing)
            {
                // Perform actions while the "Player" GameObject stays inside the trigger
                Debug.Log("Player is inside the trigger!");
            }
        }
    }
}
