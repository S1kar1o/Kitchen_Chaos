using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private const string PLAYER_PREFS_BUIDING = "InputBuidings";
    public static InputSystem Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    private PlayerInputAction playerInputAction;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternative,
        Pause,
        GamePad_Interact,
        GamePad_Interact_Alternative,
        GamePad_Pause
    }

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();


        if (PlayerPrefs.HasKey(PLAYER_PREFS_BUIDING))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BUIDING));
        }

        playerInputAction.Player.Enable();
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

    }
    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 currentPosition = playerInputAction.Player.Move.ReadValue<Vector2>();

        currentPosition = currentPosition.normalized;
        return currentPosition;
    }
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case (Binding.Move_Up):
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case (Binding.Move_Down):
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case (Binding.Move_Left):
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case (Binding.Move_Right):
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case (Binding.Interact):
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case (Binding.Interact_Alternative):
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case (Binding.Pause):
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();


            case (Binding.GamePad_Interact):
                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case (Binding.GamePad_Interact_Alternative):
                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case (Binding.GamePad_Pause):
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();
        }
    }
    public void ReBinding(Binding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int buindingIndex;
        switch (binding)
        {
            default:
            case (Binding.Move_Up):
                inputAction = playerInputAction.Player.Move;
                buindingIndex = 1;
                break;
            case (Binding.Move_Down):
                inputAction = playerInputAction.Player.Move;
                buindingIndex = 2;
                break;
            case (Binding.Move_Left):
                inputAction = playerInputAction.Player.Move;
                buindingIndex = 3;
                break;
            case (Binding.Move_Right):
                inputAction = playerInputAction.Player.Move;
                buindingIndex = 4;
                break;
            case (Binding.Interact):
                inputAction = playerInputAction.Player.Interact;
                buindingIndex = 0;
                break;
            case (Binding.Interact_Alternative):
                inputAction = playerInputAction.Player.InteractAlternate;
                buindingIndex = 0;
                break;
            case (Binding.Pause):
                inputAction = playerInputAction.Player.Pause;
                buindingIndex = 0;
                break;
            case (Binding.GamePad_Pause):
                inputAction = playerInputAction.Player.Pause;
                buindingIndex = 1;
                break;
            case (Binding.GamePad_Interact):
                inputAction = playerInputAction.Player.Interact;
                buindingIndex = 1;
                break;
            case (Binding.GamePad_Interact_Alternative):
                inputAction = playerInputAction.Player.InteractAlternate;
                buindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(buindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputAction.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BUIDING, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();

    }
}
