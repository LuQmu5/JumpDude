using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView _view;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _legs;
    [SerializeField] private DashEffect _dashEffect;
    [SerializeField] private CharacterConfig _characterConfig;

    private PlayerInput _input;
    private GroundChecker _groundChecker;
    private GravityHandler _gravityHandler;
    private MovementHandler _movementHandler;
    private JumpHandler _jumpHandler;
    private GlideHandler _glideHandler;
    private DashHandler _dashHandler;
    private FallHandler _fallHandler;

    private Vector3 _rightRotation = Vector3.zero;
    private Vector3 _leftRotation = new Vector3(0, 180, 0);

    public void Init()
    {
        _groundChecker = new GroundChecker(_characterConfig.GroundCheckConfig, _legs);
        _gravityHandler = new GravityHandler(_rigidbody, _groundChecker);
        _movementHandler = new MovementHandler(_characterConfig.MovementConfig, _rigidbody, this);
        _jumpHandler = new JumpHandler(_characterConfig.JumpConfig, _rigidbody, _groundChecker, this);
        _glideHandler = new GlideHandler(_characterConfig.GlideConfig, _rigidbody, this, _groundChecker);
        _dashHandler = new DashHandler(_characterConfig.DashConfig, _rigidbody, this, _dashEffect);
        _fallHandler = new FallHandler(_characterConfig.FallConfig, _rigidbody, GetComponent<Collider2D>(), _legs, _groundChecker, this);

        _input = new PlayerInput();
        _input.Enable();

        _input.Movement.Jump.started += OnJumpStarted;
        _input.Movement.Jump.canceled += OnJumpPeformed;

        _input.Movement.Dash.started += OnDashStarted;
        _input.Movement.Dash.canceled += OnDashPerformed;

        _input.Movement.Fall.started += OnFallStarted;
        _input.Movement.Fall.canceled += OnFallPerformed;
    }

    private void Update()
    {
        _gravityHandler.HandleGravity(_fallHandler.IsFalling);

        _movementHandler.IsGliding = _glideHandler.IsGliding;
        _movementHandler.UpdateFallState();

        HandleMovement();
        UpdateView();
    }


    private void UpdateView()
    {
        _view.SetVelocityX(Mathf.Abs(_rigidbody.linearVelocityX));
        _view.SetVelocityY(_rigidbody.linearVelocityY);
        _view.SetOnGroundState(_groundChecker.OnGround());
        _view.SetGlidingState(_glideHandler.IsGliding);
        _view.SetFallState(_fallHandler.IsFalling);
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

    private void OnJumpStarted(CallbackContext context)
    {
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
        _dashHandler.StartDash();
    }

    private void OnDashPerformed(CallbackContext context)
    {
        if (_dashHandler.TryReleaseDash())
            _view.SetDashTrigger();
    }

    private void OnFallPerformed(CallbackContext context)
    {
        _fallHandler.StartFall();
    }

    private void OnFallStarted(CallbackContext context)
    {
        _fallHandler.StopFall();
    }

    private void RotateFromVelocity(float velocity)
    {
        if (velocity > 0)
            transform.eulerAngles = _rightRotation;
        if (velocity < 0)
            transform.eulerAngles = _leftRotation;
    }
}
