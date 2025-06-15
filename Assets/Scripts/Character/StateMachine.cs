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
        Debug.Log($"Exit from state: {_currentState.GetType()}");
        _currentState = newState;
        _currentState.Enter();
        Debug.Log($"Enter to new state: {_currentState.GetType()}");
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
