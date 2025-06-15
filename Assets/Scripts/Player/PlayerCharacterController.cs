using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _legsPoint;
    [SerializeField] private LayerMask _groundMask;

    private IInputHandler _input;
    private IGroundChecker _groundChecker;
    private IMover _moveHandler;
    private IJumper _jumpHandler;

    private void Awake()
    {
        PlayerKeyboardInput input = new PlayerKeyboardInput();
        input.Enable();

        _input = new KeyboardInputHandler(input);

        _groundChecker = new LayerGroundChecker(_legsPoint, legsRadius: 0.1f, _groundMask);
        _moveHandler = new PlayerPhysicsMovement(_rigidbody, this);
        _jumpHandler = new PlayerPhysicsJump();
    }

    private void Update()
    {
        
    }
}

public interface IJumper
{
    public void Jump(float jumpPower, Vector2 direction);
}

public class PlayerPhysicsJump : IJumper
{
    public void Jump(float jumpPower, Vector2 direction)
    {
        throw new System.NotImplementedException();
    }
}