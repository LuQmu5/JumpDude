using UnityEngine;

public class KeyboardInputReader : MonoBehaviour, IInputReader
{
    public Vector2 MoveDirection => new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    public bool JumpPressed => Input.GetKeyDown(KeyCode.Space);
    public bool DashHeld => Input.GetKey(KeyCode.LeftShift);
    public bool HookPressed => Input.GetMouseButtonDown(1);
    public bool FallFastPressed => Input.GetKeyDown(KeyCode.S);
}