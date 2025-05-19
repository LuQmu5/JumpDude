using System;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 5;

    [Header("Jump Settings")]
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _legsRadius = 1;
    [SerializeField] private float _maxJumpHoldTime = 2f;

    [Header("Dash Settings")]
    [SerializeField] private float _dashForce = 25f;
    [SerializeField] private float _dashDuration = 0.15f;
    [SerializeField] private float _dashCooldown = 0.75f;

    private PlayerController _controller;

    private CharacterMover _mover;
    private GroundChecker _groundChecker;
    private CharacterJumper _jumper;
    private CharacterDasher _dasher;

    private CharacterView _view;

    public Vector2 InputDirection { get; private set; }
    public bool CanMove => _dasher.IsDashing == false &&
        _rigidbody.linearVelocityY >= 0 &&
        _jumper.IsCharging == false;

    private void Awake()
    {
        _controller = new PlayerController();

        _groundChecker = new GroundChecker(_legsPoint, _legsRadius, _groundMask);

        _mover = new CharacterMover(_rigidbody, _movementSpeed, _groundChecker);
        _jumper = new CharacterJumper(_groundChecker, _rigidbody, _jumpPower, _maxJumpHoldTime);
        _dasher = new CharacterDasher(this, _rigidbody, _dashForce, _dashDuration, _dashCooldown);

        _view = new CharacterView(_spriteRenderer, _animator);
    }

    private void OnEnable()
    {
        _controller.Enable();

        _controller.Player.Move.performed += ctx => InputDirection = ctx.ReadValue<Vector2>();
        _controller.Player.Move.canceled += _ => InputDirection = Vector2.zero;

        _controller.Player.Jump.started += _ => StartCharge();
        _controller.Player.Jump.canceled += _ => _jumper.ReleaseJump();

        _controller.Player.Dash.performed += _ => Dash();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    private void Update()
    {
        UpdateView();
        UpdateJumper();

        if (CanMove)
            _mover.SetMoveDirection(InputDirection);
    }

    private void StartCharge()
    {
        _mover.Stop();
        _jumper.StartCharge();
    }

    private void UpdateJumper()
    {
        _jumper.UpdateCharge(Time.deltaTime);
        _jumper.UpdateJumpDirection(InputDirection);
    }

    private void UpdateView()
    {
        _view.UpdateLookDirection(InputDirection);
        _view.UpdateVelocityParams(_rigidbody.linearVelocity);
        _view.UpdateOnGroundParam(_groundChecker.OnGround());
        _view.UpdateJumpChargingParam(_jumper.IsCharging);
    }

    private void Dash()
    {
        Vector2 direction = InputDirection == Vector2.zero? _view.LookDirection : InputDirection.normalized;

        if (_dasher.TryDash(direction))
        {
            _view.SetDashTrigger();
        }
    }
}
