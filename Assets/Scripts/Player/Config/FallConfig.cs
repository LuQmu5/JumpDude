using UnityEngine;

[CreateAssetMenu(fileName = "Fall Config", menuName = "StaticData/Configs/Fall Config", order = 54)]
public class FallConfig : ScriptableObject
{
    [field: SerializeField] public float GravityMultiplier { get; private set; }
    [field: SerializeField] public float AcceleratedFallForce { get; private set; }
    [field: SerializeField] public float DisableCollisionDuration { get; private set; }
    [field: SerializeField] public LayerMask PlatformLayer { get; private set; }
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
}
