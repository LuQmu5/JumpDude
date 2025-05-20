using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "Character/Character Settings")]
public class CharacterSettings : ScriptableObject
{
    public MovementSettings movementSettings;
    public JumpSettings jumpSettings;
    public DashSettings dashSettings;
}
