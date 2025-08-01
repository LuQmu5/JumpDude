using System.Collections;
using UnityEngine;


public class DashHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly DashEffect _dashEffect;
    private readonly KillAreaTrigger _killAreaTrigger;
    private readonly float _minHoldTime;
    private readonly float _maxHoldTime;
    private readonly float _dashPower;
    private readonly float _cooldown;
    private readonly float _dashDuration;

    private bool _canDash = true;
    private float _dashHoldStartTime;

    public bool IsDashing { get; private set; } = false;

    public DashHandler(DashConfig config, Rigidbody2D rigidbody,
                       MonoBehaviour coroutineRunner, DashEffect dashEffect, KillAreaTrigger killAreaTrigger)
    {
        _dashPower = config.Power;
        _cooldown = config.Cooldown;
        _dashDuration = config.Duration;
        _minHoldTime = config.MinHoldTime;
        _maxHoldTime = config.MaxHoldTime;

        _rigidbody = rigidbody;
        _coroutineRunner = coroutineRunner;
        _dashEffect = dashEffect;
        _killAreaTrigger = killAreaTrigger;
    }

    public void StartDash()
    {
        if (!_canDash || _rigidbody.linearVelocityY < 0)
            return;

        _dashHoldStartTime = Time.time;
    }

    public bool TryReleaseDash()
    {
        if (!_canDash || _rigidbody.linearVelocityY < 0)
            return false;

        float heldTime = Time.time - _dashHoldStartTime;
        float clampedHold = Mathf.Clamp(heldTime, _minHoldTime, _maxHoldTime);
        float powerPercent = clampedHold / _maxHoldTime;
        float finalForce = _dashPower * powerPercent;

        Vector2 direction = _rigidbody.transform.eulerAngles == Vector3.zero ? Vector2.right : Vector2.left;

        _coroutineRunner.StartCoroutine(Dashing(direction, finalForce));

        return true;
    }

    private IEnumerator Dashing(Vector2 direction, float force)
    {
        IsDashing = true;
        _killAreaTrigger.Activate();
        _canDash = false;

        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        _rigidbody.linearVelocity = Vector2.zero;

        float elapsed = 0f;
        float speed = force / _dashDuration;

        _dashEffect.Play(_dashDuration, direction);

        while (elapsed < _dashDuration)
        {
            _rigidbody.linearVelocity = direction * speed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocityY);
        _rigidbody.gravityScale = originalGravity;
        IsDashing = false;
        _killAreaTrigger.Deactivate();

        yield return new WaitForSeconds(_cooldown);
        _canDash = true;
    }
}
