public class JumpState : PlayerState
{
    public JumpState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        player.Jump();
        player.GetComponent<AnimationController>()?.PlayJump();
    }

    public override void LogicUpdate()
    {
        if (player.IsFalling)
        {
            stateMachine.ChangeState(new GlideState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Move(player.input.MoveDirection);
    }
}
