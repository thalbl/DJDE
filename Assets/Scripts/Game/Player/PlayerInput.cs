using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputActions;

    // Public accessors to the player's actions
    public InputAction Interact => this.inputActions.Player.Interact;
    public InputAction OpenMap => this.inputActions.Player.OpenMap;
    public InputAction OpenPortfolio => this.inputActions.Player.OpenPortfolio;
    public InputAction OpenInventory => this.inputActions.Player.OpenInventory;
    
    public InputAction Left  => this.inputActions.Player.Left;
    public InputAction Right => this.inputActions.Player.Right;
    public InputAction Up    => this.inputActions.Player.Up;
    public InputAction Down  => this.inputActions.Player.Down;

    void Awake() => this.inputActions = new PlayerInputActions();
    private void OnEnable() => this.inputActions.Enable();
    private void OnDisable() => this.inputActions.Disable();
}
