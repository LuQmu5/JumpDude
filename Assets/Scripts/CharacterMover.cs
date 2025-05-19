using UnityEngine;

public class CharacterMover
{
    private Rigidbody2D _rigidbody;
    private float _movementSpeed = 5;

    public CharacterMover(Rigidbody2D rigidbody, float movementSpeed)
    {
        _rigidbody = rigidbody;
        _movementSpeed = movementSpeed;
    }

    public void SetInputDirection(Vector2 direction)
    {
        _rigidbody.linearVelocityX = direction.x * _movementSpeed;
    }
}
