using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternateInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler<OnPlayerStateChangedEventArgs> OnPlayerStateChanged;
    public class OnPlayerStateChangedEventArgs : EventArgs
    {
        public PlayerState eventPlayerState;
    }

    private PlayerInputActions playerInputActions;

    private bool isPaused;

    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.AlternateInteract.performed += AlternateInteract_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
        TogglePause();
    }

    private void AlternateInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnAlternateInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        if (!Player.Instance.GetIsDashing() && GameManager.Instance.IsGamePlaying())
        {
            // Regular movement based on keyboard input
            Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            if (inputVector.x == 0 && inputVector.y == 0)
            {
                OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
                {
                    eventPlayerState = PlayerState.Idle,
                });
            }
            if (inputVector.x > 0)
            {
                OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
                {
                    eventPlayerState = PlayerState.Right,
                });
            }
            if (inputVector.x < 0)
            {
                OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
                {
                    eventPlayerState = PlayerState.Left,
                });
            }
            if (inputVector.y > 0)
            {
                OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
                {
                    eventPlayerState = PlayerState.Up,
                });
            }
            if (inputVector.y < 0)
            {
                OnPlayerStateChanged?.Invoke(this, new OnPlayerStateChangedEventArgs
                {
                    eventPlayerState = PlayerState.Down,
                });
            }

            return inputVector;
        }
        return new Vector2(0, 0);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
