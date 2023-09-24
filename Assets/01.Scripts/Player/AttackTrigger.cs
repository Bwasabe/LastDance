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
        ITakeDamageable takeDamageable = other.transform.root.GetComponentInChildren<ITakeDamageable>();

        if(takeDamageable is null) return;
        
        
        takeDamageable.Hit();
            
        TimeManager.Instance.StopTime(_hitStopDuration);

    }
}