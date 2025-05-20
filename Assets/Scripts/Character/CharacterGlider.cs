using UnityEngine;

public class CharacterGlider
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly float _glideGravityScale;
    private readonly float _glideSpeedMultiplier;

    private float _originalGravityScale;
    private bool _isGliding;

    public bool IsGliding => _isGliding;

    public CharacterGlider(Rigidbody2D rigidbody, GroundChecker groundChecker, float glideGravityScale, float glideSpeedMultiplier)
    {
        _rigidbody = rigidbody;
        _groundChecker = groundChecker;
        _glideGravityScale = glideGravityScale;
        _glideSpeedMultiplier = glideSpeedMultiplier;

        _originalGravityScale = _rigidbody.gravityScale;
    }

    public void Update(bool jumpHeld, Vector2 inputDirection)
    {
        if (_groundChecker.OnGround())
        {
            if (_isGliding)
                StopGlide();
            return;
        }

        if (jumpHeld && _rigidbody.linearVelocity.y < 0)
        {
            StartGlide();
            ApplyGlideMovement(inputDirection);
        }
        else if (_isGliding)
        {
            StopGlide();
        }
    }

    private void StartGlide()
    {
        if (_isGliding)
            return;

        _isGliding = true;
        _originalGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = _glideGravityScale;
    }

    private void StopGlide()
    {
        _isGliding = false;
        _rigidbody.gravityScale = _originalGravityScale;
    }

    private void ApplyGlideMovement(Vector2 inputDirection)
    {
        Vector2 velocity = _rigidbody.linearVelocity;
        velocity.x = inputDirection.x * _glideSpeedMultiplier;
        _rigidbody.linearVelocity = velocity;
    }
}
