public class GlideState : PlayerState
{
    public GlideState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        player.GetComponent<AnimationController>()?.PlayGlide(true);
    }

    public override void Exit()
    {
        player.GetComponent<AnimationController>()?.PlayGlide(false);
    }

    public override void HandleInput()
    {
        if (player.input.FallFastPressed)
        {
            stateMachine.ChangeState(new FallFastState(player, stateMachine));
        }

        if (player.IsGrounded)
        {
            stateMachine.ChangeState(new GroundedState(player, stateMachine));
        }

        if (!player.input.JumpPressed || !player.IsFalling)
        {
            stateMachine.ChangeState(new AirborneState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Glide();
        player.Move(player.input.MoveDirection);
    }
}

