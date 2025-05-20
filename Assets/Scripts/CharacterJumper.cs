using UnityEngine;

public class CharacterJumper
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly DoubleJumpHandler _doubleJumpHandler;
    private readonly ChargeHandler _jumpCharge;

    private readonly float _minJumpPower = 10;

    private Vector2 _lastJumpDirection = Vector2.up;
    private float _lastJumpForce = 0f;
    private Vector2 _jumpDirection = Vector2.up;

    public bool IsCharging => _jumpCharge.IsCharging;

    public CharacterJumper(
        GroundChecker groundChecker,
        Rigidbody2D rigidbody,
        float jumpPower,
        float maxHoldTime,
        DoubleJumpHandler doubleJumpHandler)
    {
        _groundChecker = groundChecker;
        _rigidbody = rigidbody;
        _doubleJumpHandler = doubleJumpHandler;
        _jumpCharge = new ChargeHandler(maxHoldTime, _minJumpPower, jumpPower);
    }

    public bool TryStartCharge()
    {
        if (_groundChecker.OnGround() == false && _doubleJumpHandler.CanDoubleJump == false)
            return false;

        _jumpCharge.StartCharge();

        return true;
    }

    public void UpdateCharge(float deltaTime)
    {
        if (!_jumpCharge.IsCharging) 
            return;

        _jumpCharge.Update(deltaTime);

        if (!_jumpCharge.IsCharging)
            TryReleaseJump();
    }

    public bool TryReleaseJump()
    {
        if (!_groundChecker.OnGround())
            return false;

        float finalJumpForceY = _jumpCharge.GetFinalValue();
        float extraVelocityX = _rigidbody.linearVelocityX;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        Vector2 force = new Vector2(_jumpDirection.x * extraVelocityX, _jumpDirection.y * finalJumpForceY);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);

        _lastJumpDirection = _jumpDirection;
        _lastJumpForce = finalJumpForceY;

        _jumpCharge.Reset();
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
