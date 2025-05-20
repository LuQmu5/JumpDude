using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "Character/Movement Settings")]
public class MovementSettings : ScriptableObject
{
    public float movementSpeed = 5;
    public float maxSpeedMultiplier = 5;
    public float accelerationTime = 5;
}
