using System;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _maxSpeedMultiplier = 5;
    [SerializeField] private float _accelerationTime = 5;

    [Header("Jump Settings")]
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _legsRadius = 1;
    [SerializeField] private float _maxJumpHoldTime = 2f;
    [SerializeField] private float _doubleJumpTimer = 0.5f;
    [SerializeField] private ParticleSystem _doubleJumpVFX;

    [Header("Dash Settings")]
    [SerializeField] private float _minDashForce = 10f;
    [SerializeField] private float _maxDashForce = 25f;
    [SerializeField] private float _dashDuration = 0.15f;
    [SerializeField] private float _dashCooldown = 0.75f;
    [SerializeField] private float _dashChargeTime = 1f;
    [SerializeField] private SpriteRenderer[] _shadowRenderers;

    private PlayerController _controller;

    private CharacterMover _mover;
    private GroundChecker _groundChecker;
    private DoubleJumpHandler _doubleJumpHandler;
    private CharacterJumper _jumper;
    private CharacterDasher _dasher;
    private CharacterView _view;

    public Vector2 InputDirection { get; private set; }
    public bool CanMove => !_dasher.IsDashing && _rigidbody.linearVelocityY >= 0;

    private void Awake()
    {
        _controller = new PlayerController();

        _view = new CharacterView(_spriteRenderer, _animator, _shadowRenderers, this, _doubleJumpVFX);

        _groundChecker = new GroundChecker(_legsPoint, _legsRadius, _groundMask);
        _doubleJumpHandler = new DoubleJumpHandler(_groundChecker, _doubleJumpTimer);

        _mover = new CharacterMover(_rigidbody, _movementSpeed, _groundChecker, _maxSpeedMultiplier, _accelerationTime);
        _jumper = new CharacterJumper(_groundChecker, _rigidbody, _jumpPower, _maxJumpHoldTime, _doubleJumpHandler);
        _dasher = new CharacterDasher(this, _rigidbody, _minDashForce, _maxDashForce, _dashDuration, _dashCooldown, _dashChargeTime, _view.PlayDashEffect);
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

        UpdateView();

        if (CanMove)
            _mover.SetMoveDirection(InputDirection);
    }

    private void TryStartJumpCharge()
    {
        _jumper.TryStartCharge();
    }

    private void TryReleaseJump()
    {
        if (_jumper.TryReleaseJump() && _doubleJumpHandler.UsedDoubleJump)
        {
            _view.PlayDoubleJumpEffect();
        }
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
            _view.PlayDashEffect(_dashDuration);
        }
    }

    private void UpdateView()
    {
        _view.UpdateLookDirection(InputDirection);
        _view.UpdateVelocityParams(_rigidbody.linearVelocity);
        _view.UpdateOnGroundParam(_groundChecker.OnGround());
        _view.UpdateJumpChargingParam(_jumper.IsCharging);
    }
}
