using System.Collections;
using UnityEngine;

public class HookHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _hookStartPoint;
    private readonly LayerMask _hookableLayer;
    private readonly float _pullForce;
    private readonly float _pullFinalForce;
    private readonly float _maxDistance;
    private readonly float _minDistance;
    private readonly float _minDistanceY;
    private readonly float _minTotalDistance;
    private readonly LineRenderer _lineRenderer;
    private readonly Collider2D _collider;

    private Coroutine _hookRoutine;
    private bool _isHooking;

    public bool IsHooking => _isHooking;

    public HookHandler(
        HookConfig config,
        Rigidbody2D rigidbody,
        Transform hookStartPoint,
        MonoBehaviour coroutineRunner,
        LineRenderer lineRenderer,
        Collider2D collider)
    {
        _rigidbody = rigidbody;
        _collider = collider;
        _coroutineRunner = coroutineRunner;
        _hookStartPoint = hookStartPoint;
        _hookableLayer = config.HookableLayer;
        _pullForce = config.PullForce;
        _maxDistance = config.MaxDistance;
        _minDistanceY = config.MinDistanceY;
        _minTotalDistance = config.MinTotalDistance;
        _pullFinalForce = config.PullFinalForce;
        _lineRenderer = lineRenderer;
        _lineRenderer.enabled = false;
    }
    public bool StartHook(Vector2 cursorWorldPosition)
    {
        if (_isHooking || _hookRoutine != null)
            return false;

        Vector2 start = _hookStartPoint.position;
        Vector2 dir = (cursorWorldPosition - start).normalized;

        float totalDistance = Vector2.Distance(start, cursorWorldPosition);
        float heightDifference = Mathf.Abs(cursorWorldPosition.y - start.y);

        if (totalDistance < _minTotalDistance || heightDifference < _minDistanceY)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, _maxDistance, _hookableLayer);

        if (hit.collider == null)
            return false;

        _hookRoutine = _coroutineRunner.StartCoroutine(HookRoutine(cursorWorldPosition, hit));
        return true;
    }


    public void StopHook()
    {
        _isHooking = false;

        if (_hookRoutine != null)
        {
            _coroutineRunner.StopCoroutine(_hookRoutine);
            _hookRoutine = null;
        }

        _lineRenderer.enabled = false;
    }

    private IEnumerator HookRoutine(Vector2 cursorPosition, RaycastHit2D hit)
    {
        _lineRenderer.enabled = true;
        _isHooking = true;
        Vector2 hookPoint = hit.point;

        while (_isHooking)
        {
            Vector2 playerPos = _rigidbody.position;
            Vector2 toHook = (hookPoint - playerPos).normalized;

            _rigidbody.linearVelocity = Vector2.zero; 
            _rigidbody.AddForce(toHook * _pullForce, ForceMode2D.Force);

            _lineRenderer.SetPosition(0, _hookStartPoint.position);
            _lineRenderer.SetPosition(1, hookPoint);

            if (Vector2.Distance(playerPos, hookPoint) < _collider.bounds.size.x * 2)
            {
                _rigidbody.AddForce(Vector2.up * _pullForce * _pullFinalForce, ForceMode2D.Impulse);
                break;
            }

            yield return null;
        }

        StopHook();
    }
}
