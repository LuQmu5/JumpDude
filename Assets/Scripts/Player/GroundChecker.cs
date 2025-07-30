using UnityEngine;

public class GroundChecker
{
    private LayerMask _groundMask;
    private Transform _legsPoint;
    private float _legsRadius;

    public GroundChecker(GroundCheckConfig config, Transform legsPoint)
    {
        _groundMask = config.GroundMask;
        _legsPoint = legsPoint;
        _legsRadius = config.LegsRadius;
    }

    public bool OnGround() => Physics2D.OverlapCircle(_legsPoint.position, _legsRadius, _groundMask);
}
