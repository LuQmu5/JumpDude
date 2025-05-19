using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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

    public Vector2 MoveInput { get; private set; }

    private void Awake()
    {
        _controller = new PlayerController();
        _mover = new CharacterMover(_rigidbody, _movementSpeed);
        _groundChecker = new GroundChecker(_legsPoint, _legsRadius, _groundMask);
        _jumper = new CharacterJumper(_groundChecker, _rigidbody, _jumpPower, _maxJumpHoldTime);
        _dasher = new CharacterDasher(this, _rigidbody, _dashForce, _dashDuration, _dashCooldown);
        _view = new CharacterView(_spriteRenderer);
    }

    private void OnEnable()
    {
        _controller.Enable();

        _controller.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controller.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        _controller.Player.Jump.started += _ => _jumper.StartCharge();
        _controller.Player.Jump.canceled += _ => _jumper.ReleaseJump();

        _controller.Player.Dash.performed += _ => Dash();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    private void Update()
    {
        if (_dasher.IsDashing == false)
            _mover.SetInputDirection(MoveInput);

        _view.FlipByDirection(MoveInput);
        _jumper.UpdateCharge(Time.deltaTime);
    }

    private void Dash()
    {
        Vector2 direction = MoveInput != Vector2.zero ? MoveInput.normalized : Vector2.right;
        _dasher.TryDash(direction);
    }
}
