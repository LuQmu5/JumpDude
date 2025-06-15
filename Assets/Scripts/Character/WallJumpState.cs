using UnityEngine;

public class WallJumpState : PlayerState
{
    public WallJumpState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        Vector2 wallNormal = Vector2.left * Mathf.Sign(player.transform.localScale.x);
        player.WallJump(wallNormal);
    }

    public override void LogicUpdate()
    {
        if (player.IsFalling)
        {
            stateMachine.ChangeState(new AirborneState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Move(player.input.MoveDirection);
    }
}
