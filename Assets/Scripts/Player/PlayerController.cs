using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerView _view;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _legs;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _jumpPower = 10;
    [SerializeField] private float _dashPower = 10;
    [SerializeField] private float _dashCooldown = 1;
    [SerializeField] private DashEffect _dashEffect;

    private PlayerInput _input;
    private GroundChecker _groundChecker;
    private GravityHandler _gravityHandler;
    private MovementHandler _movementHandler;
    private JumpHandler _jumpHandler;
    private GlideHandler _glideHandler;
    private DashHandler _dashHandler;

    private Vector3 _rightRotation = Vector3.zero;
    private Vector3 _leftRotation = new Vector3(0, 180, 0);

    public void Init()
    {
        _groundChecker = new GroundChecker(_groundMask, _legs);
        _gravityHandler = new GravityHandler(_rigidbody, _groundChecker);
        _movementHandler = new MovementHandler(_movementSpeed, _rigidbody, this);
        _jumpHandler = new JumpHandler(_jumpPower, _rigidbody, _groundChecker, this);
        _glideHandler = new GlideHandler(_rigidbody, _rigidbody.gravityScale, this, _groundChecker);
        _dashHandler = new DashHandler(_rigidbody, _dashPower, _dashCooldown, this, _dashEffect);

        _input = new PlayerInput();
        _input.Enable();

        _input.Movement.Jump.started += OnJumpStarted;
        _input.Movement.Jump.canceled += OnJumpPeformed;

        _input.Movement.Dash.started += OnDashStarted;
        _input.Movement.Dash.canceled += OnDashPerformed;
    }

    private void Update()
    {
        _gravityHandler.HandleGravity();

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

    private void RotateFromVelocity(float velocity)
    {
        if (velocity > 0)
            transform.eulerAngles = _rightRotation;
        if (velocity < 0)
            transform.eulerAngles = _leftRotation;
    }
}
