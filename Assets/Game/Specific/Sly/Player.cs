using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float speed;
    [SerializeField] private float dashSpeed;
    private bool isDashing = false;

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