using System.Collections;
using UnityEngine;

public class GlideHandler
{
    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;
    private MonoBehaviour _coroutineRunner;

    private float _glideGravityScale;
    private float _glideMovementSpeedMultiplier;
    private float _changeGravityDuration;
    private float _instantGravityScaleMultiplier;

    private float _baseGravityScale;

    public float GlideMovementSpeedMultiplier => _glideMovementSpeedMultiplier;

    public GlideHandler(GlideConfig config, Rigidbody2D rigidbody, MonoBehaviour coroutineRunner, GroundChecker groundChecker)
    {
        _glideGravityScale = config.ModifiedGravityScale;
        _glideMovementSpeedMultiplier = config.MovementSpeedMultiplier;
        _changeGravityDuration = config.ChangeGravityDuration;
        _instantGravityScaleMultiplier = config.InstantGravityScaleMultiplier;

        _rigidbody = rigidbody;
        _baseGravityScale = _rigidbody.gravityScale;
        _coroutineRunner = coroutineRunner;
        _groundChecker = groundChecker;
    }

    public bool IsGliding { get; private set; }

    public void StartGlide()
    {
        IsGliding = true;
        _coroutineRunner.StartCoroutine(ChangeGravityScale(_rigidbody.gravityScale, _glideGravityScale, _changeGravityDuration));
        _coroutineRunner.StartCoroutine(GlideProcessing());
    }

    public void StopGlide()
    {
        IsGliding = false;
        _coroutineRunner.StartCoroutine(ChangeGravityScale(_rigidbody.gravityScale, _baseGravityScale, _changeGravityDuration));
    }

    private IEnumerator ChangeGravityScale(float from, float to, float duration)
    {
        float mid = Mathf.Lerp(from, to, _instantGravityScaleMultiplier);
        _rigidbody.gravityScale = mid;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            _rigidbody.gravityScale = Mathf.Lerp(mid, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigidbody.gravityScale = to;
    }


    private IEnumerator GlideProcessing()
    {
        yield return new WaitUntil(() => IsGliding == false || _groundChecker.OnGround());

        StopGlide();
    }
}