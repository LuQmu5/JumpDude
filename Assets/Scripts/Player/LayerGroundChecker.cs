using UnityEngine;

public class LayerGroundChecker : IGroundChecker
{
    private Transform _legsPoint;
    private float _legsRadius;
    private LayerMask _groundMask;

    public LayerGroundChecker(Transform legsPoint, float legsRadius, LayerMask groundMask)
    {
        _legsPoint = legsPoint;
        _legsRadius = legsRadius;
        _groundMask = groundMask;
    }

    public bool IsGrounded() => Physics2D.OverlapCircle(_legsPoint.position, _legsRadius, _groundMask);
}
