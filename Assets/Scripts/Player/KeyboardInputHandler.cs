using UnityEngine;

public class KeyboardInputHandler : IInputHandler
{
    private readonly PlayerKeyboardInput _inputActions;

    public KeyboardInputHandler(PlayerKeyboardInput inputActions)
    {
        _inputActions = inputActions;
    }

    public bool DashKeyPressed() => _inputActions.Player.Dash.WasPressedThisFrame();

    public bool DashKeyReleased() => _inputActions.Player.Dash.WasReleasedThisFrame();

    public bool FastFallKeyPressed() => _inputActions.Player.FastFall.WasPressedThisFrame();

    public bool FastFallKeyReleased() => _inputActions.Player.FastFall.WasReleasedThisFrame();

    public bool HookKeyPressed() => _inputActions.Player.Hook.WasPressedThisFrame();

    public bool HookKeyReleased() => _inputActions.Player.Hook.WasReleasedThisFrame();

    public bool JumpKeyPressed() => _inputActions.Player.Jump.WasPressedThisFrame();

    public bool JumpKeyReleased() => _inputActions.Player.Jump.WasReleasedThisFrame();

    public Vector2 ReadHorizontalInput() => _inputActions.Player.Move.ReadValue<Vector2>();
}