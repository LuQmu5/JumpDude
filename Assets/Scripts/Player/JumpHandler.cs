using System.Collections;
using UnityEngine;

public class JumpHandler
{
    private Rigidbody2D _rigidbody;
    private GroundChecker _groundChecker;
    private MonoBehaviour _coroutineRunner;

    private float _jumpPower;
    private int _extraJumpsCount;
    private int _maxExtraJumpsCount = 1;
    private float _minHoldTime = 0.25f;
    private float _maxHoldTime = 0.5f;
    private float _jumpHoldStartTime;

    public JumpHandler(float jumpPower, Rigidbody2D rigidbody, GroundChecker groundChecker, MonoBehaviour coroutineRunner)
    {
        _jumpPower = jumpPower;
        _rigidbody = rigidbody;
        _groundChecker = groundChecker;
        _coroutineRunner = coroutineRunner;

        _extraJumpsCount = _maxExtraJumpsCount;
    }

    public void StartJump()
    {
        _jumpHoldStartTime = Time.time;
    }

    public bool TryReleaseJump()
    {
        if (CanJump() == false)
        {
            return false;
        }

        float heldTime = Time.time - _jumpHoldStartTime;
        float clampedHold = Mathf.Clamp(heldTime, _minHoldTime, _maxHoldTime);
        float powerPercent = clampedHold / _maxHoldTime;

        float finalJumpForce = _jumpPower * powerPercent;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, 0f);
        _rigidbody.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);

        return true;
    }

    private IEnumerator RefreshingExtraJump()
    {
        yield return new WaitUntil(() => _groundChecker.OnGround());
        _extraJumpsCount = _maxExtraJumpsCount;
    }

    private bool CanJump()
    {
        if (_groundChecker.OnGround())
        {
            return true;
        }
        else if (_extraJumpsCount > 0 && _rigidbody.linearVelocityY > 0)
        {
            _extraJumpsCount--;
            _coroutineRunner.StartCoroutine(RefreshingExtraJump());
            return true;
        }

        return false;
    }
}

