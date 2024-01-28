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
        InputManager.Instance.OnPlayerStateChanged += InputManager_OnPlayerStateChanged;
    }

    private void InputManager_OnPlayerStateChanged(object sender, InputManager.OnPlayerStateChangedEventArgs e)
    {
        Debug.Log(e.eventPlayerState);
        switch (e.eventPlayerState)
        {
            case Player.PlayerState.Idle:
                anim.SetBool(IDLE, true);
                anim.SetBool(WALK, false);
                break;
            case Player.PlayerState.Up:
                anim.SetBool(WALK, true);
                anim.SetBool(IDLE, false);
                direction = Direction.Up;
                break;
            case Player.PlayerState.Down:
                anim.SetBool(WALK, true);
                anim.SetBool(IDLE, false);
                direction = Direction.Down;
                break;
            case Player.PlayerState.Left:
                anim.SetBool(WALK, true);
                anim.SetBool(IDLE, false);
                direction = Direction.Left;
                spriteRenderer.flipX = true;
                break;
            case Player.PlayerState.Right:
                anim.SetBool(WALK, true);
                anim.SetBool(IDLE, false);
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

        }
    }
}
