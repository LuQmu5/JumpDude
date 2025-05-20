using System;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("VFX & Points")]
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private ParticleSystem _doubleJumpVFX;
    [SerializeField] private SpriteRenderer[] _shadowRenderers;
    [SerializeField] private TrailRenderer _dashTrailVFX;

    [Header("Settings")]
    [SerializeField] private CharacterSettings _characterSettings;

    [Header("Glide Settings")]
    [SerializeField] private float _glideGravityScale = 0.5f;
    [SerializeField] private float _glideSpeedMultiplier = 2f;

    private CharacterGlider _glider;

    private PlayerController _controller;
    private CharacterMover _mover;
    private GroundChecker _groundChecker;
    private DoubleJumpHandler _doubleJumpHandler;
    private CharacterJumper _jumper;
    private CharacterDasher _dasher;
    private CharacterView _view;

    public Vector2 InputDirection { get; private set; }
    public bool CanMove => !_dasher.IsDashing && (_rigidbody.linearVelocityY >= 0 || _glider.IsGliding);

    private void Awake()
    {
        _controller = new PlayerController();

        var jump = _characterSettings.jumpSettings;
        var dash = _characterSettings.dashSettings;
        var move = _characterSettings.movementSettings;

        DashHitDetector hitDetector = new DashHitDetector(transform, dash.dashHitRadius, dash.enemyLayer);

        _view = new CharacterView(_spriteRenderer, _animator, _shadowRenderers, this, _doubleJumpVFX, _dashTrailVFX, hitDetector);

        _groundChecker = new GroundChecker(_legsPoint, jump.legsRadius, jump.groundMask);
        _doubleJumpHandler = new DoubleJumpHandler(_groundChecker, jump.doubleJumpTimer);

        _mover = new CharacterMover(_rigidbody, move.movementSpeed, _groundChecker, move.maxSpeedMultiplier, move.accelerationTime);
        _jumper = new CharacterJumper(_groundChecker, _rigidbody, jump.jumpPower, jump.maxJumpHoldTime, _doubleJumpHandler);
        _dasher = new CharacterDasher(this, _rigidbody, dash.minDashForce, dash.maxDashForce, dash.dashDuration, dash.dashCooldown, dash.dashChargeTime, _view.PlayDashEffect);
        _glider = new CharacterGlider(_rigidbody, _groundChecker, _glideGravityScale, _glideSpeedMultiplier);

    }

    private void OnEnable()
    {
        _controller.Enable();

        _controller.Player.Move.performed += ctx => InputDirection = ctx.ReadValue<Vector2>();
        _controller.Player.Move.canceled += _ => InputDirection = Vector2.zero;

        _controller.Player.Jump.started += _ => TryStartJumpCharge();
        _controller.Player.Jump.canceled += _ => TryReleaseJump();

        _controller.Player.DoubleJump.performed += _ => TryDoubleJump();

        _controller.Player.Dash.started += _ => TryStartDashCharge();
        _controller.Player.Dash.canceled += _ => TryReleaseDash();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    private void Update()
    {
        _doubleJumpHandler.Update(Time.deltaTime);

        _jumper.UpdateCharge(Time.deltaTime);
        _jumper.UpdateJumpDirection(InputDirection);

        _dasher.UpdateDashCharge(Time.deltaTime);

        bool jumpHeld = _controller.Player.Jump.ReadValue<float>() > 0;
        _glider.Update(jumpHeld, InputDirection);

        UpdateView();

        if (CanMove)
            _mover.SetMoveDirection(InputDirection);
    }


    private void TryStartJumpCharge() => _jumper.TryStartCharge();

    private void TryReleaseJump()
    {
        if (_jumper.TryReleaseJump() && _doubleJumpHandler.UsedDoubleJump)
            _view.PlayDoubleJumpEffect();
    }

    private bool TryDoubleJump()
    {
        if (!_groundChecker.OnGround() && _doubleJumpHandler.CanDoubleJump)
        {
            _jumper.PerformDoubleJump();
            _doubleJumpHandler.MarkUsed();
            _view.PlayDoubleJumpEffect();
            return true;
        }
        return false;
    }

    private void TryStartDashCharge()
    {
        Vector2 direction = InputDirection == Vector2.zero ? _view.LookDirection : InputDirection.normalized;
        _dasher.StartDashCharge(direction);
    }

    private void TryReleaseDash()
    {
        if (_dasher.IsCharging)
        {
            _dasher.ReleaseDash();
            _view.SetDashTrigger();
            _view.PlayDashEffect(_characterSettings.dashSettings.dashDuration);
        }
    }

    private void UpdateView()
    {
        _view.UpdateLookDirection(InputDirection);
        _view.UpdateVelocityParams(_rigidbody.linearVelocity);
        _view.UpdateOnGroundParam(_groundChecker.OnGround());
        _view.UpdateJumpChargingParam(_jumper.IsCharging);
        _view.UpdateGlideParam(_glider.IsGliding);
    }
}
