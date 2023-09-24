using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    private float _hitStopDuration = 0.1f;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ITakeDamageable damageable))
        {
            damageable.Hit();
            
            TimeManager.Instance.StopTime(_hitStopDuration);
        }
    }
}