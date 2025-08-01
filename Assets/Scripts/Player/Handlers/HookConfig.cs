﻿using UnityEngine;

[CreateAssetMenu(fileName = "Hook Config", menuName = "StaticData/Configs/Hook Config", order = 54)]
public class HookConfig : ScriptableObject
{
    [field: SerializeField] public LayerMask HookableLayer { get; private set; }
    [field: SerializeField] public float PullForce { get; private set; }
    [field: SerializeField] public float PullFinalForce { get; private set; }
    [field: SerializeField] public float MaxDistance { get; private set; }
    [field: SerializeField] public float MinDistanceY { get; private set; }
    [field: SerializeField] public float MinTotalDistance { get; private set; }
    [field: SerializeField] public float HookSpeed { get; private set; }
}
