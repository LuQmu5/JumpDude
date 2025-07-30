using UnityEngine;

[CreateAssetMenu(fileName = "Glide Config", menuName = "StaticData/Configs/Glide Config", order = 54)]
public class GlideConfig : ScriptableObject
{
    [field: SerializeField] public float ModifiedGravityScale { get; private set; }
    [field: SerializeField] public float MovementSpeedMultiplier { get; private set; }
    [field: SerializeField] public float ChangeGravityDuration { get; private set; }
}
