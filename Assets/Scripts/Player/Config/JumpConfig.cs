using UnityEngine;

[CreateAssetMenu(fileName = "Jump Config", menuName = "StaticData/Configs/Jump Config", order = 54)]
public class JumpConfig : ScriptableObject
{
    [field: SerializeField] public float Power { get; private set; }
    [field: SerializeField] public float ExtraPower { get; private set; }
    [field: SerializeField] public int ExtraJumpsCount { get; private set; }
    [field: SerializeField] public float MinHoldTime { get; private set; }
    [field: SerializeField] public float MaxHoldTime { get; private set; }
}

