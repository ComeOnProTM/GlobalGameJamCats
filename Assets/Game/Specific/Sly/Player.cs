using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;
    private bool isDashing = false;
    private float radius = 1;
    public LayerMask layermask = 1 << 6;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isDashing)
        {
            // Calculate dash direction based on mouse position
            Vector3 dashDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

            // Move the player in the dash direction with dash speed
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
        }

    //private void OnTriggerEnter(Collider col)
    //{
    //    Debug.Log("test");
    //    if (isDashing)
    //    {
    //        Debug.Log("taste: " + col + "  : " +  col.gameObject + "  : " + col.gameObject.GetComponent<SmallFireScript>());

    //        if (col.gameObject.TryGetComponent<SmallFireScript>(out SmallFireScript fire))
    //        {
    //            Debug.Log("Lol");
    //            Destroy(fire)
    //            var hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);
    //        }
    //    }
    //}

        // Regular movement based on keyboard input
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W) && !isDashing)
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A) && !isDashing)
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S) && !isDashing)
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D) && !isDashing)
        {
            inputVector.x = 1;
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += moveDir * speed * Time.deltaTime;

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);

        // Dash input (you can change this condition based on your input setup)
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }
    

    IEnumerator Dash()
    {

        Extinguish();
        Debug.Log("Dashed");
        isDashing = true;

        // Wait for a short duration for the dash
        yield return new WaitForSeconds(0.2f);

        isDashing = false;
    }
 
    public bool IsDashing()
    {
        return isDashing;
    }
}
void Extinguish()
    {

        var hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);

        foreach (var hitCollider in hitColliders) { 
            if (hitCollider.gameObject.TryGetComponent<SmallFireScript>(out SmallFireScript fire))
        {
            Destroy(fire);
            }
        }
    }
}