using UnityEngine;

public interface IInputHandler
{
    public Vector2 ReadHorizontalInput();
    public bool JumpKeyPressed();
    public bool JumpKeyReleased();
    public bool DashKeyPressed();
    public bool DashKeyReleased();
    public bool HookKeyPressed();
    public bool HookKeyReleased();
    public bool FastFallKeyPressed();
    public bool FastFallKeyReleased();
}
