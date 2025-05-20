using UnityEngine;

[CreateAssetMenu(fileName = "DashSettings", menuName = "Character/Dash Settings")]
public class DashSettings : ScriptableObject
{
    public float minDashForce = 10f;
    public float maxDashForce = 25f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.75f;
    public float dashChargeTime = 1f;
    public float dashHitRadius = 0.5f;
    public LayerMask enemyLayer;
}
