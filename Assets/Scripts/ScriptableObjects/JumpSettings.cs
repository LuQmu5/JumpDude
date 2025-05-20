using UnityEngine;

[CreateAssetMenu(fileName = "JumpSettings", menuName = "Character/Jump Settings")]
public class JumpSettings : ScriptableObject
{
    public float jumpPower = 5;
    public float maxJumpHoldTime = 2f;
    public float doubleJumpTimer = 0.5f;
    public float legsRadius = 1;
    public LayerMask groundMask;
}
