using System.Collections;
using UnityEngine;

public class DashHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly DashEffect _dashEffect;

    private readonly float _minHoldTime = 0.25f;
    private readonly float _maxHoldTime = 0.5f;
    private readonly float _dashPower;
    private readonly float _cooldown;
    private readonly float _dashDuration = 0.2f;

    private bool _canDash = true;
    private float _dashHoldStartTime;

    public bool IsDashing { get; private set; } = false;

    public DashHandler(Rigidbody2D rigidbody, float dashPower, float cooldown,
                       MonoBehaviour coroutineRunner, DashEffect dashEffect)
    {
        _rigidbody = rigidbody;
        _dashPower = dashPower;
        _cooldown = cooldown;
        _coroutineRunner = coroutineRunner;
        _dashEffect = dashEffect;
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

        yield return new WaitForSeconds(_cooldown);
        _canDash = true;
    }
}
