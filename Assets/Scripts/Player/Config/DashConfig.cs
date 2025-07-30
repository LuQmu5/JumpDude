using UnityEngine;

[CreateAssetMenu(fileName = "Dash Config", menuName = "StaticData/Configs/Dash Config", order = 54)]
public class DashConfig : ScriptableObject
{
    [field: SerializeField] public float MinHoldTime { get; private set; }
    [field: SerializeField] public float MaxHoldTime { get; private set; }
    [field: SerializeField] public float Power { get; private set; }
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField] public float Duration { get; private set; }
}
