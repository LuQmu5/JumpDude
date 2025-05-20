using UnityEngine;

[CreateAssetMenu(fileName = "GlideSettings", menuName = "Character/Abilities/Glide Settings")]
public class GlideSettings : ScriptableObject
{
    [Header("Glide Physics")]
    [Range(0.01f, 1f)] public float gravityScale = 0.25f;
    [Range(0.1f, 3f)] public float horizontalSpeedMultiplier = 0.5f;

    [Header("Smooth Transition")]
    public float gravityTransitionDuration = 0.25f;

    [Header("Input Settings")]
    public KeyCode glideKey = KeyCode.Space;

    [Header("Animation")]
    public string glidingBoolParam = "IsGliding";
}
