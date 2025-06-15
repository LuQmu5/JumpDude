using UnityEngine;

public class HookPullState : PlayerState
{
    private float duration = 0.3f;
    private float timer;

    public HookPullState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        timer = duration;
        player.GetComponent<AnimationController>()?.PlayHook();
    }

    public override void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            stateMachine.ChangeState(new AirborneState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        // Ничего — физика рывка срабатывает сразу в HookController
    }
}

