using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CharacterView
{
    private const string VelocityX = nameof(VelocityX);
    private const string VelocityY = nameof(VelocityY);
    private const string OnGround = nameof(OnGround);
    private const string Dash = nameof(Dash);
    private const string IsCharging = nameof(IsCharging);

    private readonly SpriteRenderer _spriteRenderer;
    private readonly Animator _animator;
    private readonly ParticleSystem _doubleJumpVFX;

    private DashEffect _dashEffect;

    public Vector2 LookDirection => _spriteRenderer.transform.localScale.x == 1 ? Vector2.right : Vector2.left;

    public CharacterView(SpriteRenderer spriteRenderer, 
        Animator animator, 
        SpriteRenderer[] dashShadows, 
        MonoBehaviour monoBehaviour,
        ParticleSystem doubleJumpVFX)
    {
        _spriteRenderer = spriteRenderer;
        _animator = animator;
        _doubleJumpVFX = doubleJumpVFX;
        _dashEffect = new DashEffect(dashShadows, _spriteRenderer, spriteRenderer.transform, monoBehaviour);
    }

    public void UpdateLookDirection(Vector2 direction)
    {
        if (direction.x == 0)
            return;

        Vector3 scale = _spriteRenderer.transform.localScale;
        scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
        _spriteRenderer.transform.localScale = scale;
    }

    public void UpdateVelocityParams(Vector2 velocity)
    {
        _animator.SetInteger(VelocityX, Mathf.Abs((int)velocity.x));
        _animator.SetInteger(VelocityY, Mathf.Abs((int)velocity.y));
    }

    public void UpdateOnGroundParam(bool state)
    {
        _animator.SetBool(OnGround, state);
    }

    public void SetDashTrigger()
    {
        _animator.SetTrigger(Dash);

    }

    public void UpdateJumpChargingParam(bool state)
    {
        _animator.SetBool(IsCharging, state);
    }

    public void PlayDoubleJumpEffect()
    {
        _doubleJumpVFX.Play();
    }

    public void PlayDashEffect(float dashDuration)
    {
        _dashEffect.Play(dashDuration, LookDirection);
    }
}
