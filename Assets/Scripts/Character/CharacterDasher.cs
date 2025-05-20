using System;
using System.Collections;
using UnityEngine;

public class CharacterDasher
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _monoBehaviour;

    private readonly float _dashDuration;
    private readonly float _cooldown;
    private readonly ChargeHandler _dashCharge;

    private bool _canDash = true;
    private bool _isDashing = false;
    private Vector2 _lastInputDirection = Vector2.right;
    private readonly Action<float> _onDashStart;

    public bool IsDashing => _isDashing;
    public bool IsCharging => _dashCharge.IsCharging;

    public CharacterDasher(MonoBehaviour context, Rigidbody2D rigidbody, 
        float minDashForce, float maxDashForce, float dashDuration, float cooldown, float maxChargeTime,
        Action<float> onDashStart)
    {
        _monoBehaviour = context;
        _rigidbody = rigidbody;
        _dashDuration = dashDuration;
        _cooldown = cooldown;
        _dashCharge = new ChargeHandler(maxChargeTime, minDashForce, maxDashForce);
        _onDashStart = onDashStart;
    }

    public void StartDashCharge(Vector2 direction)
    {
        if (!_canDash || _rigidbody.linearVelocityY < 0)
            return;

        _lastInputDirection = direction.normalized;
        _dashCharge.StartCharge();
    }

    public void UpdateDashCharge(float deltaTime)
    {
        if (!_dashCharge.IsCharging) return;

        _dashCharge.Update(deltaTime);

        if (!_dashCharge.IsCharging)
            ReleaseDash();
    }

    public void ReleaseDash()
    {
        if (!_canDash)
            return;

        float force = _dashCharge.GetFinalValue();
        _monoBehaviour.StartCoroutine(DashCoroutine(_lastInputDirection, force));
        _dashCharge.Reset();
    }

    private IEnumerator DashCoroutine(Vector2 direction, float force)
    {
        float extraMultiplier = 3;
        float t1 = 0.75f;
        float t2 = 1 - t1;

        float originalGravity = _rigidbody.gravityScale;

        _canDash = false;
        _isDashing = true;
        _rigidbody.gravityScale = 0;
        _rigidbody.linearVelocity = Vector2.zero;

        _onDashStart?.Invoke(_dashDuration);

        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_dashDuration * t1);

        _rigidbody.AddForce(direction * force * extraMultiplier, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_dashDuration * t2);

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.gravityScale = originalGravity;
        _isDashing = false;

        yield return new WaitForSeconds(_cooldown);
        _canDash = true;
    }
}
