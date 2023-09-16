using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerComponentBase
{
    private readonly int attackHash = Animator.StringToHash("Attack");

    private Animator playerAnimator;

    protected override void Start()
    {
        base.Start();

        playerAnimator = transform.GetComponentCache<Animator>();
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger(attackHash);
        }
    }
}
