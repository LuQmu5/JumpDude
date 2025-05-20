using System;
using UnityEngine;

public class CharacterMover
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;

    private float _movementSpeed = 5;
    private float _currentSpeedMultiplier = 1f;
    private float _runDirection = 0f;
    private float _runTime = 0f;

    private readonly float _maxSpeedMultiplier = 2f;
    private readonly float _accelerationTime = 1f;

    public CharacterMover(Rigidbody2D rigidbody, float movementSpeed, GroundChecker groundChecker, float maxSpeedMultiplier, float accelerationTime)
    {
        _rigidbody = rigidbody;
        _movementSpeed = movementSpeed;
        _groundChecker = groundChecker;
        _maxSpeedMultiplier = maxSpeedMultiplier;
        _accelerationTime = accelerationTime;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        float targetDirection = Mathf.Sign(direction.x);

        if (targetDirection != 0)
        {
            if (Mathf.Approximately(targetDirection, _runDirection))
            {
                _runTime += Time.deltaTime;
            }
            else
            {
                _runDirection = targetDirection;
                _runTime = 0f;
                _currentSpeedMultiplier = 1f;
            }

            float t = Mathf.Clamp01(_runTime / _accelerationTime);
            _currentSpeedMultiplier = Mathf.Lerp(1f, _maxSpeedMultiplier, t);
        }
        else
        {
            _runTime = 0f;
            _currentSpeedMultiplier = 1f;
        }

        float airSpeedMultiplier = 0.5f;
        float groundSpeedMultiplier = 1f;
        float groundAirFactor = _groundChecker.OnGround() ? groundSpeedMultiplier : airSpeedMultiplier;

        _rigidbody.linearVelocityX = direction.x * _movementSpeed * _currentSpeedMultiplier * groundAirFactor;
    }
}
