using System;
using UnityEngine;

public class CharacterMover
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private float _movementSpeed = 5;

    public CharacterMover(Rigidbody2D rigidbody, float movementSpeed, GroundChecker groundChecker)
    {
        _rigidbody = rigidbody;
        _movementSpeed = movementSpeed;
        _groundChecker = groundChecker;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        float airSpeedMultiplier = 0.5f;
        float groundSpeedMultiplier = 1f;
        float speedMultiplier = _groundChecker.OnGround() ? groundSpeedMultiplier : airSpeedMultiplier;

        _rigidbody.linearVelocityX = direction.x * _movementSpeed * speedMultiplier;
    }

    public void Stop()
    {
        _rigidbody.linearVelocityX = 0;
    }
}
