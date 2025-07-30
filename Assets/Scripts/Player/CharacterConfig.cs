using UnityEngine;

[CreateAssetMenu(fileName = "Character Config", menuName = "StaticData/Configs/Character Config", order = 54)]
public class CharacterConfig : ScriptableObject
{
    [field: SerializeField] public DashConfig DashConfig { get; private set; }
    [field: SerializeField] public GlideConfig GlideConfig { get; private set; }
    [field: SerializeField] public GroundCheckConfig GroundCheckConfig { get; private set; }
    [field: SerializeField] public JumpConfig JumpConfig { get; private set; }
    [field: SerializeField] public MovementConfig MovementConfig { get; private set; }
}
