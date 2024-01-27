using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashSpeed = 15f;
    private bool isDashing = false;
    private float radius = 1;
    public LayerMask layermask = 1 << 6;

    [Header("Dash")]
    [SerializeField] private DashState dashState;
    [SerializeField] private float dashMaxTime = 0.8f;
    private float dashTimer;
    private Vector2 savedVelocity;

    private enum DashState
    {
        Ready,
        Dashing,
        Cooldown,
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {

        // Regular movement based on keyboard input
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += moveDir * movementSpeed * Time.deltaTime;

        switch (dashState)
        {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashKeyDown /*&& inputVector.x != 0 && inputVector.y != 0*/)
                {
                    savedVelocity = inputVector;
                    //rb.velocity = new Vector2(inputVector.x * 3f * Time.deltaTime, inputVector.y * 3f * Time.deltaTime);
                    dashState = DashState.Dashing;
                }
            break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;
                isDashing = true;
                if (dashTimer >= dashMaxTime)
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
            Extinguish();
            moveDir = new Vector3(savedVelocity.x, savedVelocity.y, 0);
            transform.position += moveDir * dashSpeed * Time.deltaTime;
        }
        else
        {
            moveDir = new Vector3(inputVector.x, inputVector.y, 0);
            transform.position += moveDir * movementSpeed * Time.deltaTime;
        }
    }

        void Extinguish()
        {

            var hitColliders = Physics.OverlapSphere(transform.position, radius, layermask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.TryGetComponent<SmallFireScript>(out SmallFireScript fire))
                {
                    Destroy(fire);
                }
            }
        }
    


}
            
    