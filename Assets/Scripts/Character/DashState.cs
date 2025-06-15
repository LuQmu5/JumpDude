using UnityEngine;

public class DashState : PlayerState
{
    private float dashTime = 0.2f;
    private float dashTimer = 0f;
    private bool isAirDash;

    public DashState(PlayerController player, StateMachine sm) : base(player, sm) { }

    public override void Enter()
    {
        isAirDash = !player.IsGrounded;

        // Дэш на земле — слабее и без урона
        if (!isAirDash)
        {
            player.Dash(strength: 1f);
            player.GetComponent<AnimationController>()?.PlayDash();
        }
        else
        {
            player.Dash(strength: 1.5f); // Воздушный дэш — дальше
            player.GetComponent<AnimationController>()?.PlayDash();

            // Здесь можно вызывать метод атаки (например, рубящая атака на пути)
            DoAirSlash();
        }

        dashTimer = dashTime;
    }

    public override void LogicUpdate()
    {
        dashTimer -= Time.deltaTime;

        if (dashTimer <= 0f)
        {
            if (player.IsGrounded)
            {
                stateMachine.ChangeState(new GroundedState(player, stateMachine));
            }
            else
            {
                stateMachine.ChangeState(new AirborneState(player, stateMachine));
            }
        }
    }

    public override void PhysicsUpdate()
    {
        // Дэш не отменяется до конца таймера, игрок не может управлять движением
    }

    private void DoAirSlash()
    {
        // Здесь ты можешь реализовать нанесение урона врагам на пути
        // Простой пример:
        Vector2 origin = player.transform.position;
        Vector2 direction = new Vector2(player.transform.localScale.x, 0);
        float length = 2f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, length);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                // Уничтожаем врага
                GameObject.Destroy(hit.collider.gameObject);
            }
        }
    }
}
