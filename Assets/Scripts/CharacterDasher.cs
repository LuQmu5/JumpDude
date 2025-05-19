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

    public bool TryDash(Vector2 direction)
    {
        if (_canDash == false) 
            return false;

        if (_rigidbody.linearVelocityY < 0)
            return false;

        _monoBehaviour.StartCoroutine(DashCoroutine(direction.normalized));

        return true;
    }

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        float lastDashMultiplier = 3;
        float originalGravity = _rigidbody.gravityScale;
        float timeMultiplierToNormalDash = 0.75f;
        float timeMultiplierToExtraDash = 1 - timeMultiplierToNormalDash;

        _canDash = false;
        _isDashing = true;
        _rigidbody.gravityScale = 0;
        _rigidbody.linearVelocity = Vector2.zero;

        _rigidbody.AddForce(direction * _dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_dashDuration * timeMultiplierToNormalDash);

        _rigidbody.AddForce(direction * _dashForce * lastDashMultiplier, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_dashDuration * timeMultiplierToExtraDash);

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.gravityScale = originalGravity;
        _isDashing = false;

        yield return new WaitForSeconds(_cooldown);

        _canDash = true;
    }
}
