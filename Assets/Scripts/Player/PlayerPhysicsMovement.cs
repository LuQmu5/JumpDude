using UnityEngine;

public class PlayerPhysicsMovement : IMover
{
    private Rigidbody2D _rigidbody;
    private PlayerCharacterController _player;

    public PlayerPhysicsMovement(Rigidbody2D rigidbody, PlayerCharacterController controller)
    {
        _rigidbody = rigidbody;
        _player = controller;
    }

    public void Move(Vector2 direction)
    {
        _rigidbody.linearVelocityX = direction.x * _player.MovementSpeed;
    }
}
