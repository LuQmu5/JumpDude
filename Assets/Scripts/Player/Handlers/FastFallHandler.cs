﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFallHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Collider2D _playerCollider;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _legsPoint;
    private readonly LayerMask _platformLayer;
    private readonly LayerMask _groundLayer;
    private readonly TrailRenderer _trailRenderer;
    private readonly KillAreaTrigger _killAreaTrigger;
    private readonly float _fallGravityScale;
    private readonly float _acceleratedFallForce;
    private readonly float _baseGravityScale;

    private Coroutine _fallRoutine;
    private bool _isFallingActive = false;

    private readonly HashSet<Collider2D> _ignoredPlatforms = new();

    private const float GroundCheckDistance = 0.15f;
    private const float PlatformIgnoreRadius = 2f;

    public bool IsFalling => _isFallingActive;

    public FastFallHandler(
        FastFallConfig config,
        Rigidbody2D rigidbody,
        Collider2D playerCollider,
        Transform legsPoint,
        MonoBehaviour coroutineRunner,
        TrailRenderer trailRenderer,
        KillAreaTrigger killAreaTrigger)
    {
        _fallGravityScale = config.GravityMultiplier;
        _acceleratedFallForce = config.AcceleratedFallForce;
        _platformLayer = config.PlatformLayer;
        _groundLayer = config.GroundLayer;
        _baseGravityScale = rigidbody.gravityScale;

        _rigidbody = rigidbody;
        _playerCollider = playerCollider;
        _legsPoint = legsPoint;
        _coroutineRunner = coroutineRunner;
        _trailRenderer = trailRenderer;
        _killAreaTrigger = killAreaTrigger;
        EnableTrail(false);
    }

    public void StartFastFall()
    {
        /*
        if (_rigidbody.linearVelocityY > -2 && _rigidbody.linearVelocityY < 2)
        {
            _rigidbody.AddForce(Vector2.up * _acceleratedFallForce, ForceMode2D.Impulse);
            return;
        }
        */

        if (_isFallingActive)
            return;

        _isFallingActive = true;

        EnableTrail(true);

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
        EnableTrail(false);

        if (_fallRoutine != null)
        {
            _coroutineRunner.StopCoroutine(_fallRoutine);
            _fallRoutine = null;
        }
    }

    private IEnumerator FallRoutine()
    {
        _killAreaTrigger.Activate();

        while (_isFallingActive && !IsTouchingGround())
        {
            IgnoreNearbyPlatforms();
            yield return null;
        }

        _isFallingActive = false;
        ResetGravity();
        RestoreIgnoredPlatforms();
        EnableTrail(false);
        _killAreaTrigger.Deactivate();
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

    private void EnableTrail(bool enabled)
    {
        if (_trailRenderer != null)
        {
            _trailRenderer.emitting = enabled;
        }
    }
}
