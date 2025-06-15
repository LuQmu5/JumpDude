using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private PlayerState _currentState;
    public PlayerState CurrentState => _currentState;

    public void Initialize(PlayerState startState)
    {
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void Update()
    {
        _currentState?.HandleInput();
        _currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _currentState?.PhysicsUpdate();
    }
}
