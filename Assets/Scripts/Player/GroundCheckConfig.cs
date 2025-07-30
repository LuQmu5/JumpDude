using UnityEngine;

[CreateAssetMenu(fileName = "Ground Check Config", menuName = "StaticData/Configs/Ground Check Config", order = 54)]
public class GroundCheckConfig : ScriptableObject
{
    [field: SerializeField] public float LegsRadius { get; private set; }
    [field: SerializeField] public LayerMask GroundMask { get; private set; }
}
