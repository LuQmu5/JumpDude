using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler
{
    private readonly float _movementSpeed;
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;

    private Coroutine _slowdownCoroutine;
    private bool _isFalling = false;

    public bool IsGliding { get; set; } = false;

    public MovementHandler(float movementSpeed, Rigidbody2D rigidbody, MonoBehaviour coroutineRunner)
    {
        _movementSpeed = movementSpeed;
        _rigidbody = rigidbody;
        _coroutineRunner = coroutineRunner;
    }

    public void SetHorizontalVelocity(float input, float multiplier = 1)
    {
        if (_isFalling && !IsGliding)
            return;

        _rigidbody.linearVelocityX = input * _movementSpeed * multiplier;
    }

    public void UpdateFallState()
    {
        bool isCurrentlyFalling = _rigidbody.linearVelocity.y < 0;

        if (isCurrentlyFalling && !_isFalling)
        {
            _isFalling = true;
            StartSlowdown();
        }
        else if (!isCurrentlyFalling && _isFalling)
        {
            _isFalling = false;
            StopSlowdown();
        }
    }

    private void StartSlowdown()
    {
        if (IsGliding)
            return;

        StopSlowdown();

        _slowdownCoroutine = _coroutineRunner.StartCoroutine(SlowdownCoroutine());
    }


    private void StopSlowdown()
    {
        if (_slowdownCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_slowdownCoroutine);
            _slowdownCoroutine = null;
        }
    }

    private IEnumerator SlowdownCoroutine()
    {
        float initialX = _rigidbody.linearVelocity.x;
        float targetX = initialX * 0.2f;

        while (_isFalling && Mathf.Abs(_rigidbody.linearVelocity.x - targetX) > 0.01f)
        {
            float newX = Mathf.Lerp(_rigidbody.linearVelocity.x, targetX, 5f * Time.deltaTime);
            _rigidbody.linearVelocityX = newX;

            yield return null;
        }

        _rigidbody.linearVelocityX = targetX;
    }
}
