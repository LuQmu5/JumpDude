using UnityEngine;

public class CharacterJumper
{
    private readonly GroundChecker _groundChecker;
    private readonly Rigidbody2D _rigidbody;
    private readonly float _jumpPower;
    private readonly float _maxHoldTime;

    private bool _isCharging;
    private float _chargeTime;
    private float _minJumpPower = 5;

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

        _isCharging = true;
        _chargeTime = 0f;
    }

    public void CancelCharge()
    {
        _isCharging = false;
        _chargeTime = 0f;
    }

    public void UpdateCharge(float deltaTime)
    {
        if (_isCharging == false)
            return;

        _chargeTime += deltaTime;

        if (_chargeTime > _maxHoldTime)
            _chargeTime = _maxHoldTime;
    }

    public void ReleaseJump()
    {
        if (_groundChecker.OnGround() == false) 
            return;

        if (!_isCharging)
            return;

        float multiplier = Mathf.Clamp01(_chargeTime / _maxHoldTime);
        float finalJumpForce = multiplier * _jumpPower;

        if (finalJumpForce < _minJumpPower)
            finalJumpForce = _minJumpPower;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        _rigidbody.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);

        _isCharging = false;
        _chargeTime = 0f;
    }
}
