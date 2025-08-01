using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView _view;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private Transform _hookPoint;
    [SerializeField] private Transform _hookVisual;
    [SerializeField] private DashEffect _dashEffect;
    [SerializeField] private CharacterConfig _characterConfig;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private TrailRenderer _fallFastTrail;
    [SerializeField] private LineRenderer _hookRenderer;
    [SerializeField] private KillAreaTrigger _killAreaTriggerDash;
    [SerializeField] private KillAreaTrigger _killAreaTriggerFastFall;

    private PlayerInput _input;
    private GroundChecker _groundChecker;
    private GravityHandler _gravityHandler;
    private MovementHandler _movementHandler;
    private JumpHandler _jumpHandler;
    private GlideHandler _glideHandler;
    private DashHandler _dashHandler;
    private FastFallHandler _fallHandler;
    private HookHandler _hookHandler;
    private FallTimeSlowHandler _fallTimeSlowHandler;

    private Vector3 _rightRotation = Vector3.zero;
    private Vector3 _leftRotation = new Vector3(0, 180, 0);

    private bool _inAction = false;

    public void Init(PlayerInput input)
    {
        _fallHandler = new FastFallHandler(_characterConfig.FallConfig, _rigidbody, _collider, _legsPoint, this, _fallFastTrail, _killAreaTriggerFastFall);
        _groundChecker = new GroundChecker(_characterConfig.GroundCheckConfig, _legsPoint);
        _gravityHandler = new GravityHandler(_rigidbody, _groundChecker);
        _movementHandler = new MovementHandler(_characterConfig.MovementConfig, _rigidbody, this);
        _jumpHandler = new JumpHandler(_characterConfig.JumpConfig, _rigidbody, _groundChecker, this);
        _glideHandler = new GlideHandler(_characterConfig.GlideConfig, _rigidbody, this, _groundChecker);
        _dashHandler = new DashHandler(_characterConfig.DashConfig, _rigidbody, this, _dashEffect, _killAreaTriggerDash);
        _hookHandler = new HookHandler(_characterConfig.HookConfig, _rigidbody, _hookPoint, _hookVisual, this, _hookRenderer, _collider);
        _fallTimeSlowHandler = new FallTimeSlowHandler(_rigidbody, this, _characterConfig.FallTimeSlowConfig, _groundChecker);

        _input = input;
        _input.Enable();

        _input.Movement.Jump.started += OnJumpStarted;
        _input.Movement.Jump.canceled += OnJumpPeformed;

        _input.Movement.Dash.started += OnDashStarted;
        _input.Movement.Dash.canceled += OnDashPerformed;

        _input.Movement.FastFall.started += OnFastFallStarted;
        _input.Movement.FastFall.canceled += OnFastFallCanceled;

        _input.Movement.Hook.started += OnHookStarted;
        _input.Movement.Hook.canceled += OnHookCanceled;
    }

    private void Update()
    {
        _gravityHandler.HandleGravity(_fallHandler.IsFalling);

        _movementHandler.IsGliding = _glideHandler.IsGliding;
        _movementHandler.UpdateFallState();

        if (_glideHandler.IsGliding == false)
            _fallTimeSlowHandler.Update();

        HandleMovement();
        UpdateView();
    }


    private void UpdateView()
    {
        _view.SetVelocityX(Mathf.Abs((int)_rigidbody.linearVelocityX));
        _view.SetVelocityY((int)_rigidbody.linearVelocityY);
        _view.SetOnGroundState(_groundChecker.OnGround());
        _view.SetGlidingState(_glideHandler.IsGliding);
        _view.SetFastFallState(_fallHandler.IsFalling);
        _view.SetHookState(_hookHandler.IsHooking);
    }

    private void HandleMovement()
    {
        if (_dashHandler.IsDashing)
            return;

        float horizontalInput = _input.Movement.Move.ReadValue<float>();
        float glideMultiplier = _glideHandler.IsGliding ? _glideHandler.GlideMovementSpeedMultiplier : 1;

        _movementHandler.SetHorizontalVelocity(horizontalInput * glideMultiplier);
        RotateFromVelocity(horizontalInput);
    }

    private void OnHookCanceled(CallbackContext context)
    {
        _hookHandler.StopHook();
        _inAction = false;
    }

    private void OnHookStarted(CallbackContext context)
    {
        if (_inAction || _glideHandler.IsGliding)
            return;

        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_hookHandler.StartHook(direction))
        {
            RotateFromVelocity(direction.x - transform.position.x);
            _inAction = true;
        }
    }

    private void OnJumpStarted(CallbackContext context)
    {
        if (_inAction)
            return;

        if (_rigidbody.linearVelocityY >= 0)
            _jumpHandler.StartJump();
        else
            _glideHandler.StartGlide();
    }

    private void OnJumpPeformed(CallbackContext context)
    {
        if (_jumpHandler.TryReleaseJump())
        {
            _view.SetJumpTrigger();
        }
        else if (_glideHandler.IsGliding)
        {
            _glideHandler.StopGlide();
        }
    }

    private void OnDashStarted(CallbackContext context)
    {
        if (_inAction)
            return;

        _dashHandler.StartDash();
    }

    private void OnDashPerformed(CallbackContext context)
    {
        if (_dashHandler.TryReleaseDash())
            _view.SetDashTrigger();
    }

    private void OnFastFallCanceled(CallbackContext context)
    {
        _fallHandler.StopFastFall();
        _inAction = false;
    }

    private void OnFastFallStarted(CallbackContext context)
    {
        if (_inAction)
            return;

        _inAction = true;
        _fallHandler.StartFastFall();
    }

    private void RotateFromVelocity(float velocity)
    {
        if (velocity > 0)
            transform.eulerAngles = _rightRotation;
        if (velocity < 0)
            transform.eulerAngles = _leftRotation;
    }

    public void Deactivate()
    {
        _inAction = false;
        _rigidbody.linearVelocity = Vector2.zero;
        _killAreaTriggerDash.Deactivate();
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
