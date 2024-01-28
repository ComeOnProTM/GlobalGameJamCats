using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string IDLE = "idle";
    private const string WALK = "walk";
    private const string PUNCH = "punch";
    private const string FRONT = "front";
    private const string SIDE = "side";
    private const string BACK = "back";

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject waterParticles;
    [SerializeField] private Transform waterTransform;

    private Direction direction;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private void Start()
    {
        Player.Instance.OnDashStateChanged += Player_OnDashStateChanged;
        Player.Instance.OnSmallDashStateChanged += Player_OnSmallDashStateChanged;
        InputManager.Instance.OnPlayerStateChanged += InputManager_OnPlayerStateChanged;
    }

    private void Player_OnSmallDashStateChanged(object sender, Player.OnSmallDashStateChangedEventArgs e)
    {
        Debug.Log(e.eventSmallDashState);
        switch (e.eventSmallDashState)
        {
            case Player.SmallDashState.SmallReady:
                anim.SetBool(PUNCH, false);
                break;
            case Player.SmallDashState.SmallDashing:
                anim.SetBool(PUNCH, true);
                Debug.Log("WaterSpawn");
                Instantiate(waterParticles, waterTransform);
                break;
            case Player.SmallDashState.SmallCooldown:
                anim.SetBool(PUNCH, false);
                break;
        }
    }

    private void InputManager_OnPlayerStateChanged(object sender, InputManager.OnPlayerStateChangedEventArgs e)
    {
        switch (e.eventPlayerState)
        {
            case Player.PlayerState.Idle:
                if (!Player.Instance.GetIsDashing())
                {
                    anim.SetBool(IDLE, true);
                    anim.SetBool(WALK, false);
                }
                break;
            case Player.PlayerState.Up:
                if (!Player.Instance.GetIsDashing())
                {
                    anim.SetBool(IDLE, false);
                    anim.SetBool(WALK, true);
                }
                direction = Direction.Up;
                break;
            case Player.PlayerState.Down:
                if (!Player.Instance.GetIsDashing())
                {
                    anim.SetBool(IDLE, false);
                    anim.SetBool(WALK, true);
                }
                direction = Direction.Down;
                break;
            case Player.PlayerState.Left:
                if (!Player.Instance.GetIsDashing())
                {
                    anim.SetBool(IDLE, false);
                    anim.SetBool(WALK, true);
                }
                direction = Direction.Left;
                spriteRenderer.flipX = true;
                break;
            case Player.PlayerState.Right:
                if (!Player.Instance.GetIsDashing())
                {
                    anim.SetBool(IDLE, false);
                    anim.SetBool(WALK, true);
                }
                direction = Direction.Right;
                break;
        }

        if (e.eventPlayerState != Player.PlayerState.Idle)
        {
            if (e.eventPlayerState != Player.PlayerState.Left)
            {
                spriteRenderer.flipX = false;
            }
            switch (direction)
            {
                case Direction.Up:
                    anim.SetBool(FRONT, false);
                    anim.SetBool(BACK, true);
                    anim.SetBool(SIDE, false);
                break;
                case Direction.Down:
                    anim.SetBool(BACK, false);
                    anim.SetBool(FRONT, true);
                    anim.SetBool(SIDE, false);
                break;
                case Direction.Left:
                    anim.SetBool(SIDE, true);
                    anim.SetBool(BACK, false);
                    anim.SetBool(FRONT, false);
                    spriteRenderer.flipX = true;
                    break;
                case Direction.Right:
                    anim.SetBool(SIDE, true);
                    anim.SetBool(BACK, false);
                    anim.SetBool(FRONT, false);
                break;
            }
        }
    }

    private void Player_OnDashStateChanged(object sender, Player.OnDashStateChangedEventArgs e)
    {
        switch (e.eventDashState)
        {
            case Player.DashState.Ready:
                anim.SetBool(PUNCH, false);
                break;
            case Player.DashState.Dashing:
                anim.SetBool(PUNCH, true);
            break;
            case Player.DashState.Cooldown:
                anim.SetBool(PUNCH, false);
                break;
        }
    }
}
