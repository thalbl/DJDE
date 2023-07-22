using UnityEngine;
using System.Threading.Tasks;

public class PlayerMovingState : PlayerBaseState
{
    public PlayerMovingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        // Start animation, etc

        if(this.stateMachine.IsActing)
            return;
        
        // Can't re-enter the moving state a second time
        this.stateMachine.IsActing = true;
        
        Move();
    }

    public override void Exit()
    {
        // Stop animation, etc
    }

    public override void Update()
    {
        // Play animation, etc
    }

    private async void Move()
    {
        // Wait until the player is done moving
        await PlayerEvents.OnMove.Invoke(this.stateMachine.Player);
        
        this.stateMachine.IsActing = false;

        // End this player's turn
        PlayerEvents.OnTurnEnd.Invoke(this.stateMachine.Player);
    }
}