using UnityEngine;

[CreateAssetMenu(fileName = "FallTimeSlow Config", menuName = "StaticData/Configs/FallTimeSlow Config", order = 54)]
public class FallTimeSlowConfig : ScriptableObject
{
    [Header("Time Scale Settings")]
    [field: SerializeField, Range(0f, 1f)] public float InitialTimeScale { get; private set; } = 0.5f;
    [field: SerializeField, Range(0f, 1f)] public float FinalTimeScale { get; private set; } = 0.2f;
    [field: SerializeField] public float TransitionDuration { get; private set; } = 0.4f;

    [Header("Trigger Conditions")]
    [field: SerializeField] public float MinFallVelocityY { get; private set; } = -1.5f;
    [field: SerializeField] public float GroundProximityDistance { get; private set; } = 1f;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    [Header("Limit")]
    [field: SerializeField] public float MaxSlowDuration { get; private set; } = 2f;
}
