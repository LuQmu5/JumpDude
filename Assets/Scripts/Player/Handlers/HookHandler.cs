using System.Collections;
using UnityEngine;

public class HookHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Transform _hookStartPoint;
    private readonly Transform _hookVisual;
    private readonly LayerMask _hookableLayer;
    private readonly float _pullForce;
    private readonly float _pullFinalForce;
    private readonly float _maxDistance;
    private readonly float _minDistanceY;
    private readonly float _minTotalDistance;
    private readonly float _hookSpeed;
    private readonly LineRenderer _lineRenderer;
    private readonly Collider2D _collider;

    private Coroutine _hookRoutine;
    private bool _isHooking;

    public bool IsHooking => _isHooking;

    public HookHandler(
        HookConfig config,
        Rigidbody2D rigidbody,
        Transform hookStartPoint,
        Transform hookVisual,
        MonoBehaviour coroutineRunner,
        LineRenderer lineRenderer,
        Collider2D collider)
    {
        _rigidbody = rigidbody;
        _hookVisual = hookVisual;
        _coroutineRunner = coroutineRunner;
        _hookStartPoint = hookStartPoint;
        _hookableLayer = config.HookableLayer;
        _pullForce = config.PullForce;
        _pullFinalForce = config.PullFinalForce;
        _maxDistance = config.MaxDistance;
        _minDistanceY = config.MinDistanceY;
        _minTotalDistance = config.MinTotalDistance;
        _hookSpeed = config.HookSpeed;
        _collider = collider;
        _lineRenderer = lineRenderer;
        _lineRenderer.enabled = false;

        _hookVisual.gameObject.SetActive(false);
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

        _hookRoutine = _coroutineRunner.StartCoroutine(HookRoutine(hit.point));
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
        _hookVisual.gameObject.SetActive(false);
    }

    private IEnumerator HookRoutine(Vector2 hookPoint)
    {
        _lineRenderer.enabled = true;
        _hookVisual.gameObject.SetActive(true);

        _isHooking = true;

        Vector2 start = _hookStartPoint.position;
        Vector2 current = start;

        while (Vector2.Distance(current, hookPoint) > 0.1f)
        {
            current = Vector2.MoveTowards(current, hookPoint, _hookSpeed * Time.deltaTime);
            _hookVisual.position = current;

            Vector2 direction = _hookVisual.transform.position - _hookStartPoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _hookVisual.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            _lineRenderer.SetPosition(0, _hookStartPoint.position);
            _lineRenderer.SetPosition(1, current);

            yield return null;
        }

        while (_isHooking)
        {
            Vector2 playerPos = _rigidbody.position;
            Vector2 toHook = (hookPoint - playerPos).normalized;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(toHook * _pullForce, ForceMode2D.Force);

            _lineRenderer.SetPosition(0, _hookStartPoint.position);
            _lineRenderer.SetPosition(1, hookPoint);
            _hookVisual.position = hookPoint;

            if (Vector2.Distance(playerPos, hookPoint) < _collider.bounds.size.x * 2f)
            {
                _rigidbody.AddForce(Vector2.up * _pullForce * _pullFinalForce, ForceMode2D.Impulse);
                break;
            }

            yield return null;
        }

        StopHook();
    }
}
