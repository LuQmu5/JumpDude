using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _glideFallSpeed = 1f;
    [SerializeField] private float _wallJumpForce = 10f;
    [SerializeField] private LayerMask _groundLayer;

    public LayerMask wallLayer;
    public StateMachine stateMachine;

    public bool IsTouchingWall => Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.5f, wallLayer);

    public Rigidbody2D rb { get; private set; }

    public IInputReader input { get; private set; }

    public bool IsGrounded => Physics2D.Raycast(transform.position, Vector2.down, 0.1f, _groundLayer);
    public bool IsFalling => rb.linearVelocityY < -0.1f;
    public bool IsRising => rb.linearVelocityY > 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<IInputReader>();
    }

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        stateMachine.Initialize(new GroundedState(this, stateMachine));
    }

    public void Move(Vector2 dir)
    {
        rb.linearVelocity = new Vector2(dir.x * _moveSpeed, rb.linearVelocityY);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, _jumpForce);
    }

    public void Dash(float strength)
    {
        rb.linearVelocity = new Vector2(transform.localScale.x * _dashForce * strength, 0);
    }

    public void Glide()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, -_glideFallSpeed);
    }

    public void FallFast()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, -_jumpForce * 2f);
    }

    public void WallJump(Vector2 normal)
    {
        rb.linearVelocity = new Vector2(normal.x * _wallJumpForce, _wallJumpForce);
    }
}
