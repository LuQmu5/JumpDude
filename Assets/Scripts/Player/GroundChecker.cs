using UnityEngine;

public class GroundChecker
{
    private LayerMask _groundMask;
    private Transform _legsPoint;
    private float _legsRadius = 0.1f;

    public GroundChecker(LayerMask groundMask, Transform legsPoint)
    {
        _groundMask = groundMask;
        _legsPoint = legsPoint;
    }

    public bool OnGround() => Physics2D.OverlapCircle(_legsPoint.position, _legsRadius, _groundMask);
}
