using UnityEngine;

public class GravityHandler
{
    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;

    public GravityHandler(Rigidbody2D rigidbody, GroundChecker groundChecker)
    {
        _rigidbody = rigidbody;
        _groundChecker = groundChecker;
    }

    public void HandleGravity()
    {
        if (_groundChecker.OnGround() && _rigidbody.linearVelocityY <= 0)
            _rigidbody.linearVelocityY = 0;
    }
}
