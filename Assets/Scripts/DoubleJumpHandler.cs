using UnityEngine;

public class DoubleJumpHandler
{
    private readonly GroundChecker _groundChecker;
    private readonly float _maxAirTime;

    private float _airTime;
    private bool _canDoubleJump = true;

    public DoubleJumpHandler(GroundChecker groundChecker, float maxAirTime)
    {
        _groundChecker = groundChecker;
        _maxAirTime = maxAirTime;
    }

    public void Update(float deltaTime)
    {
        if (_groundChecker.OnGround())
        {
            _airTime = 0f;
            _canDoubleJump = true;
        }
        else
        {
            _airTime += deltaTime;
        }
    }

    public bool CanDoubleJump()
    {
        return _canDoubleJump && _airTime <= _maxAirTime;
    }

    public bool TryUseDoubleJump()
    {
        if (_canDoubleJump && _airTime <= _maxAirTime)
        {
            _canDoubleJump = false;

            return true;
        }

        return false;
    }
}
