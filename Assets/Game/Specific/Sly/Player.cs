using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler<OnPlayerStateChangedEventArgs> OnPlayerStateChanged;
    public event EventHandler<OnDashStateChangedEventArgs> OnDashStateChanged;
    public class OnPlayerStateChangedEventArgs : EventArgs
    {
        public PlayerState eventPlayerState;
    }
    public class OnDashStateChangedEventArgs : EventArgs
    {
        public DashState eventDashState;
    }

    [Header("Movement")]
    [SerializeField] private PlayerState playerState;
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

    [SerializeField]
    private Rigidbody2D rb;

    public enum PlayerState
    {
        Up,
        Down,
        Left,
        Right,
    }

    public enum DashState
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
            playerState = PlayerState.Up;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
            {
                eventPlayerState = playerState,
            });
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
            playerState = PlayerState.Left;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
            {
                eventPlayerState = playerState,
            });
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
            playerState = PlayerState.Down;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
            {
                eventPlayerState = playerState,
            });
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
            playerState = PlayerState.Right;
            OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
            {
                eventPlayerState = playerState,
            });
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
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs
                    {
                        eventDashState = dashState,
                    });
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
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs
                    {
                        eventDashState = dashState,
                    });
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                isDashing = false;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs
                    {
                        eventDashState = dashState,
                    });
                }
                break;
        }

        if (isDashing)
        {
            // Move the player in the dash direction with dash speed
            Extinguish();
            //moveDir = new Vector3(savedVelocity.x, savedVelocity.y, 0);

            // transform.position += moveDir * dashSpeed * Time.deltaTime;
            rb.AddForce(new Vector2(moveDir.x, moveDir.y) * dashSpeed);
            //rb.MovePosition(rb.position + new Vector2(moveDir.x * 2f, moveDir.y * 2f) * Time.deltaTime);
            //transform.position += moveDir * dashSpeed * Time.deltaTime;
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

    public PlayerState GetPlayerState()
    {
        return playerState;
    }

    public DashState GetDashState()
    {
        return dashState;
    }

    public bool GetIsDashing()
    {
        return isDashing;
    }
}
