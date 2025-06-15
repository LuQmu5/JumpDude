using UnityEngine;

public class AirborneState : PlayerState
{
    public AirborneState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void HandleInput()
    {
        if (player.IsGrounded)
        {
            stateMachine.ChangeState(new GroundedState(player, stateMachine));
        }

        // Планирование
        if (player.input.JumpPressed && player.IsFalling)
        {
            stateMachine.ChangeState(new GlideState(player, stateMachine));
        }

        // Второй прыжок (допустим только при восходящем движении)
        if (player.input.JumpPressed && player.IsRising)
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine)); // Двойной прыжок
        }

        if (player.input.DashHeld && player.IsRising)
        {
            stateMachine.ChangeState(new DashState(player, stateMachine));
        }

        if (player.input.FallFastPressed && player.IsFalling)
        {
            stateMachine.ChangeState(new FallFastState(player, stateMachine));
        }

        if (player.input.HookPressed)
        {
            bool success = player.GetComponent<HookController>().TryFireHook(out Vector2 _);

            if (success)
            {
                stateMachine.ChangeState(new HookPullState(player, stateMachine));
            }
        }

    }

    public override void PhysicsUpdate()
    {
        player.Move(player.input.MoveDirection);
    }
}
