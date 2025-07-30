using System.Collections;
using UnityEngine;

public class GlideHandler
{
    private Rigidbody2D _rigidbody;
    private float _baseGravityScale;
    private GroundChecker _groundChecker;
    private MonoBehaviour _coroutineRunner;

    private float _glideGravityScale = 0.3f;
    private float _glideMovementSpeedMultiplier = 0.65f;

    public float GlideMovementSpeedMultiplier => _glideMovementSpeedMultiplier;

    public GlideHandler(Rigidbody2D rigidbody, float baseGravityScale, MonoBehaviour coroutineRunner, GroundChecker groundChecker)
    {
        _rigidbody = rigidbody;
        _baseGravityScale = baseGravityScale;
        _coroutineRunner = coroutineRunner;
        _groundChecker = groundChecker;
    }

    public bool IsGliding { get; private set; }

    public void StartGlide()
    {
        IsGliding = true;
        _coroutineRunner.StartCoroutine(ChangeGravityScale(_rigidbody.gravityScale, _glideGravityScale, 0.25f));
        _coroutineRunner.StartCoroutine(GlideProcessing());
    }

    public void StopGlide()
    {
        IsGliding = false;
        _coroutineRunner.StartCoroutine(ChangeGravityScale(_rigidbody.gravityScale, _baseGravityScale, 0.25f));
    }

    private IEnumerator ChangeGravityScale(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _rigidbody.gravityScale = Mathf.Lerp(from, to, elapsed / duration);
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