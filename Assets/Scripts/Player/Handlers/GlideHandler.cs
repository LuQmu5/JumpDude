using System.Collections;
using UnityEngine;

public class GlideHandler
{
    private readonly Rigidbody2D _rigidbody;
    private readonly GroundChecker _groundChecker;
    private readonly MonoBehaviour _coroutineRunner;

    private readonly float _glideGravityScale;
    private readonly float _glideMovementSpeedMultiplier;
    private readonly float _baseGravityScale;

    public float GlideMovementSpeedMultiplier => _glideMovementSpeedMultiplier;

    public bool IsGliding { get; private set; }

    public GlideHandler(
        GlideConfig config,
        Rigidbody2D rigidbody,
        MonoBehaviour coroutineRunner,
        GroundChecker groundChecker)
    {
        _glideGravityScale = config.ModifiedGravityScale;
        _glideMovementSpeedMultiplier = config.MovementSpeedMultiplier;

        _rigidbody = rigidbody;
        _baseGravityScale = _rigidbody.gravityScale;
        _coroutineRunner = coroutineRunner;
        _groundChecker = groundChecker;
    }

    public void StartGlide()
    {
        if (IsGliding)
            return;

        IsGliding = true;
        _rigidbody.gravityScale = _glideGravityScale;
        _coroutineRunner.StartCoroutine(GlideProcessing());
    }

    public void StopGlide()
    {
        if (!IsGliding)
            return;

        IsGliding = false;
        _rigidbody.gravityScale = _baseGravityScale;
    }

    private IEnumerator GlideProcessing()
    {
        yield return new WaitUntil(() => !IsGliding || _groundChecker.OnGround());
        StopGlide();
    }
}
