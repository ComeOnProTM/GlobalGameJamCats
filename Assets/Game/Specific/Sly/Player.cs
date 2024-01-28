using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static Player;
using Unity.Profiling;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public event EventHandler<OnDashStateChangedEventArgs> OnDashStateChanged;
    public event EventHandler<OnSmallDashStateChangedEventArgs> OnSmallDashStateChanged;
    public class OnDashStateChangedEventArgs : EventArgs
    {
        public DashState eventDashState;
    }
    public class OnSmallDashStateChangedEventArgs : EventArgs
    {
        public SmallDashState eventSmallDashState;
    }

    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HealthBar healthBar;

    [Header("Movement")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private float movementSpeed;
    private bool isDashing = false;
    private bool isSmallDashing = false;
    private bool isWalking = false;
    private float radius = 1;
    [SerializeField] private LayerMask fireLayer;
    [SerializeField] private LayerMask NPCLayer;


    [Header("Dash")]
    [SerializeField] private DashState dashState;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashMaxTime = 0.8f;
    [SerializeField] private float dashCooldown = 2f;
    private float dashTimer;
    private Vector2 savedVelocity;

    [Header("SmallDash")]
    [SerializeField] private SmallDashState smallDashState;
    [SerializeField] private float smallDashSpeed = 15f;
    [SerializeField] private float smallDashMaxTime = 0.8f;
    [SerializeField] private float smallDashCooldown = 2f;
    private float smallDashTimer;

    [Header("Health")]
    [SerializeField] private int healthBackingField = 20;
    [SerializeField] private float damageTimerMax = 1.275f;
    private bool damageTick;
    private float damageTimer;
    private float healthSpeedModifier;
    [SerializeField] private float damageSpeedModifier = 1f;
    [SerializeField] private float healSpeedModifier = 3f;
    private float currentHealth;

    public int Health 
    {
        get => healthBackingField;
        set 
        {
            healthBackingField = clamp(value, 0, maxHealth);
        }
    }

    public int maxHealth;

    public float HealthPrimantissa => (float)Health / (float)maxHealth;

    public enum PlayerState
    {
        Idle,
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

    public enum SmallDashState
    {
        SmallReady,
        SmallDashing,
        SmallCooldown,
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.OnInteractAction += InputManager_OnInteractAction;
        InputManager.Instance.OnAlternateInteractAction += InputManager_OnAlternateInteractAction;
        damageTimer = damageTimerMax;
        currentHealth = maxHealth;
    }

    private void InputManager_OnAlternateInteractAction(object sender, EventArgs e)
    {
        Debug.Log("Small Fire");
        if (smallDashState == SmallDashState.SmallReady)
        {
            savedVelocity = InputManager.Instance.GetMovementVectorNormalized();
            //rb.velocity = new Vector2(inputVector.x * 3f * Time.deltaTime, inputVector.y * 3f * Time.deltaTime);
            smallDashState = SmallDashState.SmallDashing;
            OnSmallDashStateChanged?.Invoke(this, new OnSmallDashStateChangedEventArgs
            {
                eventSmallDashState = smallDashState,
            });
        }
    }

    private void InputManager_OnInteractAction(object sender, EventArgs e)
    {
        if (dashState == DashState.Ready)
        {
            savedVelocity = InputManager.Instance.GetMovementVectorNormalized();
            //rb.velocity = new Vector2(inputVector.x * 3f * Time.deltaTime, inputVector.y * 3f * Time.deltaTime);
            dashState = DashState.Dashing;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs
            {
                eventDashState = dashState,
            });
        }
    }

    void Update()
    {
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0);
        transform.position += moveDir * movementSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        switch (dashState)
        {
            case DashState.Ready:
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime;
                isDashing = true;
                if (dashTimer >= dashMaxTime)
                {
                    //dashTimer = maxDash;
                    //rb.velocity = savedVelocity;
                    dashState = DashState.Cooldown;
                    OnSmallDashStateChanged?.Invoke(this, new OnSmallDashStateChangedEventArgs
                    {
                        eventSmallDashState = smallDashState,
                    });
                }
            break;
            case DashState.Cooldown:
                dashTimer += Time.deltaTime;
                isDashing = false;
                if (dashTimer >= dashCooldown)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                    OnSmallDashStateChanged?.Invoke(this, new OnSmallDashStateChangedEventArgs
                    {
                        eventSmallDashState = smallDashState,
                    });
                }
            break;
        }

        switch (smallDashState)
        {
            case SmallDashState.SmallReady:
                break;
            case SmallDashState.SmallDashing:
                smallDashTimer += Time.deltaTime;
                isSmallDashing = true;
                if (smallDashTimer >= smallDashMaxTime)
                {
                    //dashTimer = maxDash;
                    //rb.velocity = savedVelocity;
                    smallDashState = SmallDashState.SmallCooldown;
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs
                    {
                        eventDashState = dashState,
                    });
                }
                break;
            case SmallDashState.SmallCooldown:
                smallDashTimer += Time.deltaTime;
                isSmallDashing = false;
                if (smallDashTimer >= smallDashCooldown)
                {
                    smallDashTimer = 0;
                    smallDashState = SmallDashState.SmallReady;
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
            PushNpc();
            moveDir = new Vector3(savedVelocity.x, savedVelocity.y, 0);
            MovePlayer(moveDir, dashSpeed);

            // transform.position += moveDir * dashSpeed * Time.deltaTime;
            //rb.AddForce(new Vector2(savedVelocity.x * dashSpeed, savedVelocity.y * dashSpeed));
            //rb.MovePosition(rb.position + new Vector2(moveDir.x * 2f, moveDir.y * 2f) * Time.deltaTime);
            transform.position += moveDir * dashSpeed * Time.deltaTime;
        }
        if (isSmallDashing)
        {
            // Move the player in the dash direction with dash speed
            Extinguish();
            PushNpc();
            moveDir = new Vector3(savedVelocity.x, savedVelocity.y, 0);
            MovePlayer(moveDir, smallDashSpeed);

            // transform.position += moveDir * dashSpeed * Time.deltaTime;
            //rb.AddForce(new Vector2(savedVelocity.x * dashSpeed, savedVelocity.y * dashSpeed));
            //rb.MovePosition(rb.position + new Vector2(moveDir.x * 2f, moveDir.y * 2f) * Time.deltaTime);
            transform.position += moveDir * dashSpeed * Time.deltaTime;
        }
        else
        {
            MovePlayer(moveDir, movementSpeed);
        }

        if (damageTick)
        {
            damageTimer -= Time.deltaTime * healthSpeedModifier;
            if (damageTimer < 0)
            {
                damageTick = false;
                damageTimer = damageTimerMax;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, fireLayer);
        Debug.Log(hitColliders.Length);
        if (hitColliders.Length > 0)
        {
            foreach (Collider _collider in hitColliders)
            {
                if (!damageTick)
                {
                    if (_collider.TryGetComponent<SmallFireScript>(out SmallFireScript _))
                    {
                        currentHealth -= 1;
                        healthBar.AdjustHealth(-1);
                        damageTick = true;
                        healthSpeedModifier = damageSpeedModifier;
                    }
                    else if (currentHealth < maxHealth)
                    {
                        currentHealth += 1;
                        healthBar.AdjustHealth(+1);
                        damageTick = true;
                        healthSpeedModifier = healSpeedModifier;
                    }
                }
            }
        }
    }

    void Extinguish()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, radius, fireLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<SmallFireScript>(out SmallFireScript fire))
            {
                Destroy(fire);
            }
        }
    }

    private void PushNpc()
    {
        //check if dashing x
        // check collision with NPC
        Collider2D[] collisionNpc = Physics2D.OverlapCircleAll(transform.position, radius, NPCLayer);

        for (int i = 0; i < collisionNpc.Length; i++) 
        {
            if (collisionNpc[i].gameObject.TryGetComponent<Civilian>(out Civilian _NPC))
            {
                Debug.Log("NPC");
            }
        }

        // npc movement = player movement
    }

    public void MovePlayer(Vector3 _moveDir, float _speed)
    {
        transform.position += _moveDir * _speed * Time.deltaTime;
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
