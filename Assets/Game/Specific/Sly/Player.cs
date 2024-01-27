using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Player")]
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private Rigidbody2D rb;
    private bool isDashing;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private DashState dashState;
    [SerializeField] private float maxDash = 0.8f;
    private float dashTimer;
    private Vector2 savedVelocity;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
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

        switch (dashState)
        {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashKeyDown)
                {
                    savedVelocity = inputVector;
                    rb.velocity = new Vector2 (inputVector.x * 3f * Time.deltaTime, inputVector.y * 3f * Time.deltaTime);
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;
                isDashing = true;
                if (dashTimer >= maxDash)
                {
                    //dashTimer = maxDash;
                    //rb.velocity = savedVelocity;
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                isDashing = false;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }

        if (isDashing)
        {
            // Move the player in the dash direction with dash speed
            Vector3 moveDir = new Vector3(savedVelocity.x, savedVelocity.y, 0);
            transform.position += moveDir * dashSpeed * Time.deltaTime;
        }
        else
        {
            Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
            transform.position += moveDir * movementSpeed * Time.deltaTime;
        }
    }
        
    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
}