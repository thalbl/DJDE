using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerStartingState : PlayerBaseState
{
    public PlayerStartingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        this.stateMachine.Input.Interact.performed      += Move;
        this.stateMachine.Input.OpenMap.performed       += OpenMap;
        this.stateMachine.Input.OpenPortfolio.performed += OpenPortfolio;
        this.stateMachine.Input.OpenInventory.performed += OpenInventory;
        
        PlayerEvents.OnTurnStart?.Invoke(this.stateMachine.Player);
    }

    public override void Exit()
    {
        this.stateMachine.Input.Interact.performed      -= Move;
        this.stateMachine.Input.OpenMap.performed       -= OpenMap;
        this.stateMachine.Input.OpenPortfolio.performed -= OpenPortfolio;
        this.stateMachine.Input.OpenInventory.performed -= OpenInventory;
    }

    public override void Update()
    {
        // Player animation, etc
    }

    private void Move(InputAction.CallbackContext context) => this.stateMachine.SwitchState(this.stateMachine.Moving());

    private void OpenMap(InputAction.CallbackContext context)       => PlayerEvents.OnOpenMap.Invoke(this.stateMachine.Player);
    private void OpenPortfolio(InputAction.CallbackContext context) => PlayerEvents.OnOpenPortfolio.Invoke(this.stateMachine.Player);
    private void OpenInventory(InputAction.CallbackContext context) => PlayerEvents.OnOpenInventory.Invoke(this.stateMachine.Player);
}