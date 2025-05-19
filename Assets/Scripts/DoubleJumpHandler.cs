using UnityEngine;

public class DoubleJumpHandler
{
    private readonly GroundChecker _groundChecker;

    private readonly float _maxAirTime;
    private float _airTime;

    public bool UsedDoubleJump { get; private set; }

    public bool CanDoubleJump =>
        _airTime <= _maxAirTime && !UsedDoubleJump;

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
            UsedDoubleJump = false;
        }
        else
        {
            _airTime += deltaTime;
        }
    }

    public void MarkUsed()
    {
        UsedDoubleJump = true;
    }
}
