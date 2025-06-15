using UnityEngine;

public interface IInputReader
{
    Vector2 MoveDirection { get; }
    bool JumpPressed { get; }
    bool DashHeld { get; }
    bool HookPressed { get; }
    bool FallFastPressed { get; }
}