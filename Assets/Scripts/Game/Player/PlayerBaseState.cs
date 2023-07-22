public abstract class PlayerBaseState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine sm)
    {
        this.stateMachine = sm;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
