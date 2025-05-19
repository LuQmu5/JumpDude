using UnityEngine;

public class CharacterJumper
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly DoubleJumpHandler _doubleJumpHandler;
    private readonly float _movementSpeed;
    private readonly float _jumpPower;
    private readonly float _maxHoldTime;
    private readonly float _minJumpPower = 10;

    private float _chargeTime;
    private Vector2 _jumpDirection = Vector2.up;

    public bool IsCharging { get; private set; }


    public CharacterJumper(
        GroundChecker groundChecker,
        Rigidbody2D rigidbody,
        float jumpPower,
        float maxHoldTime,
        DoubleJumpHandler doubleJumpHandler,
        float movementSpeed)
    {
        _groundChecker = groundChecker;
        _rigidbody = rigidbody;
        _jumpPower = jumpPower;
        _maxHoldTime = maxHoldTime;
        _doubleJumpHandler = doubleJumpHandler;
        _movementSpeed = movementSpeed;
    }

    public void StartCharge()
    {
        bool grounded = _groundChecker.OnGround();
        bool allowedInAir = _doubleJumpHandler != null && _doubleJumpHandler.CanDoubleJump();

        if (!grounded && !allowedInAir)
            return;

        IsCharging = true;
        _chargeTime = 0f;
    }


    public void CancelCharge()
    {
        IsCharging = false;
        _chargeTime = 0f;
    }

    public void UpdateCharge(float deltaTime)
    {
        if (IsCharging == false)
            return;

        _chargeTime += deltaTime;

        if (_chargeTime >= _maxHoldTime)
        {
            _chargeTime = _maxHoldTime;
            ReleaseJump();
        }
    }

    public void ReleaseJump()
    {
        if (!IsCharging) 
            return;

        bool grounded = _groundChecker.OnGround();
        bool allowedInAir = _doubleJumpHandler != null && _doubleJumpHandler.TryUseDoubleJump();

        if (!grounded && !allowedInAir)
            return;

        float multiplier = Mathf.Clamp01(_chargeTime / _maxHoldTime);
        float finalJumpForce = Mathf.Max(multiplier * _jumpPower, _minJumpPower);
        float extraVelocityX = _rigidbody.linearVelocityX / 2;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        _rigidbody.AddForce(new Vector3(_jumpDirection.x * extraVelocityX, _jumpDirection.y * finalJumpForce), ForceMode2D.Impulse);

        IsCharging = false;
        _chargeTime = 0f;
    }

    public void UpdateJumpDirection(Vector2 direction)
    {
        _jumpDirection = (Vector2.up + new Vector2(direction.x, 0)).normalized;
    }
}
