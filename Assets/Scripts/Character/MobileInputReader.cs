using UnityEngine;

public class MobileInputReader : MonoBehaviour, IInputReader
{
    public Vector2 move;
    public bool jumpPressed;
    public bool dashHeld;
    public bool hookPressed;
    public bool fallFastPressed;

    public Vector2 MoveDirection => move;
    public bool JumpPressed => jumpPressed;
    public bool DashHeld => dashHeld;
    public bool HookPressed => hookPressed;
    public bool FallFastPressed => fallFastPressed;

    // Кнопки вызывают методы, например:
    public void OnJumpButtonDown() => jumpPressed = true;
    public void OnJumpButtonUp() => jumpPressed = false;
}

