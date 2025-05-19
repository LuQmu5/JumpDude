using UnityEngine;

public class CharacterJumper
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly DoubleJumpHandler _doubleJumpHandler;

    private readonly float _jumpPower;
    private readonly float _maxHoldTime;
    private readonly float _minJumpPower = 10;

    private Vector2 _lastJumpDirection = Vector2.up;
    private float _lastJumpForce = 0f;

    private float _chargeTime;
    private Vector2 _jumpDirection = Vector2.up;

    public bool IsCharging { get; private set; }

    public CharacterJumper(
        GroundChecker groundChecker,
        Rigidbody2D rigidbody,
        float jumpPower,
        float maxHoldTime,
        DoubleJumpHandler doubleJumpHandler)
    {
        _groundChecker = groundChecker;
        _rigidbody = rigidbody;
        _jumpPower = jumpPower;
        _maxHoldTime = maxHoldTime;
        _doubleJumpHandler = doubleJumpHandler;
    }

    public bool TryStartCharge()
    {
        if (_groundChecker.OnGround() == false && _doubleJumpHandler.CanDoubleJump == false)
            return false;

        IsCharging = true;
        _chargeTime = 0f;

        return true;
    }

    public void UpdateCharge(float deltaTime)
    {
        if (IsCharging == false)
            return;

        _chargeTime += deltaTime;

        if (_chargeTime >= _maxHoldTime)
        {
            _chargeTime = _maxHoldTime;
            TryReleaseJump();
        }
    }

    public bool TryReleaseJump()
    {
        bool isGrounded = _groundChecker.OnGround();

        if (!isGrounded)
            return false;

        float multiplier = Mathf.Clamp01(_chargeTime / _maxHoldTime);
        float finalJumpForceY = Mathf.Max(multiplier * _jumpPower, _minJumpPower);
        float extraVelocityX = _rigidbody.linearVelocityX;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        Vector2 force = new Vector2(_jumpDirection.x * extraVelocityX, _jumpDirection.y * finalJumpForceY);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);

        _lastJumpDirection = _jumpDirection;
        _lastJumpForce = finalJumpForceY;

        IsCharging = false;
        _chargeTime = 0f;

        return true;
    }

    public void PerformDoubleJump()
    {
        float forceY = _lastJumpForce * 0.75f;
        Vector2 force = _lastJumpDirection * forceY;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }


    public void UpdateJumpDirection(Vector2 direction)
    {
        _jumpDirection = (Vector2.up + new Vector2(direction.x, 0)).normalized;
    }
}
