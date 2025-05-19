using System.Collections;
using UnityEngine;

public class CharacterDasher
{
    private Rigidbody2D _rigidbody;
    private float _dashForce;
    private float _dashDuration;
    private MonoBehaviour _monoBehaviour;
    private bool _canDash = true;
    private float _cooldown;
    private bool _isDashing = false;

    public bool IsDashing => _isDashing;

    public CharacterDasher(MonoBehaviour context, Rigidbody2D rigidbody, float dashForce, float dashDuration, float cooldown)
    {
        _monoBehaviour = context;
        _rigidbody = rigidbody;
        _dashForce = dashForce;
        _dashDuration = dashDuration;
        _cooldown = cooldown;
    }

    public void TryDash(Vector2 direction)
    {
        if (!_canDash) 
            return;

        _monoBehaviour.StartCoroutine(DashCoroutine(direction.normalized));
    }

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        _canDash = false;
        _isDashing = true;

        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        _rigidbody.linearVelocity = Vector2.zero;

        _rigidbody.AddForce(direction * _dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_dashDuration);

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.gravityScale = originalGravity;
        _isDashing = false;

        yield return new WaitForSeconds(_cooldown);

        _canDash = true;
    }
}
