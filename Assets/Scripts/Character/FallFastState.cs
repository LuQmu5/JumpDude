using UnityEngine;

public class FallFastState : PlayerState
{
    public FallFastState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        player.FallFast();
        // Здесь можешь включить эффект "удар вниз", анимацию и т.д.
    }

    public override void LogicUpdate()
    {
        if (player.IsGrounded)
        {
            // Здесь можно нанести урон по прилегающим врагам
            DoGroundImpact();
            stateMachine.ChangeState(new GroundedState(player, stateMachine));
        }
    }

    public override void PhysicsUpdate()
    {
        player.Move(player.input.MoveDirection);
    }

    private void DoGroundImpact()
    {
        float radius = 1.5f;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, radius);

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
                GameObject.Destroy(enemy.gameObject);
        }
    }
}
