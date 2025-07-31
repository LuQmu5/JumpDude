using UnityEngine;
using System.Collections;

public class FallTimeSlowHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly MonoBehaviour _coroutineRunner;
    private readonly FallTimeSlowConfig _config;
    private readonly GroundChecker _groundChecker;

    private Coroutine _slowRoutine;
    private bool _isFalling;
    private bool _isSlowing;

    public FallTimeSlowHandler(
        Rigidbody2D rigidbody,
        MonoBehaviour coroutineRunner,
        FallTimeSlowConfig config,
        GroundChecker groundChecker)
    {
        _rigidbody = rigidbody;
        _coroutineRunner = coroutineRunner;
        _config = config;
        _groundChecker = groundChecker;
    }

    public void Update()
    {
        bool shouldFallSlow =
            _rigidbody.linearVelocity.y < _config.MinFallVelocityY &&
            !_groundChecker.OnGround() &&
            !IsNearGround();

        if (shouldFallSlow)
        {
            if (!_isFalling)
            {
                _isFalling = true;
                StartSlowdown();
            }
        }
        else if (_isFalling)
        {
            _isFalling = false;
            StopSlowdown();
        }
    }

    private void StartSlowdown()
    {
        if (_isSlowing) return;
        _slowRoutine = _coroutineRunner.StartCoroutine(SlowRoutine());
    }

    private void StopSlowdown()
    {
        if (_slowRoutine != null)
        {
            _coroutineRunner.StopCoroutine(_slowRoutine);
            _slowRoutine = null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        _isSlowing = false;
    }

    private IEnumerator SlowRoutine()
    {
        _isSlowing = true;

        float time = 0f;
        float slowElapsed = 0f;

        Time.timeScale = _config.InitialTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        while (_isFalling)
        {
            if (_groundChecker.OnGround() || IsNearGround())
                break;

            time += Time.unscaledDeltaTime;
            slowElapsed += Time.unscaledDeltaTime;

            // ограничение времени действия
            if (slowElapsed >= _config.MaxSlowDuration)
                break;

            float t = Mathf.Clamp01(time / _config.TransitionDuration);
            float scale = Mathf.Lerp(_config.InitialTimeScale, _config.FinalTimeScale, t);
            Time.timeScale = scale;
            Time.fixedDeltaTime = 0.02f * scale;

            yield return null;
        }

        StopSlowdown();
    }

    private bool IsNearGround()
    {
        Vector2 origin = _rigidbody.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _config.GroundProximityDistance, _config.GroundLayer);
        return hit.collider != null;
    }
}
