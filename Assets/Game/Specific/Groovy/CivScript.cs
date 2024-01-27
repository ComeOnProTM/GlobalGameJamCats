using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (Player.Instance.GetIsDashing())
        {
            //Civilian Pushing script here
            Debug.Log("Code fired when dashing into object");
        }
    }
}
