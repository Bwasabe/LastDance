using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 20f;

    private void Update()
    {
        transform.Translate( moveSpeed * Time.deltaTime * Vector3.left);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        PlayerDead playerDead = other.GetComponent<PlayerDead>();

        if(playerDead is null) return;
        
        playerDead.Hit();
    }
}
