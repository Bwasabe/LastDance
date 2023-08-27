using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttackCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.transform.GetComponentInParent<TurretController>().Death();
        }
    }

}
