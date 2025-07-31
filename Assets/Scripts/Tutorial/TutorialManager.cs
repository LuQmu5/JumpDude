using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum TutorialActions
{
    Move,
    Jump,
    Dash,
    Hook,
    FastFall
}

public class TutorialManager
{
    private readonly HintDisplay _hintDisplay;
    private readonly PlayerInput _playerInput;

    private Dictionary<TutorialActions, InputAction> _actionMap;
    private InputAction _currentAction;

    public static TutorialManager Singleton { get; private set; }

    public TutorialManager(HintDisplay hintDisplay, PlayerInput playerInput)
    {
        Singleton = this;

        _hintDisplay = hintDisplay;
        _playerInput = playerInput;

        _actionMap = new Dictionary<TutorialActions, InputAction>()
        {
            [TutorialActions.Move] = _playerInput.Movement.Move,
            [TutorialActions.Jump] = _playerInput.Movement.Jump,
            [TutorialActions.Dash] = _playerInput.Movement.Dash,
            [TutorialActions.Hook] = _playerInput.Movement.Hook,
            [TutorialActions.FastFall] = _playerInput.Movement.FastFall
        };
    }

    public void ShowHint(string message, TutorialActions action)
    {
        _hintDisplay.Show(message);

        _currentAction = _actionMap[action];
        _currentAction.performed += OnActionPerformed;
    }

    private void OnActionPerformed(InputAction.CallbackContext obj)
    {
        _hintDisplay.Hide();
        _currentAction.performed -= OnActionPerformed;
    }
}