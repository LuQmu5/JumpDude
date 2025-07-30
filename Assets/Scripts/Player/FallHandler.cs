using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Collider2D _playerCollider;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _legsPoint;
    private readonly LayerMask _platformLayer;
    private readonly LayerMask _groundLayer;

    private readonly float _fallGravityScale;
    private readonly float _acceleratedFallForce;
    private readonly float _baseGravityScale;

    private Coroutine _fallRoutine;
    private bool _isFallingActive = false;

    private readonly HashSet<Collider2D> _ignoredPlatforms = new();

    private const float GroundCheckDistance = 0.15f;
    private const float PlatformIgnoreRadius = 2f;

    public bool IsFalling => _isFallingActive;

    public FallHandler(
        FallConfig config,
        Rigidbody2D rigidbody,
        Collider2D playerCollider,
        Transform legsPoint,
        MonoBehaviour coroutineRunner)
    {
        _fallGravityScale = config.GravityMultiplier;
        _acceleratedFallForce = config.AcceleratedFallForce;
        _platformLayer = config.PlatformLayer;
        _groundLayer = config.GroundLayer;
        _baseGravityScale = 1.2f;

        _rigidbody = rigidbody;
        _playerCollider = playerCollider;
        _legsPoint = legsPoint;
        _coroutineRunner = coroutineRunner;
    }

    public void StartFastFall()
    {
        if (_isFallingActive)
            return;

        _isFallingActive = true;

        _rigidbody.gravityScale = _fallGravityScale;
        _rigidbody.AddForce(Vector2.down * _acceleratedFallForce, ForceMode2D.Impulse);
        _fallRoutine = _coroutineRunner.StartCoroutine(FallRoutine());
    }

    public void StopFastFall()
    {
        if (!_isFallingActive)
            return;

        _isFallingActive = false;

        ResetGravity();
        RestoreIgnoredPlatforms();

        if (_fallRoutine != null)
        {
            _coroutineRunner.StopCoroutine(_fallRoutine);
            _fallRoutine = null;
        }
    }

    private IEnumerator FallRoutine()
    {
        while (_isFallingActive && !IsTouchingGround())
        {
            IgnoreNearbyPlatforms();
            yield return null;
        }

        _isFallingActive = false;
        ResetGravity();
        RestoreIgnoredPlatforms();
        _fallRoutine = null;
    }

    private void IgnoreNearbyPlatforms()
    {
        Collider2D[] platforms = Physics2D.OverlapCircleAll(_legsPoint.position, PlatformIgnoreRadius, _platformLayer);

        foreach (var platform in platforms)
        {
            if (!_ignoredPlatforms.Contains(platform))
            {
                Physics2D.IgnoreCollision(_playerCollider, platform, true);
                _ignoredPlatforms.Add(platform);
            }
        }
    }

    private void RestoreIgnoredPlatforms()
    {
        foreach (var platform in _ignoredPlatforms)
        {
            if (platform != null)
                Physics2D.IgnoreCollision(_playerCollider, platform, false);
        }

        _ignoredPlatforms.Clear();
    }

    private void ResetGravity()
    {
        _rigidbody.gravityScale = _baseGravityScale;
    }

    private bool IsTouchingGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(_legsPoint.position, Vector2.down, GroundCheckDistance, _groundLayer);
        Debug.DrawRay(_legsPoint.position, Vector2.down * GroundCheckDistance, Color.red, 0.1f);
        return hit.collider != null;
    }
}
