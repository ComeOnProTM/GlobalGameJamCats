using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private LayerMask fireLayer;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);

        if (collision.gameObject == player.gameObject)
        {
            Debug.Log("lol");
        }
    }
}
