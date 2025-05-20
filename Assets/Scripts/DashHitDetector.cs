using UnityEngine;

public class DashHitDetector
{
    private readonly Transform _characterTransform;
    private readonly float _checkRadius;
    private readonly LayerMask _enemyLayer;

    public DashHitDetector(Transform characterTransform, float checkRadius, LayerMask enemyLayer)
    {
        _characterTransform = characterTransform;
        _checkRadius = checkRadius;
        _enemyLayer = enemyLayer;
    }

    public void ProcessDashStep()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_characterTransform.position, _checkRadius, _enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var target))
            {
                target.Kill();
            }
        }
    }
}
