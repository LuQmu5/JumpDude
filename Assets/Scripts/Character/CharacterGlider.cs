using UnityEngine;

public class CharacterGlider
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GlideSettings _settings;
    private readonly Animator _animator;

    private float _originalGravityScale;
    private float _currentTransitionTime;
    private bool _isGliding;

    public bool IsGliding => _isGliding;

    public CharacterGlider(Rigidbody2D rigidbody, Animator animator, GlideSettings settings)
    {
        _rigidbody = rigidbody;
        _settings = settings;
        _animator = animator;

        _originalGravityScale = _rigidbody.gravityScale;
    }

    public void Update(bool isGrounded, bool jumpKeyHeld, float deltaTime)
    {
        if (isGrounded)
        {
            StopGliding();
            return;
        }

        bool shouldGlide = jumpKeyHeld && _rigidbody.linearVelocity.y < 0;

        if (shouldGlide && !_isGliding)
        {
            StartGliding();
        }
        else if (!shouldGlide && _isGliding)
        {
            StopGliding();
        }

        if (_isGliding)
        {
            _currentTransitionTime += deltaTime;
            float t = Mathf.Clamp01(_currentTransitionTime / _settings.gravityTransitionDuration);
            _rigidbody.gravityScale = Mathf.Lerp(_originalGravityScale, _settings.gravityScale, t);
        }
        else
        {
            _currentTransitionTime += deltaTime;
            float t = Mathf.Clamp01(_currentTransitionTime / _settings.gravityTransitionDuration);
            _rigidbody.gravityScale = Mathf.Lerp(_settings.gravityScale, _originalGravityScale, t);
        }
    }

    private void StartGliding()
    {
        _isGliding = true;
        _currentTransitionTime = 0f;
        _animator.SetBool(_settings.glidingBoolParam, true);
    }

    private void StopGliding()
    {
        _isGliding = false;
        _currentTransitionTime = 0f;
        _animator.SetBool(_settings.glidingBoolParam, false);
    }

    public float GetHorizontalSpeedMultiplier()
    {
        return _isGliding ? _settings.horizontalSpeedMultiplier : 1f;
    }
}
