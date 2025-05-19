using UnityEngine;

public class CharacterJumper
{
    private readonly GroundChecker _groundChecker;
    private readonly Rigidbody2D _rigidbody;
    private readonly float _jumpPower;
    private readonly float _maxHoldTime;

    private float _chargeTime;
    private float _minJumpPower = 5;
    private Vector2 _jumpDirection = Vector2.up;

    public bool IsCharging { get; private set; }

    public CharacterJumper(GroundChecker groundChecker, Rigidbody2D rigidbody, float jumpPower, float maxHoldTime)
    {
        _groundChecker = groundChecker;
        _rigidbody = rigidbody;
        _jumpPower = jumpPower;
        _maxHoldTime = maxHoldTime;
    }

    public void StartCharge()
    {
        if (_groundChecker.OnGround() == false) 
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
        if (_groundChecker.OnGround() == false)
            return;

        if (IsCharging == false)
            return;

        float multiplier = Mathf.Clamp01(_chargeTime / _maxHoldTime);
        float finalJumpForce = multiplier * _jumpPower;

        if (finalJumpForce < _minJumpPower)
            finalJumpForce = _minJumpPower;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        _rigidbody.AddForce(_jumpDirection * finalJumpForce, ForceMode2D.Impulse);

        IsCharging = false;
        _chargeTime = 0f;
    }

    public void UpdateJumpDirection(Vector2 direction)
    {
        _jumpDirection = (Vector2.up + new Vector2(direction.x, 0)).normalized;
    }
}
