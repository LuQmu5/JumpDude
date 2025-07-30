using UnityEngine;

public class GroundChecker
{
    private readonly LayerMask _groundMask;
    private readonly Transform _legsPoint;
    private readonly Vector2 _boxSize;

    public GroundChecker(GroundCheckConfig config, Transform legsPoint)
    {
        _groundMask = config.GroundMask;
        _legsPoint = legsPoint;
        _boxSize = new Vector2(config.Width, config.Height);
    }

    public bool OnGround()
    {
        return Physics2D.OverlapBox(_legsPoint.position, _boxSize, 0f, _groundMask);
    }
}
