using UnityEngine;

[CreateAssetMenu(fileName = "Movement Config", menuName = "StaticData/Configs/Movement Config", order = 54)]
public class MovementConfig : ScriptableObject
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float SlowDownSpeed { get ; private set; }
    [field: SerializeField] public float FallSpeedXMultiplier { get ; private set; }
}
