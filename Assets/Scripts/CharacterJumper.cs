using UnityEngine;

public class CharacterJumper
{
    private GroundChecker _groundChecker;
    private Rigidbody2D _rigidbody;
    private float _jumpPower;

    public CharacterJumper(GroundChecker groundChecker, Rigidbody2D rigidbody, float jumpPower)
    {
        _groundChecker = groundChecker;
        _rigidbody = rigidbody;
        _jumpPower = jumpPower;
    }

    public bool TryJump()
    {
        if (_groundChecker.OnGround() == false)
            return false;

        _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);

        return true;
    }
}
