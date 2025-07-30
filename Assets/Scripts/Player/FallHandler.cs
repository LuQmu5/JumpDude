using System.Collections;
using UnityEngine;

public class FallHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly Collider2D _playerCollider;
    private readonly GroundChecker _groundChecker;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _legsPoint;
    private readonly LayerMask _platformLayer;

    private readonly float _fallGravityScale;
    private readonly float _acceleratedFallForce;
    private readonly float _disableCollisionDuration;

    private Coroutine _fallRoutine;
    private bool _isFallingActive;

    public bool IsFalling => _isFallingActive;

    public FallHandler(
        FallConfig config,
        Rigidbody2D rigidbody,
        Collider2D playerCollider,
        Transform legsPoint,
        GroundChecker groundChecker,
        MonoBehaviour coroutineRunner)
    {
        _fallGravityScale = config.GravityMultiplier;
        _acceleratedFallForce = config.AcceleratedFallForce;
        _disableCollisionDuration = config.DisableCollisionDuration;
        _platformLayer = config.PlatformLayer;

        _rigidbody = rigidbody;
        _playerCollider = playerCollider;
        _legsPoint = legsPoint;
        _groundChecker = groundChecker;
        _coroutineRunner = coroutineRunner;
    }

    public void StartFall()
    {
        if (IsOnPlatform())
        {
            if (_fallRoutine == null)
                _coroutineRunner.StartCoroutine(DisableCollisionRoutine());
        }
        else
        {
            if (_fallRoutine == null)
                _fallRoutine = _coroutineRunner.StartCoroutine(AcceleratedFallRoutine());
        }
    }

    public void StopFall()
    {
        _isFallingActive = false;
    }

    private bool IsOnPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(_legsPoint.position, 0.1f, _platformLayer);
        return hit != null;
    }

    private Collider2D GetCurrentPlatform()
    {
        Collider2D hit = Physics2D.OverlapCircle(_legsPoint.position, 0.1f, _platformLayer);
        return hit;
    }

    private IEnumerator DisableCollisionRoutine()
    {
        Collider2D platform = GetCurrentPlatform();

        if (platform != null)
        {
            Physics2D.IgnoreCollision(_playerCollider, platform, true);
            _isFallingActive = true;
            _rigidbody.AddForce(Vector2.down * _acceleratedFallForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(_disableCollisionDuration);

            _isFallingActive = false;
            Physics2D.IgnoreCollision(_playerCollider, platform, false);
        }
    }

    private IEnumerator AcceleratedFallRoutine()
    {
        _isFallingActive = true;

        float originalGravity = _rigidbody.gravityScale;
        _rigidbody.gravityScale = _fallGravityScale;

        _rigidbody.AddForce(Vector2.down * _acceleratedFallForce, ForceMode2D.Impulse);

        yield return new WaitUntil(() => !_isFallingActive || _groundChecker.OnGround());

        _rigidbody.gravityScale = originalGravity;
        _fallRoutine = null;
    }
}
