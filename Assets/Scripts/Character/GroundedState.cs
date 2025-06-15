using UnityEngine;

public class GroundedState : PlayerState
{
    public GroundedState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void HandleInput()
    {
        if (player.input.JumpPressed)
        {
            stateMachine.ChangeState(new JumpState(player, stateMachine));
        }
        else if (player.input.DashHeld)
        {
            stateMachine.ChangeState(new DashState(player, stateMachine));
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
