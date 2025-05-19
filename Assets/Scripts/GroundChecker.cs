using UnityEngine;

public class GroundChecker
{
    private Transform _legsPoint;
    private float _legsRadius;
    private LayerMask _groundLayer;

    public GroundChecker(Transform legsPoint, float legsRadius, LayerMask groundLayer)
    {
        _legsPoint = legsPoint;
        _legsRadius = legsRadius;
        _groundLayer = groundLayer;
    }

    public bool OnGround() => Physics2D.OverlapCircle(_legsPoint.position, _legsRadius, _groundLayer);
}
