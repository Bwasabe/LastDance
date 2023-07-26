using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "SO/Turret")]
public class TurretSO : ScriptableObject
{
    // Patrol State
    [field: SerializeField] public float RotationSpeed { get; private set; } = 90f;  // 회전 속도 (각도/초)
    [field: SerializeField] [field: Range(-180f, 180f)] public float MinRotateZ { get; private set; } = 0f;
    [field: SerializeField] [field: Range(-180f, 180f)] public float MaxRotateZ { get; private set; } = 180f;
    [field: SerializeField] [field: Range(-180f, 180f)] public float MinRotateX { get; private set; } = -110f;
    [field: SerializeField] [field: Range(-180f, 180f)] public float MaxRotateX { get; private set; } = -75f;
    [field: SerializeField] public float DelayMinAgainRotate { get; private set; } = 0f;
    [field: SerializeField] public float DelayMaxAgainRotate { get; private set; } = 1.5f;
    // Shoot State
    [field: SerializeField] public float ShootDelay { get; private set; } = 1f;
    [field: SerializeField] public float CheckStateDelay { get; private set; } = 1f;

}
