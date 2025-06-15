using UnityEngine;

public class WallClingState : PlayerState
{
    public WallClingState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        player.rb.linearVelocity = Vector2.zero;
        player.rb.gravityScale = 0.2f;
        player.GetComponent<AnimationController>()?.PlayWallCling();
    }

    public override void Exit()
    {
        player.rb.gravityScale = 1f;
    }

    public override void HandleInput()
    {
        if (player.input.JumpPressed)
        {
            stateMachine.ChangeState(new WallJumpState(player, stateMachine));
        }

        if (!player.IsTouchingWall || player.IsGrounded)
        {
            stateMachine.ChangeState(new AirborneState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        // Легкий спад вниз при прилипания к стене
        player.rb.linearVelocity = new Vector2(0, -0.5f);
    }
}

